//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor;

namespace ArborEditor.UIElements
{
	internal sealed class AutoScrollElement : VisualElement
	{
		private static readonly long kUpdateInterval = 10;

		private readonly VisualElement _ContentViewport;
		private readonly System.Action<Vector2> _OnScroll;

		private bool _IsAutoScrolling = false;

		private IVisualElementScheduledItem _UpdateScheduled;
		private VisualElement _CaptureElement;

		public AutoScrollElement(VisualElement contentViewport, System.Action<Vector2> onScroll)
		{
			pickingMode = PickingMode.Ignore;

			_ContentViewport = contentViewport;
			_OnScroll = onScroll;

			AddToClassList("auto-scroll");

			RegisterCallback<AttachToPanelEvent>(OnAttachToPanel);
			RegisterCallback<DetachFromPanelEvent>(OnDetachFromPanel);
		}

		void OnAttachToPanel(AttachToPanelEvent e)
		{
			VisualElement captureElement = panel.GetCapturingElement(PointerId.mousePointerId) as VisualElement;
			SetCaptureElement(captureElement);

			_ContentViewport.RegisterCallback<MouseCaptureEvent>(OnMouseCapture);
			RegisterCallbackMouseEvent(_ContentViewport, TrickleDown.TrickleDown);
		}

		void OnDetachFromPanel(DetachFromPanelEvent e)
		{
			StopAutoScroll();

			_IsAutoScrolling = false;
			EnableInClassList("auto-scroll-active", _IsAutoScrolling);

			if (_CaptureElement != null)
			{
				UnregisterCallbackMouseEvent(_CaptureElement);
				_CaptureElement = null;
			}

			_ContentViewport.UnregisterCallback<MouseCaptureEvent>(OnMouseCapture);
			UnregisterCallbackMouseEvent(_ContentViewport, TrickleDown.TrickleDown);
		}

		void UnregisterCallbackCapture()
		{
			if (_CaptureElement != null)
			{
				_CaptureElement.UnregisterCallback<MouseCaptureOutEvent>(OnMouseCaptureOut);
				UnregisterCallbackMouseEvent(_CaptureElement);
				_CaptureElement = null;
			}
		}

		void SetCaptureElement(VisualElement captureElement)
		{
			if (captureElement != _CaptureElement)
			{
				UnregisterCallbackCapture();

				_CaptureElement = captureElement;

				if (_CaptureElement != null)
				{
					_CaptureElement.RegisterCallback<MouseCaptureOutEvent>(OnMouseCaptureOut);
					RegisterCallbackMouseEvent(_CaptureElement);
				}
			}
		}

		void OnMouseCapture(MouseCaptureEvent e)
		{
			VisualElement captureElement = e.target as VisualElement;
			SetCaptureElement(captureElement);
		}

		void OnMouseCaptureOut(MouseCaptureOutEvent e)
		{
			UnregisterCallbackCapture();
		}

		void RegisterCallbackMouseEvent(VisualElement target, TrickleDown trickleDown = TrickleDown.NoTrickleDown)
		{
			target.RegisterCallback<MouseMoveEvent>(OnMouseMove, trickleDown);
			target.RegisterCallback<DragUpdatedEvent>(OnMouseMove, trickleDown);
		}

		void UnregisterCallbackMouseEvent(VisualElement target, TrickleDown trickleDown = TrickleDown.NoTrickleDown)
		{
			target.UnregisterCallback<MouseMoveEvent>(OnMouseMove, trickleDown);
			target.UnregisterCallback<DragUpdatedEvent>(OnMouseMove, trickleDown);
		}

		void StartAutoScroll()
		{
			if (_UpdateScheduled == null)
			{
				_UpdateScheduled = schedule.Execute(OnAutoScroll).Every(kUpdateInterval);
			}
			else
			{
				if (!_UpdateScheduled.isActive)
				{
					_UpdateScheduled.Resume();
				}
			}
		}

		void StopAutoScroll()
		{
			_UpdateScheduled?.Pause();
		}

		void OnAutoScroll(TimerState state)
		{
			if (_Velocity.sqrMagnitude > 0.0f)
			{
				Vector2 move = _Velocity * state.deltaTime;

				_OnScroll?.Invoke(move);
			}
		}

		private Vector2 _Velocity;

		void UpdateMousePosition(IMouseEvent e)
		{
			var evtBase = e as EventBase;
			var eventTarget = evtBase.currentTarget as VisualElement;

			Vector2 mousePosition = eventTarget.ChangeCoordinatesTo(this, e.localMousePosition);
			Rect noScrollArea = this.contentRect;

			if (noScrollArea.Contains(mousePosition))
			{
				if (!_IsAutoScrolling)
				{
					_IsAutoScrolling = true;
					EnableInClassList("auto-scroll-active", _IsAutoScrolling);
				}

				_Velocity = Vector2.zero;
				StopAutoScroll();
				return;
			}

			if (_IsAutoScrolling)
			{
				StartAutoScroll();
			}

			Vector2 offset = Vector2.zero;

			if (mousePosition.x < noScrollArea.xMin)
			{
				offset.x = mousePosition.x - noScrollArea.xMin;
			}
			else if (noScrollArea.xMax < mousePosition.x)
			{
				offset.x = mousePosition.x - noScrollArea.xMax;
			}

			if (mousePosition.y < noScrollArea.yMin)
			{
				offset.y = mousePosition.y - noScrollArea.yMin;
			}
			else if (noScrollArea.yMax < mousePosition.y)
			{
				offset.y = mousePosition.y - noScrollArea.yMax;
			}

			offset.x = Mathf.Clamp(offset.x, -10.0f, 10.0f);
			offset.y = Mathf.Clamp(offset.y, -10.0f, 10.0f);

			_Velocity = offset * 0.1f;
		}

		void OnMouseMove(IMouseEvent e)
		{
			UpdateMousePosition(e);
		}
	}
}
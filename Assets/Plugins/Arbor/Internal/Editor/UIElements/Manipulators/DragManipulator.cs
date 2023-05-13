//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace ArborEditor.UIElements
{
	internal abstract class DragManipulator : MouseManipulator
	{
		public event System.Action<bool> onChangedActive = null;

		private bool _IsActive = false;
		public bool isActive
		{
			get
			{
				return _IsActive;
			}
			private set
			{
				if (_IsActive != value)
				{
					_IsActive = value;
					onChangedActive?.Invoke(_IsActive);
				}
			}
		}

		public enum TrickleDownMode
		{
			NoTrickleDown,
			TrickleDown,
			Both,
		}

		private readonly TrickleDownMode _TrickleDownMode = TrickleDownMode.NoTrickleDown;
		private readonly int _ControlID;

		public DragManipulator() : this(TrickleDownMode.NoTrickleDown)
		{
		}

		public DragManipulator(TrickleDownMode trickleDownMode)
		{
			_TrickleDownMode = trickleDownMode;

			// The drag being edited in the prefab is always saved, so wait until the drag is finished.
			// To wait until the drag is finished, you need to set GUIUtility.hotControl.
			// Via SearchField to get the controlID with GUIUtility.GetPermanentControlID().
			var searchField = new UnityEditor.IMGUI.Controls.SearchField();
			_ControlID = searchField.searchFieldControlID;
		}

		protected override void RegisterCallbacksOnTarget()
		{
			switch (_TrickleDownMode)
			{
				case TrickleDownMode.NoTrickleDown:
					target.RegisterCallback<MouseDownEvent>(OnMouseDownEvent, TrickleDown.NoTrickleDown);
					break;
				case TrickleDownMode.TrickleDown:
					target.RegisterCallback<MouseDownEvent>(OnMouseDownEvent, TrickleDown.TrickleDown);
					break;
				case TrickleDownMode.Both:
					target.RegisterCallback<MouseDownEvent>(OnMouseDownEvent, TrickleDown.TrickleDown);
					target.RegisterCallback<MouseDownEvent>(OnMouseDownEvent, TrickleDown.NoTrickleDown);
					break;
			}
			target.RegisterCallback<MouseMoveEvent>(OnMouseMoveEvent);
			target.RegisterCallback<MouseUpEvent>(OnMouseUpEvent);
			target.RegisterCallback<MouseCaptureOutEvent>(OnMouseCaptureOutEvent);
		}

		protected override void UnregisterCallbacksFromTarget()
		{
			switch (_TrickleDownMode)
			{
				case TrickleDownMode.NoTrickleDown:
					target.UnregisterCallback<MouseDownEvent>(OnMouseDownEvent, TrickleDown.NoTrickleDown);
					break;
				case TrickleDownMode.TrickleDown:
					target.UnregisterCallback<MouseDownEvent>(OnMouseDownEvent, TrickleDown.TrickleDown);
					break;
				case TrickleDownMode.Both:
					target.UnregisterCallback<MouseDownEvent>(OnMouseDownEvent, TrickleDown.TrickleDown);
					target.UnregisterCallback<MouseDownEvent>(OnMouseDownEvent, TrickleDown.NoTrickleDown);
					break;
			}
			target.UnregisterCallback<MouseMoveEvent>(OnMouseMoveEvent);
			target.UnregisterCallback<MouseUpEvent>(OnMouseUpEvent);
			target.UnregisterCallback<MouseCaptureOutEvent>(OnMouseCaptureOutEvent);
		}

		protected abstract void OnMouseDown(MouseDownEvent e);

		protected abstract void OnMouseMove(MouseMoveEvent e);

		protected virtual void OnMouseUp(MouseUpEvent e)
		{
		}

		protected virtual void OnMouseCaptureOut(MouseCaptureOutEvent e)
		{
		}

		protected virtual void OnStartDrag()
		{

		}

		void StartDrag()
		{
			if (isActive)
			{
				return;
			}

			isActive = true;
			target.CaptureMouse();
			GUIUtility.hotControl = _ControlID;

			OnStartDrag();
		}

		protected virtual void OnEndDrag()
		{
		}

		void EndDragInternal()
		{
			OnEndDrag();
			isActive = false;

			GUIUtility.hotControl = 0;
		}

		public void EndDrag()
		{
			if (!isActive)
			{
				return;
			}

			EndDragInternal();
			target.ReleaseMouse();
		}

		void OnMouseDownEvent(MouseDownEvent e)
		{
			if (isActive)
			{
				e.StopImmediatePropagation();
				return;
			}

			IPanel panel = (e.target as VisualElement)?.panel;
			if (panel.GetCapturingElement(PointerId.mousePointerId) != null)
			{
				return;
			}

			if (!CanStartManipulation(e))
			{
				return;
			}

			OnMouseDown(e);

			if (e.isPropagationStopped)
			{
				StartDrag();
			}
		}

		void OnMouseMoveEvent(MouseMoveEvent e)
		{
			if (!isActive)
			{
				return;
			}

			OnMouseMove(e);
		}

		void OnMouseUpEvent(MouseUpEvent e)
		{
			if (!isActive)
			{
				return;
			}

			if (!CanStopManipulation(e))
			{
				return;
			}

			OnMouseUp(e);

			EndDrag();
			e.StopPropagation();
		}

		void OnMouseCaptureOutEvent(MouseCaptureOutEvent e)
		{
			if (isActive)
			{
				OnMouseCaptureOut(e);

				EndDragInternal();
			}
		}
	}
}
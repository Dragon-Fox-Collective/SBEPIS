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
	using ArborEditor.UnityEditorBridge.UIElements.Extensions;

	internal sealed class RectangleSelector : DragOnGraphManipulator
	{
		private readonly VisualElement _RectangleElement;
		
		GraphView _GraphView;

		private Vector2 _StartPosition;
		private Vector2 _EndPosition;
		private bool _ActionKey;
		private bool _ShiftKey;

		public RectangleSelector(GraphView graphView)
		{
			_GraphView = graphView;

			activators.Add(new ManipulatorActivationFilter { button = MouseButton.LeftMouse });
			activators.Add(new ManipulatorActivationFilter { button = MouseButton.LeftMouse, modifiers = EventModifiers.Shift });
			if (Application.platform == RuntimePlatform.OSXEditor || Application.platform == RuntimePlatform.OSXPlayer)
			{
				activators.Add(new ManipulatorActivationFilter { button = MouseButton.LeftMouse, modifiers = EventModifiers.Command });
			}
			else
			{
				activators.Add(new ManipulatorActivationFilter { button = MouseButton.LeftMouse, modifiers = EventModifiers.Control });
			}

			_RectangleElement = new VisualElement()
			{
				pickingMode = PickingMode.Ignore,
			};
			_RectangleElement.AddToClassList("selector");
		}

		protected override void RegisterCallbacksOnTarget()
		{
			base.RegisterCallbacksOnTarget();
			target.RegisterCallback<KeyDownEvent>(OnKeyDown);
			target.RegisterCallback<KeyUpEvent>(OnKeyUp);
		}

		protected override void UnregisterCallbacksFromTarget()
		{
			base.UnregisterCallbacksFromTarget();
			target.UnregisterCallback<KeyDownEvent>(OnKeyDown);
			target.UnregisterCallback<KeyUpEvent>(OnKeyUp);
		}

		protected override void OnChangeGraphView(IChangeGraphViewEvent e)
		{
			if (!(e is ChangeGraphScrollEvent))
			{
				return;
			}

			EventBase evtBase = e as EventBase;
			GraphView graphView = evtBase.target as GraphView;

			Vector2 localMousePosition = graphView.GetMousePosition(target);
			UpdateSelection(localMousePosition, _ActionKey, _ShiftKey);
		}

		protected override void OnMouseDown(MouseDownEvent e)
		{
			Undo.IncrementCurrentGroup();

			_ActionKey = e.actionKey;
			_ShiftKey = e.shiftKey;

			_GraphView.BeginSelection(!_ActionKey && !_ShiftKey);

			_GraphView.contentViewport.Add(_RectangleElement);

			_StartPosition = _GraphView.ElementToGraph(e.currentTarget as VisualElement, e.localMousePosition);
			_EndPosition = _StartPosition;

			Rect selectionRect = RectUtility.FromToRect(_StartPosition, _EndPosition);
			selectionRect = _GraphView.GraphToElement(_RectangleElement.parent, selectionRect);
			_RectangleElement.SetLayout(selectionRect);

			e.StopPropagation();
		}

		bool UpdateSelection(Vector2 mousePosition, bool actionKey, bool shiftKey)
		{
			if (!isActive)
			{
				return false;
			}

			_EndPosition = _GraphView.ElementToGraph(target, mousePosition);

			Rect selectionRect = RectUtility.FromToRect(_StartPosition, _EndPosition);

			_GraphView.SelectNodesInRect(selectionRect, actionKey, shiftKey);

			selectionRect = _GraphView.GraphToElement(_RectangleElement.parent, selectionRect);
			_RectangleElement.SetLayout(selectionRect);

			return true;
		}

		protected override void OnMouseMove(MouseMoveEvent e)
		{
			_ActionKey = e.actionKey;
			_ShiftKey = e.shiftKey;

			if (UpdateSelection(e.localMousePosition, _ActionKey, _ShiftKey))
			{
				e.StopPropagation();
			}
		}

		protected override void OnMouseUp(MouseUpEvent e)
		{
			_GraphView.EndSelection();
		}

		protected override void OnMouseCaptureOut(MouseCaptureOutEvent e)
		{
			base.OnMouseCaptureOut(e);

			_GraphView.CancelSelection();
		}

		protected override void OnEndDrag()
		{
			base.OnEndDrag();

			_RectangleElement.RemoveFromHierarchy();
		}

		private void OnKeyDown(KeyDownEvent e)
		{
			if (!isActive)
			{
				return;
			}

			_ActionKey = e.actionKey;
			_ShiftKey = e.shiftKey;

			if (e.keyCode != KeyCode.Escape)
			{
				Vector2 localMousePosition = target.WorldToLocal(e.originalMousePosition);
				UpdateSelection(localMousePosition, _ActionKey, _ShiftKey);
				return;
			}

			_GraphView.CancelSelection();

			EndDrag();
			e.StopPropagation();
		}

		private void OnKeyUp(KeyUpEvent e)
		{
			if (!isActive)
			{
				return;
			}

			_ActionKey = e.actionKey;
			_ShiftKey = e.shiftKey;

			if (e.keyCode != KeyCode.Escape)
			{
				Vector2 localMousePosition = target.WorldToLocal(e.originalMousePosition);
				UpdateSelection(localMousePosition, _ActionKey, _ShiftKey);
				return;
			}
		}
	}
}
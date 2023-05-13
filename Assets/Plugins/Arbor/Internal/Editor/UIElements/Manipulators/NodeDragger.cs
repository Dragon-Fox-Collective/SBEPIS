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
	using Arbor;
	using ArborEditor.UnityEditorBridge.UIElements.Extensions;

	internal sealed class NodeDragger : DragOnGraphManipulator
	{
		public NodeDragger()
		{
			activators.Add(new ManipulatorActivationFilter { button = MouseButton.LeftMouse });
			activators.Add(new ManipulatorActivationFilter { button = MouseButton.LeftMouse, modifiers = EventModifiers.Shift });
			activators.Add(new ManipulatorActivationFilter { button = MouseButton.LeftMouse, modifiers = EventModifiers.Alt });
			if (Application.platform == RuntimePlatform.OSXEditor || Application.platform == RuntimePlatform.OSXPlayer)
			{
				activators.Add(new ManipulatorActivationFilter { button = MouseButton.LeftMouse, modifiers = EventModifiers.Command });
			}
			else
			{
				activators.Add(new ManipulatorActivationFilter { button = MouseButton.LeftMouse, modifiers = EventModifiers.Control });
			}
		}

		protected override void RegisterCallbacksOnTarget()
		{
			base.RegisterCallbacksOnTarget();
			target.RegisterCallback<KeyDownEvent>(OnKeyDown);
		}

		protected override void UnregisterCallbacksFromTarget()
		{
			base.RegisterCallbacksOnTarget();
			target.UnregisterCallback<KeyDownEvent>(OnKeyDown);
		}

		protected override void OnChangeGraphView(IChangeGraphViewEvent e)
		{
			if (!(e is ChangeGraphScrollEvent))
			{
				return;
			}

			EventBase evtBase = e as EventBase;
			GraphView graphView = evtBase.target as GraphView;

			NodeElement nodeElement = target as NodeElement;
			NodeEditor nodeEditor = nodeElement.nodeEditor;
			NodeGraphEditor graphEditor = nodeEditor.graphEditor;
			
			graphEditor.OnDragNodes(graphView.mousePosition);
		}

		protected override void OnMouseDown(MouseDownEvent e)
		{
			NodeElement nodeElement = target as NodeElement;
			if (nodeElement == null)
			{
				return;
			}

			NodeEditor nodeEditor = nodeElement.nodeEditor;
			NodeGraphEditor graphEditor = nodeEditor.graphEditor;
			GraphView graphView = graphEditor.graphView;

			Rect selectRect = nodeEditor.GetSelectableRect();
			selectRect = graphView.GraphToElement(target, selectRect);
			if (!selectRect.Contains(e.localMousePosition))
			{
				return;
			}

			Node selectTargetNode = nodeEditor.node;

			Vector2 graphMousePosition = graphView.ElementToGraph(target, e.localMousePosition);

			graphEditor.OnBeginDragNodes(selectTargetNode, graphMousePosition, e.actionKey, e.shiftKey, e.altKey);

			e.StopPropagation();
		}

		protected override void OnMouseMove(MouseMoveEvent e)
		{
			NodeElement nodeElement = target as NodeElement;

			NodeEditor nodeEditor = nodeElement.nodeEditor;
			NodeGraphEditor graphEditor = nodeEditor.graphEditor;
			GraphView graphView = graphEditor.graphView;

			Vector2 graphMousePosition = graphView.ElementToGraph(target, e.localMousePosition);

			graphEditor.OnDragNodes(graphMousePosition);

			e.StopPropagation();
		}

		protected override void OnMouseUp(MouseUpEvent e)
		{
			NodeElement nodeElement = target as NodeElement;

			NodeEditor nodeEditor = nodeElement.nodeEditor;
			NodeGraphEditor graphEditor = nodeEditor.graphEditor;

			Vector2 screenMousePosition = target.LocalToScreen(e.localMousePosition);

			graphEditor.OnEndDragNodes(screenMousePosition);
		}

		protected override void OnMouseCaptureOut(MouseCaptureOutEvent e)
		{
			base.OnMouseCaptureOut(e);

			NodeElement nodeElement = target as NodeElement;

			NodeEditor nodeEditor = nodeElement.nodeEditor;
			NodeGraphEditor graphEditor = nodeEditor.graphEditor;

			graphEditor.OnEndDragNodes();
		}

		void OnKeyDown(KeyDownEvent e)
		{
			if (!isActive)
			{
				return;
			}

			if (e.keyCode != KeyCode.Escape)
			{
				return;
			}

			NodeElement nodeElement = target as NodeElement;

			NodeEditor nodeEditor = nodeElement.nodeEditor;
			NodeGraphEditor graphEditor = nodeEditor.graphEditor;

			graphEditor.OnEscapeDragNodes();

			EndDrag();

			e.StopPropagation();
		}
	}
}
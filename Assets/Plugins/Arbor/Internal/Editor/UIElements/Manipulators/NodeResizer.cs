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
	using ArborEditor.UnityEditorBridge.UIElements.Extensions;

	internal sealed class NodeResizer : DragManipulator
	{
		private readonly NodeEditor _NodeEditor;
		private readonly ResizeDirection _Direction;

		private Vector2 _StartMouse;
		private Rect _StartRect;
		private Vector2 _MinSize;

		public NodeResizer(NodeEditor nodeEditor, ResizeDirection direction)
		{
			activators.Add(new ManipulatorActivationFilter { button = MouseButton.LeftMouse });

			_NodeEditor = nodeEditor;
			_Direction = direction;
		}

		protected override void OnMouseDown(MouseDownEvent e)
		{
			if (_NodeEditor == null)
				return;

			NodeElement nodeElement = _NodeEditor.nodeElement;

			VisualElement resizedBase = nodeElement.parent;
			if (resizedBase == null)
				return;

			Rect nodePosition = _NodeEditor.rect;

			_StartRect = nodePosition;
			_StartMouse = resizedBase.WorldToLocal(e.mousePosition);

			_MinSize = new Vector2(_NodeEditor.Internal_GetWidth(), 100f);

			_NodeEditor.OnStartResize();

			e.StopPropagation();
		}

		protected override void OnMouseMove(MouseMoveEvent e)
		{
			NodeElement nodeElement = _NodeEditor.nodeElement;

			VisualElement resizedBase = nodeElement.parent;

			Vector2 mousePos = resizedBase.WorldToLocal(e.mousePosition);

			Rect newNodePosition = _NodeEditor.rect;

			Vector2 dragNodeDistance = mousePos - _StartMouse;

			if ((_Direction & ResizeDirection.Top) == ResizeDirection.Top)
			{
				newNodePosition.yMin = _StartRect.yMin + dragNodeDistance.y;
				if (newNodePosition.size.y <= _MinSize.y)
				{
					newNodePosition.yMin = newNodePosition.yMax - _MinSize.y;
				}
				newNodePosition.yMin = EditorGUITools.SnapToGrid(newNodePosition.yMin);
			}
			if ((_Direction & ResizeDirection.Bottom) == ResizeDirection.Bottom)
			{
				newNodePosition.yMax = _StartRect.yMax + dragNodeDistance.y;
				if (newNodePosition.size.y <= _MinSize.y)
				{
					newNodePosition.yMax = newNodePosition.yMin + _MinSize.y;
				}
				newNodePosition.yMax = EditorGUITools.SnapToGrid(newNodePosition.yMax);
			}
			if ((_Direction & ResizeDirection.Left) == ResizeDirection.Left)
			{
				newNodePosition.xMin = _StartRect.xMin + dragNodeDistance.x;
				if (newNodePosition.size.x <= _MinSize.x)
				{
					newNodePosition.xMin = newNodePosition.xMax - _MinSize.x;
				}
				newNodePosition.xMin = EditorGUITools.SnapToGrid(newNodePosition.xMin);
			}
			if ((_Direction & ResizeDirection.Right) == ResizeDirection.Right)
			{
				newNodePosition.xMax = _StartRect.xMax + dragNodeDistance.x;
				if (newNodePosition.size.x <= _MinSize.x)
				{
					newNodePosition.xMax = newNodePosition.xMin + _MinSize.x;
				}
				newNodePosition.xMax = EditorGUITools.SnapToGrid(newNodePosition.xMax);
			}

			_NodeEditor.OnResizing(newNodePosition);

			e.StopPropagation();
		}
	}
}
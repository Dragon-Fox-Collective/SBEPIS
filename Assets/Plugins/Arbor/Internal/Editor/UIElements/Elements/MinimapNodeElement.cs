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

	internal sealed class MinimapNodeElement : VisualElement
	{
		private NodeElement _NodeElement;

		private MinimapView _MinimapView;

		public MinimapNodeElement(NodeElement nodeElement)
		{
			_NodeElement = nodeElement;

			AddToClassList("node-element");

			RegisterCallback<AttachToPanelEvent>(OnAttachToPanel);
			RegisterCallback<DetachFromPanelEvent>(OnDetachFromPanel);
		}

		void RegisterToMinimapView()
		{
			_MinimapView = GetFirstOfType<MinimapView>();

			if (_MinimapView != null)
			{
				_MinimapView.RegisterCallback<MinimapTransformEvent>(OnMinimapTransform);
			}
		}

		void UnregisterFromMinimap()
		{
			if (_MinimapView != null)
			{
				_MinimapView.UnregisterCallback<MinimapTransformEvent>(OnMinimapTransform);
				_MinimapView = null;
			}
		}

		void OnAttachToPanel(AttachToPanelEvent e)
		{
			UnregisterFromMinimap();
			RegisterToMinimapView();

			_NodeElement.RegisterCallback<ChangeNodePositionEvent>(OnChangeNodePosition);
			_NodeElement.RegisterCallback<GeometryChangedEvent>(OnGeometryChangedNode);
			_NodeElement.RegisterCallback<MouseDownEvent>(OnMouseDownNodeElement, TrickleDown.TrickleDown);
		}

		void OnDetachFromPanel(DetachFromPanelEvent e)
		{
			UnregisterFromMinimap();

			_NodeElement.UnregisterCallback<ChangeNodePositionEvent>(OnChangeNodePosition);
			_NodeElement.UnregisterCallback<GeometryChangedEvent>(OnGeometryChangedNode);
			_NodeElement.UnregisterCallback<MouseDownEvent>(OnMouseDownNodeElement);
		}

		void OnChangeNodePosition(ChangeNodePositionEvent e)
		{
			UpdateLayout();
		}

		void OnGeometryChangedNode(GeometryChangedEvent e)
		{
			UpdateLayout();
		}

		void OnMouseDownNodeElement(MouseDownEvent e)
		{
			BringToFront();
		}

		void OnMinimapTransform(MinimapTransformEvent e)
		{
			UpdateLayout();
		}

		void UpdateLayout()
		{
			if (_MinimapView == null)
			{
				return;
			}

			Rect nodeRect = _MinimapView.GraphToMinimap(_NodeElement.rectOnGraph);
			this.SetLayout(nodeRect);
		}
	}
}
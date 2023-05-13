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
	internal sealed class PopupButtonElement : VisualElement
	{
		private static int s_PopupButtonElementHash = "s_PopupButtonElementHash".GetHashCode();

		private GraphView _GraphView;

		private Vector2 _AttachPoint;

		public Vector2 attachPoint
		{
			get
			{
				return _AttachPoint;
			}
			set
			{
				if (_AttachPoint != value)
				{
					_AttachPoint = value;

					UpdatePosition();
				}
			}
		}
		public GUIContent content;
		public GUIStyle buttonStyle;
		public System.Action<Rect> onClick;
		public int activeControlID;

		public PopupButtonElement(GraphView graphView)
		{
			_GraphView = graphView;
			Add(new NodeContentIMGUIContainer(OnGUI));

			RegisterCallback<AttachToPanelEvent>(OnAttachToPanel);
			RegisterCallback<DetachFromPanelEvent>(OnDetachFromPanel);
			RegisterCallback<MouseLeaveEvent>(OnMouseLeave);
			RegisterCallback<GeometryChangedEvent>(OnGeometryChanged);
		}

		void OnAttachToPanel(AttachToPanelEvent evt)
		{
			_GraphView.RegisterCallback<ChangeGraphScrollEvent>(OnChangeGraphView);
			_GraphView.RegisterCallback<ChangeGraphExtentsEvent>(OnChangeGraphView);
		}

		void OnDetachFromPanel(DetachFromPanelEvent evt)
		{
			_GraphView.UnregisterCallback<ChangeGraphScrollEvent>(OnChangeGraphView);
			_GraphView.UnregisterCallback<ChangeGraphExtentsEvent>(OnChangeGraphView);
		}

		void OnGeometryChanged(GeometryChangedEvent evt)
		{
			UpdatePosition();
		}

		void OnChangeGraphView(IChangeGraphViewEvent e)
		{
			UpdatePosition();
		}

		void UpdatePosition()
		{
			if (parent != null)
			{
				Vector2 pos = _GraphView.GraphToElement(hierarchy.parent, attachPoint);
				Vector2 size = new Vector2(resolvedStyle.width, resolvedStyle.height);
				pos -= size * 0.5f;

				transform.position = pos;
			}
		}

		private void OnMouseLeave(MouseLeaveEvent evt)
		{
			RemoveFromHierarchy();
		}

		private void OnGUI()
		{
			Event current = Event.current;

			Rect position = GUILayoutUtility.GetRect(content, buttonStyle);

			int controlID = GUIUtility.GetControlID(s_PopupButtonElementHash, FocusType.Passive, position);

			EventType typeForControl = current.GetTypeForControl(controlID);
			switch (typeForControl)
			{
				case EventType.MouseDown:
					if (position.Contains(current.mousePosition))
					{
						onClick?.Invoke(position);

						current.Use();

						RemoveFromHierarchy();
					}
					break;
				case EventType.Repaint:
					EditorGUI.DropShadowLabel(position, content, buttonStyle);
					break;
			}
		}
	}
}

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
	using Arbor;

	internal sealed class NodeCommentElement : VisualElement
	{
		private NodeEditor _NodeEditor;

		public Node node
		{
			get
			{
				return _NodeEditor.node;
			}
		}

		public NodeGraphEditor graphEditor
		{
			get
			{
				return _NodeEditor.graphEditor;
			}
		}

		public NodeCommentElement(NodeEditor nodeEditor)
		{
			_NodeEditor = nodeEditor;

			// Use IMGUI because there is a problem that IME undetermined characters are not displayed in TextField of UIElements.
			// Reproducible with: Unity2019.4.40f1, Unity2020.3.46f1, Unity2021.3.20f1
			// Not reproducible with: Unity2022.2.10f1
			//   However, there is a bug that the underline is not displayed in the unconfirmed text

			Add(new NodeContentIMGUIContainer(OnGUI));
			//onGUIHandler = OnGUI;

			style.position = Position.Absolute;

			RegisterCallback<AttachToPanelEvent>(OnAttachToPanel);
			RegisterCallback<DetachFromPanelEvent>(OnDetachFromPanel);
			RegisterCallback<GeometryChangedEvent>(OnGeometryChanged);
		}

		void OnAttachToPanel(AttachToPanelEvent evt)
		{
			GraphView graphView = graphEditor.graphView;
			graphView.contentContainer.RegisterCallback<GeometryChangedEvent>(OnGraphViewGeometryChanged);

			graphView.onChangedGraphExtents -= OnChangedGraphExtents;
			graphView.onChangedGraphExtents += OnChangedGraphExtents;

			graphView.onChangedGraphPosition -= OnChangedGraphPosition;
			graphView.onChangedGraphPosition += OnChangedGraphPosition;

			_NodeEditor.nodeElement.RegisterCallback<ChangeNodePositionEvent>(OnChangedNodePosition);

			UpdatePosition();
		}

		void OnDetachFromPanel(DetachFromPanelEvent evt)
		{
			GraphView graphView = graphEditor.graphView;
			graphView.contentContainer.UnregisterCallback<GeometryChangedEvent>(OnGraphViewGeometryChanged);
			graphView.onChangedGraphExtents -= OnChangedGraphExtents;
			graphView.onChangedGraphPosition -= OnChangedGraphPosition;

			_NodeEditor.nodeElement.UnregisterCallback<ChangeNodePositionEvent>(OnChangedNodePosition);
		}

		void OnGraphViewGeometryChanged(GeometryChangedEvent evt)
		{
			UpdatePosition();
		}

		void OnGeometryChanged(GeometryChangedEvent evt)
		{
			UpdatePosition();
		}

		void OnChangedGraphExtents()
		{
			UpdatePosition();
		}

		void OnChangedGraphPosition()
		{
			UpdatePosition();
		}

		void OnChangedNodePosition(ChangeNodePositionEvent e)
		{
			UpdatePosition();
		}

		void UpdatePosition()
		{
			if (hierarchy.parent == null)
			{
				return;
			}

			// Adjust the position using the size automatically calculated by OnGUI.
			Vector2 nodePosition = _NodeEditor.position;
			nodePosition = graphEditor.graphView.GraphToElement(hierarchy.parent, nodePosition);

			Vector2 size = new Vector2(resolvedStyle.width, resolvedStyle.height);
			Rect commentRect = new Rect(nodePosition, size);
			commentRect.y -= commentRect.height + 4f;

			transform.position = commentRect.position;
		}

		void OnGUI()
		{
			if (graphEditor.nodeGraph == null)
			{
				return;
			}

			// GUI Layout is recommended because the size of IMGUI Container is automatically measured by OnGUI.
			Node node = this.node;

			EditorGUI.BeginDisabledGroup(!graphEditor.editable);

			EditorGUI.BeginChangeCheck();
			string nodeComment = EditorGUILayout.TextArea(node.nodeComment, Styles.nodeCommentField, GUILayout.MinWidth(50f));
			if (EditorGUI.EndChangeCheck())
			{
				Undo.RecordObject(node.nodeGraph, "Edit Node Comment");

				node.nodeComment = nodeComment;

				EditorUtility.SetDirty(node.nodeGraph);
			}

			EditorGUI.EndDisabledGroup();
		}
	}
}
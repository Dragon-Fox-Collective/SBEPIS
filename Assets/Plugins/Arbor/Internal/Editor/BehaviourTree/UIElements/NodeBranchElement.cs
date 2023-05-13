//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor;

namespace ArborEditor.BehaviourTree.UIElements
{
	using Arbor;
	using Arbor.BehaviourTree;
	using ArborEditor.UIElements;

	internal sealed class NodeBranchElement : VisualElement
	{
		private readonly BehaviourTreeGraphEditor _GraphEditor;
		private readonly BezierElement _BezierElement;

		public NodeBranch nodeBranch
		{
			get; private set;
		}

		private bool _IsHover;

		public NodeBranchElement(BehaviourTreeGraphEditor graphEditor)
		{
			_GraphEditor = graphEditor;
			
			_BezierElement = new BezierElement(_GraphEditor.graphView.contentContainer)
			{
				shadow = true,
				shadowColor = BehaviourTreeGraphEditor.s_BranchBezierShadowColor,
				edgeWidth = 5.0f,
				arrow = false,
			};
			Add(_BezierElement);

			RegisterCallback<MouseOverEvent>(OnMouseOver);
			RegisterCallback<MouseOutEvent>(OnMouseOut);

			this.AddManipulator(new ContextClickManipulator(OnContextClick));
		}

		void OnContextClick(ContextClickEvent evt)
		{
			NodeGraph nodeGraph = _GraphEditor.nodeGraph;
			BehaviourTreeInternal behaviourTree = nodeGraph as BehaviourTreeInternal;
			Node parentNode = nodeGraph.GetNodeFromID(nodeBranch.parentNodeID);
			Node childNode = nodeGraph.GetNodeFromID(nodeBranch.childNodeID);

			GenericMenu menu = new GenericMenu();
			menu.AddItem(GUIContentCaches.Get(Localization.GetWord("Go to Parent Node") + " : " + _GraphEditor.GetNodeTitle(parentNode)), false, () =>
			{
				_GraphEditor.BeginFrameSelected(parentNode);
			});
			menu.AddItem(GUIContentCaches.Get(Localization.GetWord("Go to Child Node") + " : " + _GraphEditor.GetNodeTitle(childNode)), false, () =>
			{
				_GraphEditor.BeginFrameSelected(childNode);
			});
			if (!nodeBranch.isActive && _GraphEditor.editable)
			{
				menu.AddItem(EditorContents.disconnect, false, () =>
				{
					Undo.IncrementCurrentGroup();
					int undoGroup = Undo.GetCurrentGroup();

					behaviourTree.DisconnectBranch(nodeBranch);
					behaviourTree.CalculatePriority();

					Undo.CollapseUndoOperations(undoGroup);
				});
			}
			else
			{
				menu.AddDisabledItem(EditorContents.disconnect);
			}

			menu.ShowAsContext();
			evt.StopPropagation();
		}

		void OnMouseOver(MouseOverEvent evt)
		{
			if (!_IsHover)
			{
				_IsHover = true;
				_GraphEditor.graphView.branchOverlayLayer.Add(this);
			}
		}

		void OnMouseOut(MouseOutEvent evt)
		{
			if (_IsHover)
			{
				_IsHover = false;
				_GraphEditor.graphView.branchUnderlayLayer.Add(this);
			}
		}

		public void Update(NodeBranch branch)
		{
			nodeBranch = branch;

			Bezier2D bezier = nodeBranch.bezier;

			Color color = BehaviourTreeGraphEditor.s_BranchBezierColor;
			if (Application.isPlaying)
			{
				if (nodeBranch.isActive)
				{
					color = BehaviourTreeGraphEditor.s_BranchBezierActiveColor;
				}
				else
				{
					color = BehaviourTreeGraphEditor.s_BranchBezierInactiveColor;
				}
			}

			_BezierElement.startPosition = bezier.startPosition;
			_BezierElement.startControl = bezier.startControl;
			_BezierElement.endPosition = bezier.endPosition;
			_BezierElement.endControl = bezier.endControl;
			_BezierElement.lineColor = color;

			_BezierElement.UpdateLayout();
		}
	}
}
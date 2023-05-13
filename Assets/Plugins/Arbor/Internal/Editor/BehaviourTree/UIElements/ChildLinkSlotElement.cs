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
	using ArborEditor.UnityEditorBridge.UIElements.Extensions;

	internal sealed class ChildLinkSlotElement : NodeLinkSlotElement
	{
		private RootNode _RootNode;

		public ChildLinkSlotElement(RootNodeEditor nodeEditor) : base(nodeEditor, false)
		{
		}

		protected override void SetNode(Node node)
		{
			RootNode rootNode = node as RootNode;
			if (_RootNode != rootNode)
			{
				_RootNode = rootNode;

				SetNodeLinkSlot(_RootNode?.GetChildLinkSlot());

				SetBranch(_RootNode?.GetChildBranch());
			}
		}

		private NodeBranch _Branch;

		void SetBranch(NodeBranch branch)
		{
			if (_Branch != branch)
			{
				if (_Branch != null)
				{
					_Branch.onActiveChanged -= OnBranchActiveChanged;
				}

				_Branch = branch;

				if (_Branch != null)
				{
					_Branch.onActiveChanged += OnBranchActiveChanged;
				}
			}
		}

		void OnBranchActiveChanged(bool active)
		{
			if (active)
			{
				CancelDrag();
			}
		}

		protected override void OnAttachToPanel(AttachToPanelEvent e)
		{
			base.OnAttachToPanel(e);

			SetBranch(_RootNode?.GetChildBranch());
		}

		protected override void OnDetachFromPanel(DetachFromPanelEvent e)
		{
			base.OnDetachFromPanel(e);

			SetBranch(null);
		}

		protected override void OnConnectionChanged()
		{
			base.OnConnectionChanged();

			SetBranch(_RootNode?.GetChildBranch());
		}

		NodeBranch GetBranch()
		{
			return _RootNode.GetChildBranch();
		}

		protected override bool CanStartDrag()
		{
			NodeBranch branch = GetBranch();
			return branch == null || !branch.isActive;
		}

		protected override bool IsConnecting()
		{
			NodeBranch branch = GetBranch();
			return branch != null;
		}

		protected override bool IsConnected(int hoverID)
		{
			NodeBranch branch = GetBranch();
			return branch != null && branch.childNodeID == hoverID;
		}

		protected override void OnPreConnect()
		{
			BehaviourTreeInternal behaviourTree = graphEditor.nodeGraph as BehaviourTreeInternal;
			NodeBranch branch = GetBranch();
			if (branch != null)
			{
				behaviourTree.DisconnectBranch(branch);
			}
		}

		protected override void OnChangedPosition()
		{
			NodeGraphEditor graphEditor = this.graphEditor;
			BehaviourTreeInternal behaviourTree = graphEditor.nodeGraph as BehaviourTreeInternal;

			NodeBranch branch = _RootNode.GetChildBranch();
			if (branch != null)
			{
				Vector2 pinPos = GetNodeLinkPinPos(layout);
				Vector2 startPosition = graphEditor.graphView.ElementToGraph(parent, pinPos);
				Vector2 startControl = startPosition + kBezierTangentOffset;

				if (branch.bezier.SetStartPoint(startPosition, startControl))
				{
					graphEditor.Repaint();
				}
			}
		}

		protected override void OnDisconnect()
		{
			NodeGraphEditor graphEditor = this.graphEditor;
			BehaviourTreeInternal behaviourTree = graphEditor.nodeGraph as BehaviourTreeInternal;

			NodeBranch branch = GetBranch();

			Undo.IncrementCurrentGroup();
			int undoGroup = Undo.GetCurrentGroup();

			behaviourTree.DisconnectBranch(branch);
			behaviourTree.CalculatePriority();

			Undo.CollapseUndoOperations(undoGroup);
		}
	}
}
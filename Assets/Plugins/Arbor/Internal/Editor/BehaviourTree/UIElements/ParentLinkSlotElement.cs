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

	internal sealed class ParentLinkSlotElement : NodeLinkSlotElement
	{
		private IParentLinkSlotHolder _ParentLinkSlotHolder;
		
		public ParentLinkSlotElement(TreeNodeBaseEditor nodeEditor) : base(nodeEditor, true)
		{
		}

		protected override void SetNode(Node node)
		{
			IParentLinkSlotHolder parentLinkSlotHolder = node as IParentLinkSlotHolder;
			if (_ParentLinkSlotHolder != parentLinkSlotHolder)
			{
				_ParentLinkSlotHolder = parentLinkSlotHolder;

				SetNodeLinkSlot(_ParentLinkSlotHolder?.GetParentLinkSlot());

				SetBranch(_ParentLinkSlotHolder?.GetParentBranch());
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

			SetBranch(_ParentLinkSlotHolder?.GetParentBranch());
		}

		protected override void OnDetachFromPanel(DetachFromPanelEvent e)
		{
			base.OnDetachFromPanel(e);

			SetBranch(null);
		}

		protected override void OnConnectionChanged()
		{
			base.OnConnectionChanged();

			SetBranch(_ParentLinkSlotHolder?.GetParentBranch());
		}

		NodeBranch GetBranch()
		{
			return _ParentLinkSlotHolder.GetParentBranch();
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
			return branch != null && branch.parentNodeID == hoverID;
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
			BehaviourTreeGraphEditor graphEditor = this.graphEditor;
			NodeBranch branch = GetBranch();
			if (branch != null)
			{
				Vector2 pinPos = GetNodeLinkPinPos(layout);
				Vector2 endPosition = graphEditor.graphView.ElementToGraph(parent, pinPos);
				Vector2 endControl = endPosition - kBezierTangentOffset;

				if (branch.bezier.SetEndPoint(endPosition, endControl))
				{
					graphEditor.Repaint();
				}
			}
		}

		protected override void OnDisconnect()
		{
			BehaviourTreeGraphEditor graphEditor = this.graphEditor;
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
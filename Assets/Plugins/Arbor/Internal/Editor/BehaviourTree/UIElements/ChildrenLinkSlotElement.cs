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

	internal sealed class ChildrenLinkSlotElement : NodeLinkSlotElement
	{
		private CompositeNode _CompositeNode;

		public ChildrenLinkSlotElement(CompositeNodeEditor nodeEditor) : base(nodeEditor, false)
		{
		}

		protected override GUIContent GetDisconnectContent()
		{
			return EditorContents.disconnectAll;
		}

		protected override void SetNode(Node Node)
		{
			CompositeNode compositeNode = Node as CompositeNode;
			if (_CompositeNode != compositeNode)
			{
				_CompositeNode = compositeNode;

				SetNodeLinkSlot(_CompositeNode?.GetChildrenLinkSlot());
			}
		}

		protected override bool IsConnecting()
		{
			var graphEditor = this.graphEditor;
			if (graphEditor == null)
			{
				return false;
			}

			var childrenLinkSlot = _CompositeNode.GetChildrenLinkSlot();
			int slotCount = childrenLinkSlot.branchIDs.Count;
			var nodeBranchis = _CompositeNode.behaviourTree.nodeBranchies;

			for (int slotIndex = 0; slotIndex < slotCount; slotIndex++)
			{
				NodeBranch branch = nodeBranchis.GetFromID(childrenLinkSlot.branchIDs[slotIndex]);
				if (branch != null)
				{
					return true;
				}
			}

			return false;
		}

		protected override bool IsConnected(int hoverID)
		{
			var childrenLinkSlot = _CompositeNode.GetChildrenLinkSlot();
			int slotCount = childrenLinkSlot.branchIDs.Count;
			var nodeBranchies = _CompositeNode.behaviourTree.nodeBranchies;

			for (int slotIndex = 0; slotIndex < slotCount; slotIndex++)
			{
				NodeBranch branch = nodeBranchies.GetFromID(childrenLinkSlot.branchIDs[slotIndex]);
				if (branch != null && branch.childNodeID == hoverID)
				{
					return true;
				}
			}

			return false;
		}

		protected override void OnChangedPosition()
		{
			NodeGraphEditor graphEditor = this.graphEditor;
			
			var childrenLinkSlot = _CompositeNode.GetChildrenLinkSlot();
			int slotCount = childrenLinkSlot.branchIDs.Count;
			var nodeBranchies = _CompositeNode.behaviourTree.nodeBranchies;
			if (slotCount > 0)
			{
				Vector2 pinPos = GetNodeLinkPinPos(layout);

				Vector2 startPosition = graphEditor.graphView.ElementToGraph(parent, pinPos);
				Vector2 startControl = startPosition + kBezierTangentOffset;

				bool changed = false;
				for (int slotIndex = 0; slotIndex < slotCount; slotIndex++)
				{
					NodeBranch branch = nodeBranchies.GetFromID(childrenLinkSlot.branchIDs[slotIndex]);
					if (branch != null)
					{
						if (branch.bezier.SetStartPoint(startPosition, startControl))
						{
							changed = true;
						}
					}
				}

				if (changed)
				{
					graphEditor.Repaint();
				}
			}

		}

		protected override void OnDisconnect()
		{
			NodeGraphEditor graphEditor = this.graphEditor;
			
			var childrenLinkSlot = _CompositeNode.GetChildrenLinkSlot();
			int slotCount = childrenLinkSlot.branchIDs.Count;
			var behaviourTree = _CompositeNode.behaviourTree;
			var nodeBranchies = behaviourTree.nodeBranchies;

			Undo.IncrementCurrentGroup();
			int undoGroup = Undo.GetCurrentGroup();

			for (int slotIndex = slotCount - 1; slotIndex >= 0; slotIndex--)
			{
				NodeBranch branch = nodeBranchies.GetFromID(childrenLinkSlot.branchIDs[slotIndex]);
				if (branch != null)
				{
					behaviourTree.DisconnectBranch(branch);
				}
			}

			behaviourTree.CalculatePriority();

			Undo.CollapseUndoOperations(undoGroup);
		}
	}
}
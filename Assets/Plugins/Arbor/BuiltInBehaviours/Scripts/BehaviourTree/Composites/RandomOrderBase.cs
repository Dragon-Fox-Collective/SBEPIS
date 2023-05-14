//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using System.Collections.Generic;

namespace Arbor.BehaviourTree.Composites
{
	public abstract class RandomOrderBase : CompositeBehaviour
	{
		private sealed class Order : System.IComparable<Order>
		{
			public int branchID;
			public float weight;

			public int CompareTo(Order other)
			{
				if (other == null)
				{
					return 1;
				}

				return weight.CompareTo(other.weight);
			}
		}

		private List<Order> _ChildNodeOrders = new List<Order>();

		private int _OrderIndex = 0;

		int GetSlotIndex(int orderIndex)
		{
			if (0 <= orderIndex && orderIndex < _ChildNodeOrders.Count)
			{
				int branchID = _ChildNodeOrders[orderIndex].branchID;

				var childrenLinkSlot = compositeNode.GetChildrenLinkSlot();
				for (int i = 0, count = childrenLinkSlot.branchIDs.Count; i < count; i++)
				{
					int slotBranchID = childrenLinkSlot.branchIDs[i];
					if (slotBranchID == branchID)
					{
						return i;
					}
				}
			}

			return -1;
		}

		public override int GetBeginIndex()
		{
			_ChildNodeOrders.Clear();

			var childrenLinkSlot = compositeNode.GetChildrenLinkSlot();
			for (int slotIndex = 0, slotCount = childrenLinkSlot.branchIDs.Count; slotIndex < slotCount; slotIndex++)
			{
				int branchID = childrenLinkSlot.branchIDs[slotIndex];
				Order order = new Order()
				{
					branchID = branchID,
					weight = Random.value,
				};
				_ChildNodeOrders.Add(order);
			}

			_ChildNodeOrders.Sort();

			_OrderIndex = 0;

			return GetSlotIndex(_OrderIndex);
		}

		public override int GetNextIndex(int index)
		{
			_OrderIndex++;

			return GetSlotIndex(_OrderIndex);
		}

		public override int GetInterruptIndex(TreeNodeBase node)
		{
			_ChildNodeOrders.Clear();

			var childrenLinkSlot = compositeNode.GetChildrenLinkSlot();
			int childCount = childrenLinkSlot.branchIDs.Count;
			var nodeBranchies = compositeNode.behaviourTree.nodeBranchies;
			for (int childIndex = 0; childIndex < childCount; ++childIndex)
			{
				NodeBranch branch = nodeBranchies.GetFromID(childrenLinkSlot.branchIDs[childIndex]);
				float weight = 0.0f;

				if (branch.childNodeID == node.nodeID)
				{
					weight = -1f;
				}
				else
				{
					weight = Random.value;
				}

				Order order = new Order()
				{
					branchID = branch.branchID,
					weight = weight,
				};

				_ChildNodeOrders.Add(order);
			}

			_ChildNodeOrders.Sort();

			_OrderIndex = 0;

			return GetSlotIndex(_OrderIndex);
		}
	}
}

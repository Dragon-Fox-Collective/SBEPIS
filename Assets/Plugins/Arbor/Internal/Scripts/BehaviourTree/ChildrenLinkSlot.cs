//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using System.Collections.Generic;

namespace Arbor.BehaviourTree
{
#if ARBOR_DOC_JA
	/// <summary>
	/// Nodeとリンクするためのスロットクラス。
	/// </summary>
#else
	/// <summary>
	/// Slot class for linking with Node.
	/// </summary>
#endif
	[System.Serializable]
	public sealed class ChildrenLinkSlot : NodeLinkSlot
	{
#if ARBOR_DOC_JA
		/// <summary>
		/// NodeBranchのbranchID
		/// </summary>
#else
		/// <summary>
		/// BranchID of NodeBranch
		/// </summary>
#endif
		public List<int> branchIDs = new List<int>();

		internal void AddBranch(int branchID)
		{
			if (branchIDs.Contains(branchID))
			{
				return;
			}

			branchIDs.Add(branchID);
			ConnectionChanged();
		}

		internal bool RemoveBranch(int branchID)
		{
			if (branchIDs.Remove(branchID))
			{
				ConnectionChanged();

				return true;
			}

			return false;
		}

		internal void Import(List<NodeLinkSlotLegacy> childrenLink)
		{
			branchIDs.Clear();
			foreach (var slot in childrenLink)
			{
				branchIDs.Add(slot.branchID);
			}
		}
	}
}
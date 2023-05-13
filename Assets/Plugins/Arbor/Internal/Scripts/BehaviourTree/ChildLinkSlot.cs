//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------

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
	public sealed class ChildLinkSlot : NodeLinkSlot
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
		public int branchID;

		internal void SetBranch(int branchID)
		{
			this.branchID = branchID;
			ConnectionChanged();
		}

		internal bool RemoveBranch(int branchID)
		{
			if (branchID != 0 && this.branchID == branchID)
			{
				this.branchID = 0;

				ConnectionChanged();

				return true;
			}

			return false;
		}
	}
}
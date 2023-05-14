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
	public sealed class NodeLinkSlotLegacy
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
	}
}
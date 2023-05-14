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
	public sealed class ParentLinkSlot : NodeLinkSlot
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

#if ARBOR_DOC_JA
		/// <summary>
		/// 親ノードに接続するNodeBranchのIDを設定する。
		/// </summary>
		/// <param name="branchID">親ノードに接続するNodeBranchのID</param>
#else
		/// <summary>
		/// Set the ID of the Node Branch that connects to the parent node.
		/// </summary>
		/// <param name="branchID">ID of the Node Branch that connects to the parent node</param>
#endif
		public void SetBranch(int branchID)
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
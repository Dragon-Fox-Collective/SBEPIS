//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
namespace Arbor
{
#if ARBOR_DOC_JA
	/// <summary>
	/// 出力DataSlotのインターフェイス
	/// </summary>
#else
	/// <summary>
	/// Interface of output DataSlot.
	/// </summary>
#endif
	public interface IOutputSlot : IDataSlot
	{
#if ARBOR_DOC_JA
		/// <summary>
		/// DataBranchの個数を取得。
		/// </summary>
		/// <returns>DataBranchの個数</returns>
#else
		/// <summary>
		/// Get count of DataBranch.
		/// </summary>
		/// <returns>Count of DataBranch</returns>
#endif
		int branchCount
		{
			get;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// DataBranchの追加。
		/// </summary>
		/// <param name="branch">DataBranch</param>
#else
		/// <summary>
		/// Addition of DataBranch.
		/// </summary>
		/// <param name="branch">DataBranch</param>
#endif
		void AddBranch(DataBranch branch);

#if ARBOR_DOC_JA
		/// <summary>
		/// DataBranchの削除。
		/// </summary>
		/// <param name="branch">DataBranch</param>
		/// <returns>削除した場合にtrueを返す。</returns>
#else
		/// <summary>
		/// Remove DataBranch.
		/// </summary>
		/// <param name="branch">DataBranch</param>
		/// <returns>Returns true if removed.</returns>
#endif
		bool RemoveBranch(DataBranch branch);

#if ARBOR_DOC_JA
		/// <summary>
		/// DataBranchの削除。
		/// </summary>
		/// <param name="index">インデックス</param>
		/// <returns>削除した場合にtrueを返す。</returns>
#else
		/// <summary>
		/// Remove DataBranch.
		/// </summary>
		/// <param name="index">Index</param>
		/// <returns>Returns true if removed.</returns>
#endif
		bool RemoveBranchAt(int index);

#if ARBOR_DOC_JA
		/// <summary>
		/// DataBranchの取得。
		/// </summary>
		/// <param name="index">インデックス</param>
		/// <returns>DataBranch</returns>
#else
		/// <summary>
		/// Get DataBranch.
		/// </summary>
		/// <param name="index">Index</param>
		/// <returns>DataBranch</returns>
#endif
		DataBranch GetBranch(int index);

#if ARBOR_DOC_JA
		/// <summary>
		/// DataBranchと接続しているか判定する。
		/// </summary>
		/// <param name="branch">DataBranch</param>
		/// <returns>接続している場合にtrue、それ以外はfalse。</returns>
#else
		/// <summary>
		/// It judges whether it is connected with DataBranch.
		/// </summary>
		/// <param name="branch">DataBranch</param>
		/// <returns>True if connected, false otherwise.</returns>
#endif
		bool IsConnected(DataBranch branch);
	}
}
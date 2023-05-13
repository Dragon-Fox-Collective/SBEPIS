//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
namespace Arbor
{
#if ARBOR_DOC_JA
	/// <summary>
	/// 入力DataSlotのインターフェイス
	/// </summary>
#else
	/// <summary>
	/// Interface of input DataSlot.
	/// </summary>
#endif
	public interface IInputSlot : IDataSlot
	{
#if ARBOR_DOC_JA
		/// <summary>
		/// DataBranchの設定。
		/// </summary>
		/// <param name="branch">DataBranch</param>
#else
		/// <summary>
		/// Set DataBranch.
		/// </summary>
		/// <param name="branch">DataBranch</param>
#endif
		void SetBranch(DataBranch branch);

#if ARBOR_DOC_JA
		/// <summary>
		/// DataBranchの削除。
		/// </summary>
		/// <param name="branch">DataBranch</param>
		/// <returns>削除したらtrueを返す。</returns>
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
		/// DataBranchのリセット。
		/// </summary>
		/// <returns>削除したらtrueを返す。</returns>
#else
		/// <summary>
		/// Reset DataBranch
		/// </summary>
		/// <returns>Returns true if removed.</returns>
#endif
		bool ResetBranch();

#if ARBOR_DOC_JA
		/// <summary>
		/// DataBranchの取得。
		/// </summary>
		/// <returns>DataBranch</returns>
#else
		/// <summary>
		/// Get DataBranch.
		/// </summary>
		/// <returns>DataBranch</returns>
#endif
		DataBranch GetBranch();

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

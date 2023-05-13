//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------

namespace Arbor.BehaviourTree
{
#if ARBOR_DOC_JA
	/// <summary>
	/// BehaviourTreeのノード状態
	/// </summary>
#else
	/// <summary>
	/// Behavior tree node status
	/// </summary>
#endif
	public enum NodeStatus
	{
#if ARBOR_DOC_JA
		/// <summary>
		/// 実行中
		/// </summary>
#else
		/// <summary>
		/// Running
		/// </summary>
#endif
		Running,

#if ARBOR_DOC_JA
		/// <summary>
		/// 成功
		/// </summary>
#else
		/// <summary>
		/// Success
		/// </summary>
#endif
		Success,

#if ARBOR_DOC_JA
		/// <summary>
		/// 失敗
		/// </summary>
#else
		/// <summary>
		/// Failure
		/// </summary>
#endif
		Failure,
	}
}
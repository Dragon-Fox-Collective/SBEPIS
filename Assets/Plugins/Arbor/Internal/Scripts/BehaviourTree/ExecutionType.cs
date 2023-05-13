//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------

namespace Arbor.BehaviourTree
{
#if ARBOR_DOC_JA
	/// <summary>
	/// BehaviourTreeの実行タイプ。
	/// </summary>
#else
	/// <summary>
	/// Behavior tree execution type.
	/// </summary>
#endif
	[Internal.Documentable]
	public enum ExecutionType
	{
#if ARBOR_DOC_JA
		/// <summary>
		/// TreeNodeBase.statusがNodeStatus.Runningになるまで全てのアクションを実行する。
		/// </summary>
#else
		/// <summary>
		/// Execute all actions until TreeNodeBase.status is NodeStatus.Running.
		/// </summary>
#endif
		UntilRunning,

#if ARBOR_DOC_JA
		/// <summary>
		/// 実行するアクションの最大数を指定する。
		/// </summary>
#else
		/// <summary>
		/// Specify the maximum count of actions to execute.
		/// </summary>
#endif
		Count,
	}
}
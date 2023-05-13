//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.BehaviourTree
{
#if ARBOR_DOC_JA
	/// <summary>
	/// BehaviourTreeの実行設定。
	/// </summary>
#else
	/// <summary>
	/// Behavior tree execution settings.
	/// </summary>
#endif
	[System.Serializable]
	[Internal.Documentable]
	public sealed class ExecutionSettings
	{
#if ARBOR_DOC_JA
		/// <summary>
		/// 実行タイプ
		/// </summary>
#else
		/// <summary>
		/// Execution type
		/// </summary>
#endif
		public ExecutionType type = ExecutionType.UntilRunning;

#if ARBOR_DOC_JA
		/// <summary>
		/// 実行するアクションの最大数。
		/// </summary>
#else
		/// <summary>
		/// Maximum count of the action to execute.
		/// </summary>
#endif
		[Range(1, 1000)]
		public int maxCount = 1;
	}
}
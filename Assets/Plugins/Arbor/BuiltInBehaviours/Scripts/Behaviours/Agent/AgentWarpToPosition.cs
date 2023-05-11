//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.StateMachine.StateBehaviours
{
#if ARBOR_DOC_JA
	/// <summary>
	/// AgentをTargetの位置にワープする。
	/// </summary>
#else
	/// <summary>
	/// Warp the Agent to the Target position.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Agent/AgentWarpToPosition")]
	[BuiltInBehaviour]
	public sealed class AgentWarpToPosition : AgentBase
	{
		#region Serialize fields

#if ARBOR_DOC_JA
		/// <summary>
		/// 対象の位置
		/// </summary>
#else
		/// <summary>
		/// Position of target
		/// </summary>
#endif
		[SerializeField] private FlexibleVector3 _Target = new FlexibleVector3();

#if ARBOR_DOC_JA
		/// <summary>
		/// ワープ成功した時のステート遷移<br />
		/// 遷移メソッド : OnStateBegin
		/// </summary>
#else
		/// <summary>
		/// State transition when warp succeeds<br />
		/// Transition Method : OnStateBegin
		/// </summary>
#endif
		[SerializeField] private StateLink _Success = new StateLink();

#if ARBOR_DOC_JA
		/// <summary>
		/// ワープ失敗した時のステート遷移<br />
		/// 遷移メソッド : OnStateBegin
		/// </summary>
#else
		/// <summary>
		/// State transitions when warp fails<br />
		/// Transition Method : OnStateBegin
		/// </summary>
#endif
		[SerializeField] private StateLink _Failure = new StateLink();

		#endregion // Serialize fields

		// Use this for enter state
		public override void OnStateBegin()
		{
			AgentController agentController = cachedAgentController;
			if (agentController != null)
			{
				if (agentController.Warp(_Target.value))
				{
					Transition(_Success);
				}
				else
				{
					Transition(_Failure);
				}
			}
		}
	}
}
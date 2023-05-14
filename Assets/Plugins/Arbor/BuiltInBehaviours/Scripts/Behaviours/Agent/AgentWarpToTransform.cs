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
	[AddBehaviourMenu("Agent/AgentWarpToTransform")]
	[BuiltInBehaviour]
	public sealed class AgentWarpToTransform : AgentBase
	{
		#region Serialize fields

#if ARBOR_DOC_JA
		/// <summary>
		/// 対象のTransform
		/// </summary>
#else
		/// <summary>
		/// Transform of target
		/// </summary>
#endif
		[SerializeField] private FlexibleTransform _Target = new FlexibleTransform();

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
				Transform target = _Target.value;
				if (target != null && agentController.Warp(target.position))
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
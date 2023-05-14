//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.BehaviourTree.Actions
{
#if ARBOR_DOC_JA
	/// <summary>
	/// Agentを停止させる
	/// </summary>
#else
	/// <summary>
	/// Stop the Agent
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Agent/AgentStop")]
	[BuiltInBehaviour]
	public sealed class AgentStop : AgentBase
	{
#if ARBOR_DOC_JA
		/// <summary>
		/// 速度をクリアするかどうか
		/// </summary>
#else
		/// <summary>
		/// Whether to clear velocity
		/// </summary>
#endif
		[SerializeField]
		private FlexibleBool _ClearVelocity = new FlexibleBool(false);

		protected override void OnExecute()
		{
			AgentController agentController = cachedAgentController;
			if (agentController != null)
			{
				agentController.Stop(_ClearVelocity.value);
			}
			FinishExecute(true);
		}
	}
}
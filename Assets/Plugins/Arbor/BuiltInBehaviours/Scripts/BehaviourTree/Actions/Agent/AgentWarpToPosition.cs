//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.BehaviourTree.Actions
{
#if ARBOR_DOC_JA
	/// <summary>
	/// AgentをTargetPositionにワープする。
	/// </summary>
	/// <remarks>成功した場合はTrueを返し、そうでなければFalseを返す。</remarks>
#else
	/// <summary>
	/// Warp the Agent to the Target position.
	/// </summary>
	/// <remarks>Returns true if successful, otherwise returns false.</remarks>
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
		/// Target position
		/// </summary>
#endif
		[SerializeField]
		private FlexibleVector3 _TargetPosition = new FlexibleVector3();

		#endregion // Serialize fields

		protected override void OnExecute()
		{
			bool result = false;
			AgentController agentController = cachedAgentController;
			if (agentController != null)
			{
				result = agentController.Warp(_TargetPosition.value);
			}
			FinishExecute(result);
		}
	}
}
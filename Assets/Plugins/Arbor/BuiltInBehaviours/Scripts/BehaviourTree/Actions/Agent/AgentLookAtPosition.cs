//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.BehaviourTree.Actions
{
#if ARBOR_DOC_JA
	/// <summary>
	/// AgentをTargetの方向へ回転させる。
	/// </summary>
#else
	/// <summary>
	/// Rotate the Agent in the direction of Target.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Agent/AgentLookAtPosition")]
	[BuiltInBehaviour]
	public sealed class AgentLookAtPosition : AgentRotateBase
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

		protected override bool OnIntervalUpdate(AgentController agentController)
		{
			agentController.LookAt(_AngularSpeed.value, _TargetPosition.value);
			return true;
		}
	}
}
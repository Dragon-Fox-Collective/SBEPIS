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
	[AddBehaviourMenu("Agent/AgentLookAtTransform")]
	[BuiltInBehaviour]
	public sealed class AgentLookAtTransform : AgentRotateBase
	{
		#region Serialize fields

#if ARBOR_DOC_JA
		/// <summary>
		/// 対象のTransform
		/// </summary>
#else
		/// <summary>
		/// Target Transform
		/// </summary>
#endif
		[SerializeField]
		private FlexibleTransform _TargetTransform = new FlexibleTransform();

		#endregion // Serialize fields

		protected override bool OnIntervalUpdate(AgentController agentController)
		{
			agentController.LookAt(_AngularSpeed.value, _TargetTransform.value);
			return true;
		}
	}
}
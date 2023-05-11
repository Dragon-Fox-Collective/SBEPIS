//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
namespace Arbor.StateMachine.StateBehaviours
{
#if ARBOR_DOC_JA
	/// <summary>
	/// 指定位置の方向へ回転する。
	/// </summary>
#else
	/// <summary>
	/// Rotates in the direction of the specified position.
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
		/// Position of target
		/// </summary>
#endif
		[SerializeField] private FlexibleVector3 _Target = new FlexibleVector3();

		#endregion

		protected override bool OnIntervalUpdate(AgentController agentController)
		{
			agentController.LookAt(_AngularSpeed.value, _Target.value);
			return true;
		}
	}
}

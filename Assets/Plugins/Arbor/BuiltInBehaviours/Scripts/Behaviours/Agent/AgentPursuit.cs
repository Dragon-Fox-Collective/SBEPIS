//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using UnityEngine.Serialization;

namespace Arbor.StateMachine.StateBehaviours
{
#if ARBOR_DOC_JA
	/// <summary>
	/// AgentをTargetの移動速度を考慮して追跡するように移動させる。
	/// </summary>
#else
	/// <summary>
	/// Move the Agent to track the Target's movement speed.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Agent/AgentPursuit")]
	[BuiltInBehaviour]
	public sealed class AgentPursuit : AgentMoveBase
	{
		#region Serialize fields

#if ARBOR_DOC_JA
		/// <summary>
		/// 停止する距離
		/// </summary>
#else
		/// <summary>
		/// Distance to stop
		/// </summary>
#endif
		[SerializeField]
		private FlexibleFloat _StoppingDistance = new FlexibleFloat(0f);

#if ARBOR_DOC_JA
		/// <summary>
		/// 対面しているかを判定する角度<br/>
		/// TargetがAgent側を向いて対面している時はTargetに向かって直接移動する
		/// </summary>
#else
		/// <summary>
		/// Angle to judge whether they are facing each other <br/>
		/// When Target faces the Agent side and faces, move directly toward Target
		/// </summary>
#endif
		[SerializeField]
		[ConstantRange(0f, 180f)]
		private FlexibleFloat _FacingAngle = new FlexibleFloat(20f);

#if ARBOR_DOC_JA
		/// <summary>
		/// Targetのローカル座標系でのオフセット
		/// </summary>
#else
		/// <summary>
		/// Target's offset in the local coordinate system
		/// </summary>
#endif
		[SerializeField]
		private FlexibleVector3 _Offset = new FlexibleVector3();

#if ARBOR_DOC_JA
		/// <summary>
		/// 対象となるMovingEntity
		/// </summary>
#else
		/// <summary>
		/// Target MovingEntity
		/// </summary>
#endif
		[SerializeField]
		[SlotType(typeof(MovingEntity))]
		private FlexibleComponent _Target = new FlexibleComponent();

		#endregion

		protected override bool OnIntervalUpdate(AgentController agentController)
		{
			return agentController.Pursuit(_Speed.value, _StoppingDistance.value, _FacingAngle.value, _Target.value as MovingEntity, _Offset.value);
		}
	}
}

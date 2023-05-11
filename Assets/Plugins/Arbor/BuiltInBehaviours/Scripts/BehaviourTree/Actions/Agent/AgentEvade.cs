//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.BehaviourTree.Actions
{
#if ARBOR_DOC_JA
	/// <summary>
	/// AgentをTargetの移動速度を考慮して逃げるように移動させる。
	/// </summary>
#else
	/// <summary>
	/// Move the Agent to escape considering the movement velocity of the Target.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Agent/AgentEvade")]
	[BuiltInBehaviour]
	public sealed class AgentEvade : AgentMoveBase
	{
		#region Serialize fields

#if ARBOR_DOC_JA
		/// <summary>
		/// 離れる距離
		/// </summary>
#else
		/// <summary>
		/// Distance away
		/// </summary>
#endif
		[SerializeField] private FlexibleFloat _Distance = new FlexibleFloat();

#if ARBOR_DOC_JA
		/// <summary>
		/// 曲がり角までの距離。<br/>
		/// 隅に追いやられた場合に折り返す判断に使用される。
		/// </summary>
#else
		/// <summary>
		/// Distance to the corner.<br/>
		/// It is used to make a decision to turn back when driven to a corner.
		/// </summary>
#endif
		[SerializeField]
		private FlexibleFloat _DistanceToCorner = new FlexibleFloat(0f);

#if ARBOR_DOC_JA
		/// <summary>
		/// 対象のMovingEntity
		/// </summary>
#else
		/// <summary>
		/// Target MovingEntity
		/// </summary>
#endif
		[SerializeField]
		[SlotType(typeof(MovingEntity))]
		private FlexibleComponent _Target = new FlexibleComponent();

		#endregion // Serialize fields

		protected override bool OnIntervalUpdate(AgentController agentController)
		{
			return agentController.Evade(_Speed.value, _Distance.value, _Target.value as MovingEntity, _DistanceToCorner.value);
		}
	}
}
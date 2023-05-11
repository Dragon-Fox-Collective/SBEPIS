//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.BehaviourTree.Actions
{
#if ARBOR_DOC_JA
	/// <summary>
	/// Agentを２つのTargetの間に向かって近づくように移動させる。
	/// </summary>
#else
	/// <summary>
	/// Move the Agent closer to between the two Targets.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Agent/AgentInterpose")]
	[BuiltInBehaviour]
	public sealed class AgentInterpose : AgentMoveBase
	{
		#region Serialize fields

#if ARBOR_DOC_JA
		/// <summary>
		/// 近づきたい対象の一つ目のMovingEntity
		/// </summary>
#else
		/// <summary>
		/// The first Moving Entity of the object you want to approach
		/// </summary>
#endif
		[SerializeField]
		[SlotType(typeof(MovingEntity))]
		private FlexibleComponent _TargetA = new FlexibleComponent();

#if ARBOR_DOC_JA
		/// <summary>
		/// 近づきたい対象の二つ目のMovingEntity
		/// </summary>
#else
		/// <summary>
		/// The second Moving Entity of the object you want to approach
		/// </summary>
#endif
		[SerializeField]
		[SlotType(typeof(MovingEntity))]
		private FlexibleComponent _TargetB = new FlexibleComponent();

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
		private FlexibleFloat _StoppingDistance = new FlexibleFloat();

		#endregion // Serialize fields

		protected override bool OnIntervalUpdate(AgentController agentController)
		{
			return agentController.Interpose(_Speed.value, _TargetA.value as MovingEntity, _TargetB.value as MovingEntity, _StoppingDistance.value);
		}
	}
}
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

		[SerializeField]
		[HideInInspector]
		private int _AgentFollow_SerializeVersion = 0;

		#region old

		[SerializeField]
		[FormerlySerializedAs("_StoppingDistance")]
		[HideInInspector]
		private float _OldStoppingDistance = 0f;

		#endregion // old

		#endregion

		private const int kCurrentSerializeVersion = 1;

		protected override bool OnIntervalUpdate(AgentController agentController)
		{
			var targetA = _TargetA.value as MovingEntity;
			var targetB = _TargetB.value as MovingEntity;
			return agentController.Interpose(_Speed.value, targetA, targetB, _StoppingDistance.value);
		}

		void SerializeVer1()
		{
			_StoppingDistance = (FlexibleFloat)_OldStoppingDistance;
		}

		void Serialize()
		{
			while (_AgentFollow_SerializeVersion != kCurrentSerializeVersion)
			{
				switch (_AgentFollow_SerializeVersion)
				{
					case 0:
						SerializeVer1();
						_AgentFollow_SerializeVersion++;
						break;
					default:
						_AgentFollow_SerializeVersion = kCurrentSerializeVersion;
						break;
				}
			}
		}

		protected override void Reset()
		{
			base.Reset();

			_AgentFollow_SerializeVersion = kCurrentSerializeVersion;
		}

		public override void OnBeforeSerialize()
		{
			base.OnBeforeSerialize();

			Serialize();
		}

		public override void OnAfterDeserialize()
		{
			base.OnAfterDeserialize();

			Serialize();
		}
	}
}

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
	/// 対象のGameObjectとの距離によってステートを遷移する。
	/// </summary>
#else
	/// <summary>
	/// It will transition the state by the distance between the target of GameObject.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Transition/DistanceTransition")]
	[BuiltInBehaviour]
	public sealed class DistanceTransition : StateBehaviour, INodeBehaviourSerializationCallbackReceiver
	{
		#region Serialize fields

#if ARBOR_DOC_JA
		/// <summary>
		/// 比較元のTransform。
		/// </summary>
#else
		/// <summary>
		/// Transform to compare from.
		/// </summary>
#endif
		[SerializeField] private FlexibleTransform _Transform = new FlexibleTransform(FlexibleHierarchyType.Self);

#if ARBOR_DOC_JA
		/// <summary>
		/// 比較対象のTransform。
		/// </summary>
#else
		/// <summary>
		/// Transform to be compared.
		/// </summary>
#endif
		[SerializeField] private FlexibleTransform _Target = new FlexibleTransform();

#if ARBOR_DOC_JA
		/// <summary>
		/// Targetとの距離。
		/// </summary>
#else
		/// <summary>
		/// The distance between the Target.
		/// </summary>
#endif
		[SerializeField]
		private FlexibleFloat _Distance = new FlexibleFloat(0f);

#if ARBOR_DOC_JA
		/// <summary>
		/// Distanceよりも近い場合の遷移先ステート。<br />
		/// 遷移メソッド : OnStateUpdate
		/// </summary>
#else
		/// <summary>
		/// Transition destination state if it is closer than the Distance.<br />
		/// Transition Method : OnStateUpdate
		/// </summary>
#endif
		[SerializeField] private StateLink _NearState = new StateLink();

#if ARBOR_DOC_JA
		/// <summary>
		/// Distanceよりも遠い場合の遷移先ステート。<br />
		/// 遷移メソッド : OnStateUpdate
		/// </summary>
#else
		/// <summary>
		/// Transition destination state if it is farther away than the Distance.<br />
		/// Transition Method : OnStateUpdate
		/// </summary>
#endif
		[SerializeField] private StateLink _FarState = new StateLink();

		[SerializeField]
		[HideInInspector]
		private int _SerializeVersion = 0;

		#region old

		[SerializeField]
		[FormerlySerializedAs("_Distance")]
		[HideInInspector]
		private float _OldDistance = 0f;

		#endregion // old

		#endregion // Serialize fields

		private const int kCurrentSerializeVersion = 1;

		public override void OnStateUpdate()
		{
			Transform target = _Target.value;
			if (target == null)
			{
				return;
			}

			Transform transform = _Transform.value;
			if (transform == null)
			{
				return;
			}

			float distanceValue = _Distance.value;

			float sqrtTargetDistance = (transform.position - target.position).sqrMagnitude;

			if (sqrtTargetDistance <= distanceValue * distanceValue)
			{
				Transition(_NearState);
			}
			else
			{
				Transition(_FarState);
			}
		}

		void Reset()
		{
			_SerializeVersion = kCurrentSerializeVersion;
		}

		void SerializeVer1()
		{
			_Distance = (FlexibleFloat)_OldDistance;
		}

		void Serialize()
		{
			while (_SerializeVersion != kCurrentSerializeVersion)
			{
				switch (_SerializeVersion)
				{
					case 0:
						SerializeVer1();
						_SerializeVersion++;
						break;
					default:
						_SerializeVersion = kCurrentSerializeVersion;
						break;
				}
			}
		}

		void INodeBehaviourSerializationCallbackReceiver.OnBeforeSerialize()
		{
			Serialize();
		}

		void INodeBehaviourSerializationCallbackReceiver.OnAfterDeserialize()
		{
			Serialize();
		}
	}
}

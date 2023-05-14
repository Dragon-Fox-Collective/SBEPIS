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
	/// 開始ステートへ戻る。
	/// </summary>
#else
	/// <summary>
	/// Back to the start state.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Transition/BackToStartState")]
	[BuiltInBehaviour]
	public sealed class BackToStartState : StateBehaviour, INodeBehaviourSerializationCallbackReceiver
	{
		#region Serialize fields

#if ARBOR_DOC_JA
		/// <summary>
		/// 遷移するタイミング。
		/// </summary>
#else
		/// <summary>
		/// Transition timing.
		/// </summary>
#endif
		[SerializeField]
		[Internal.DocumentType(typeof(TransitionTiming))]
		private FlexibleTransitionTiming _TransitionTiming = new FlexibleTransitionTiming(TransitionTiming.LateUpdateOverwrite);

		[SerializeField]
		[HideInInspector]
		private int _SerializeVersion = 0;

		#region old

		[SerializeField]
		[HideInInspector]
		[FormerlySerializedAs("_TransitionTiming")]
		private TransitionTiming _OldTransitionTiming = TransitionTiming.LateUpdateOverwrite;

		#endregion // old

		#endregion // Serialize fields

		private const int kCurrentSerializeVersion = 1;

		// Use this for enter state
		public override void OnStateBegin()
		{
			stateMachine.Transition(stateMachine.startStateID, _TransitionTiming.value);
		}

		void Reset()
		{
			_SerializeVersion = kCurrentSerializeVersion;
		}

		void SerializeVer1()
		{
			_TransitionTiming = (FlexibleTransitionTiming)_OldTransitionTiming;
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

		void INodeBehaviourSerializationCallbackReceiver.OnAfterDeserialize()
		{
			Serialize();
		}

		void INodeBehaviourSerializationCallbackReceiver.OnBeforeSerialize()
		{
			Serialize();
		}
	}
}
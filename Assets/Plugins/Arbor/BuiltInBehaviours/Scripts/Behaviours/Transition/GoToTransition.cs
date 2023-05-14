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
	/// 強制的にステートを遷移する。
	/// </summary>
#else
	/// <summary>
	/// It will transition to force the state.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Transition/GoToTransition")]
	[BuiltInBehaviour]
	public sealed class GoToTransition : StateBehaviour, INodeBehaviourSerializationCallbackReceiver
	{
#if ARBOR_DOC_JA
		/// <summary>
		/// 遷移呼び出しするメソッド
		/// </summary>
#else
		/// <summary>
		/// Method to call transition
		/// </summary>
#endif
		[Internal.Documentable]
		public enum TransitionMethod
		{
#if ARBOR_DOC_JA
			/// <summary>
			/// OnStateBeginメソッドから遷移する。
			/// </summary>
#else
			/// <summary>
			/// Transition from OnStateBegin method.
			/// </summary>
#endif
			OnStateBegin,

#if ARBOR_DOC_JA
			/// <summary>
			/// OnStateUpdateメソッドから遷移する。
			/// </summary>
#else
			/// <summary>
			/// Transition from OnStateUpdate method.
			/// </summary>
#endif
			OnStateUpdate,

#if ARBOR_DOC_JA
			/// <summary>
			/// OnStateLateUpdateメソッドから遷移する。
			/// </summary>
#else
			/// <summary>
			/// Transition from OnStateLateUpdate method.
			/// </summary>
#endif
			OnStateLateUpdate,
		}

		[System.Serializable]
		public sealed class FlexibleTransitionMethod : FlexibleField<TransitionMethod>
		{
			public FlexibleTransitionMethod()
			{
			}

			public FlexibleTransitionMethod(TransitionMethod value) : base(value)
			{
			}

			public FlexibleTransitionMethod(AnyParameterReference parameter) : base(parameter)
			{
			}

			public FlexibleTransitionMethod(InputSlotAny slot) : base(slot)
			{
			}

			public static explicit operator TransitionMethod(FlexibleTransitionMethod flexible)
			{
				return flexible.value;
			}

			public static explicit operator FlexibleTransitionMethod(TransitionMethod value)
			{
				return new FlexibleTransitionMethod(value);
			}
		}

		#region Serialize fields

#if ARBOR_DOC_JA
		/// <summary>
		/// 遷移呼び出しするメソッド
		/// </summary>
#else
		/// <summary>
		/// Method to call transition
		/// </summary>
#endif
		[SerializeField]
		[Internal.DocumentType(typeof(TransitionMethod))]
		private FlexibleTransitionMethod _TransitionMethod = new FlexibleTransitionMethod(TransitionMethod.OnStateBegin);

#if ARBOR_DOC_JA
		/// <summary>
		/// 遷移先ステート。<br />
		/// 遷移メソッド : Transition Methodフィールドにより指定。
		/// </summary>
#else
		/// <summary>
		/// Transition destination state.<br />
		/// Transition Method : Set by Transition Method field.
		/// </summary>
#endif
		[SerializeField] private StateLink _NextState = new StateLink();

		[SerializeField]
		[HideInInspector]
		private int _SerializeVersion = 0;

		#region old

		[SerializeField]
		[HideInInspector]
		[FormerlySerializedAs("_TransitionMethod")]
		private TransitionMethod _OldTransitionMethod = TransitionMethod.OnStateBegin;

		#endregion // old

		#endregion // Serialize fields

		private const int kCurrentSerializeVersion = 1;

		void Transition()
		{
			Transition(_NextState);
		}

		// Use this for enter state
		public override void OnStateBegin()
		{
			if (_TransitionMethod.value == TransitionMethod.OnStateBegin)
			{
				Transition();
			}
		}

		public override void OnStateUpdate()
		{
			if (_TransitionMethod.value == TransitionMethod.OnStateUpdate)
			{
				Transition();
			}
		}

		public override void OnStateLateUpdate()
		{
			if (_TransitionMethod.value == TransitionMethod.OnStateLateUpdate)
			{
				Transition();
			}
		}

		void Reset()
		{
			_SerializeVersion = kCurrentSerializeVersion;
		}

		void SerializeVer1()
		{
			_TransitionMethod = (FlexibleTransitionMethod)_OldTransitionMethod;
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

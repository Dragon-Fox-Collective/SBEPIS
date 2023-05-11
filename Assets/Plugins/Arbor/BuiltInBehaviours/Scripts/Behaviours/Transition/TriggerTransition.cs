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
	/// ステートトリガーが送られてきたときにステートを遷移します。
	/// </summary>
#else
	/// <summary>
	/// It will transition the state when the trigger has been sent.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Transition/TriggerTransition")]
	[BuiltInBehaviour]
	public sealed class TriggerTransition : StateBehaviour, INodeBehaviourSerializationCallbackReceiver
	{
		#region Serialize fields

#if ARBOR_DOC_JA
		/// <summary>
		/// 遷移するか判定するメッセージ。
		/// </summary>
#else
		/// <summary>
		/// Message to decide whether to transition.
		/// </summary>
#endif
		[SerializeField] private FlexibleString _Message = new FlexibleString(string.Empty);

#if ARBOR_DOC_JA
		/// <summary>
		/// 遷移先ステート。<br />
		/// 遷移メソッド : OnStateTrigger
		/// </summary>
#else
		/// <summary>
		/// Transition destination state.<br />
		/// Transition Method : OnStateTrigger
		/// </summary>
#endif
		[SerializeField] private StateLink _NextState = new StateLink();

		[SerializeField]
		[HideInInspector]
		private int _SerializeVersion = 0;

		#region old

		[FormerlySerializedAs("_Message")]
		[SerializeField]
		[HideInInspector]
		private string _OldMessage = string.Empty;

		#endregion // old

		#endregion // Serialize fields

		private const int kCurrentSerializeVersion = 1;

		private string _CurrentMessage;

		public override void OnStateBegin()
		{
			base.OnStateBegin();

			_CurrentMessage = _Message.value;
		}

		public override void OnStateTrigger(string message)
		{
			if (!enabled)
			{
				return;
			}

			if (_CurrentMessage == message)
			{
				Transition(_NextState);
			}
		}

		void Reset()
		{
			_SerializeVersion = kCurrentSerializeVersion;
		}

		void SerializeVer1()
		{
			_Message = (FlexibleString)_OldMessage;
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

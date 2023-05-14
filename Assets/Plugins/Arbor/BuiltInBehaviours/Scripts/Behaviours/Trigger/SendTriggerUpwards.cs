//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using UnityEngine.Serialization;

namespace Arbor.StateMachine.StateBehaviours
{
	using Arbor.Extensions;

#if ARBOR_DOC_JA
	/// <summary>
	/// GameObjectとその親オブジェクトにトリガーを送ります。
	/// </summary>
#else
	/// <summary>
	/// Send the trigger to all of ArborFSM that is assigned to the GameObject.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Trigger/SendTriggerUpwards")]
	[BuiltInBehaviour]
	public sealed class SendTriggerUpwards : StateBehaviour, INodeBehaviourSerializationCallbackReceiver
	{
		#region Serialize fields

#if ARBOR_DOC_JA
		/// <summary>
		/// 対象のGameObject
		/// </summary>
#else
		/// <summary>
		/// GameObject target
		/// </summary>
#endif
		[SerializeField]
		private FlexibleGameObject _Target = new FlexibleGameObject(FlexibleHierarchyType.Self);

#if ARBOR_DOC_JA
		/// <summary>
		/// 送るトリガー
		/// </summary>
#else
		/// <summary>
		/// Trigger to send
		/// </summary>
#endif
		[SerializeField]
		private FlexibleString _Message = new FlexibleString(string.Empty);

#if ARBOR_DOC_JA
		/// <summary>
		/// 送るトリガーフラグ
		/// </summary>
#else
		/// <summary>
		/// Trigger flag to send
		/// </summary>
#endif
		[SerializeField]
		[Internal.DocumentType(typeof(SendTriggerFlags))]
		private FlexibleSendTriggerFlags _SendFlags = new FlexibleSendTriggerFlags(ArborFSMInternal.allSendTrigger);

		[SerializeField]
		[HideInInspector]
		private int _SerializeVersion = 0;

		#region old

		[FormerlySerializedAs("_Target")]
		[SerializeField]
		[HideInInspector]
		private GameObject _OldTarget = null;

		[FormerlySerializedAs("_Message")]
		[SerializeField]
		[HideInInspector]
		private string _OldMessage = string.Empty;

		[SerializeField]
		[HideInInspector]
		[FormerlySerializedAs("_SendFlags")]
		private SendTriggerFlags _OldSendFlags = ArborFSMInternal.allSendTrigger;

		#endregion // old

		#endregion // Serialize fields

		private const int kCurrentSerializeVersion = 3;

		public GameObject target
		{
			get
			{
				return _Target.value;
			}
		}

		public override void OnStateBegin()
		{
			string message = _Message.value;

			GameObject current = target;
			while (current != null && current.activeInHierarchy)
			{
				var arborFSMs = current.GetComponentsTemp<ArborFSM>();
				for (int fsmIndex = 0; fsmIndex < arborFSMs.Count; fsmIndex++)
				{
					ArborFSM fsm = arborFSMs[fsmIndex];
					if (fsm.parentGraph == null)
					{
						fsm.SendTrigger(message, _SendFlags.value);
					}
				}

				Transform parent = current.transform.parent;
				if (parent != null)
				{
					current = parent.gameObject;
				}
				else
				{
					current = null;
				}
			}
		}

		void Reset()
		{
			_SerializeVersion = kCurrentSerializeVersion;
		}

		void SerializeVer1()
		{
			_Target = (FlexibleGameObject)_OldTarget;
		}

		void SerializeVer2()
		{
			_Message = (FlexibleString)_OldMessage;
		}

		void SerializeVer3()
		{
			_SendFlags = (FlexibleSendTriggerFlags)_OldSendFlags;
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
					case 1:
						SerializeVer2();
						_SerializeVersion++;
						break;
					case 2:
						SerializeVer3();
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

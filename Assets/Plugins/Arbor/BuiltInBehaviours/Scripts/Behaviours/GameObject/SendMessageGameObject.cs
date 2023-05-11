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
	/// GameObjectにメッセージを送信する。
	/// </summary>
#else
	/// <summary>
	/// It will send a message to the GameObject.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("GameObject/SendMessageGameObject")]
	[BuiltInBehaviour]
	public sealed class SendMessageGameObject : StateBehaviour, INodeBehaviourSerializationCallbackReceiver
	{
		#region enum

		public enum Type
		{
			None,
			Int,
			Float,
			Bool,
			String,
		}

		#endregion // enum

		#region Serialize fields

#if ARBOR_DOC_JA
		/// <summary>
		/// メッセージの送り先GameObject。
		/// </summary>
#else
		/// <summary>
		/// Destination GameObject of message.
		/// </summary>
#endif
		[SerializeField]
		private FlexibleGameObject _Target = new FlexibleGameObject(FlexibleHierarchyType.Self);

#if ARBOR_DOC_JA
		/// <summary>
		/// メッセージのメソッド名。
		/// </summary>
#else
		/// <summary>
		/// Method name of the message.
		/// </summary>
#endif
		[SerializeField]
		private FlexibleString _MethodName = new FlexibleString();

#if ARBOR_DOC_JA
		/// <summary>
		/// メッセージの引数。
		/// </summary>
#else
		/// <summary>
		/// Argument of message.
		/// </summary>
#endif
		[SerializeField]
		private SendMessageArgument _Argument = new SendMessageArgument();

		[SerializeField]
		[HideInInspector]
		private int _SerializeVersion = 0;

		#region old

		// Ver 1
		[FormerlySerializedAs("_MethodName")]
		[SerializeField]
		[HideInInspector]
		private string _OldMethodName = "";

		[SerializeField]
		[HideInInspector]
		private Type _Type = Type.None;

		[SerializeField]
		[HideInInspector]
		private FlexibleInt _IntValue = new FlexibleInt();

		[SerializeField]
		[HideInInspector]
		private FlexibleFloat _FloatValue = new FlexibleFloat();

		[SerializeField]
		[HideInInspector]
		private FlexibleBool _BoolValue = new FlexibleBool();

		[SerializeField]
		[HideInInspector]
		private string _StringValue = string.Empty;

		// Ver 0
		[FormerlySerializedAs("_Target")]
		[SerializeField]
		[HideInInspector]
		private GameObject _OldTarget = null;

		[FormerlySerializedAs("_IntValue")]
		[SerializeField]
		[HideInInspector]
		private int _OldIntValue = 0;

		[FormerlySerializedAs("_FloatValue")]
		[SerializeField]
		[HideInInspector]
		private float _OldFloatValue = 0f;

		[FormerlySerializedAs("_BoolValue")]
		[SerializeField]
		[HideInInspector]
		private bool _OldBoolValue = false;

		#endregion // old

		#endregion // Serialize fields

		private const int kCurrentSerializeVersion = 2;

		public override void OnStateBegin()
		{
			_Argument.SendMessage(_Target.value, _MethodName.value, SendMessageOptions.DontRequireReceiver);
		}

		void Reset()
		{
			_SerializeVersion = kCurrentSerializeVersion;
		}

		void SerializeVer1()
		{
			_Target = (FlexibleGameObject)_OldTarget;
			_FloatValue = (FlexibleFloat)_OldFloatValue;
			_IntValue = (FlexibleInt)_OldIntValue;
			_BoolValue = (FlexibleBool)_OldBoolValue;
		}

		void SerializeVer2()
		{
			_MethodName = (FlexibleString)_OldMethodName;

			switch (_Type)
			{
				case Type.None:
					_Argument._Type = SendMessageArgument.Type.None;
					break;
				case Type.Int:
					_Argument._Type = SendMessageArgument.Type.Int;
					_Argument._IntValue = _IntValue;
					break;
				case Type.Float:
					_Argument._Type = SendMessageArgument.Type.Float;
					_Argument._FloatValue = _FloatValue;
					break;
				case Type.Bool:
					_Argument._Type = SendMessageArgument.Type.Bool;
					_Argument._BoolValue = _BoolValue;
					break;
				case Type.String:
					_Argument._Type = SendMessageArgument.Type.String;
					_Argument._StringValue = new FlexibleString(_StringValue);
					break;
			}
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

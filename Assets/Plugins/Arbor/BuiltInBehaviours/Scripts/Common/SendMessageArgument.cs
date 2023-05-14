//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using UnityEngine.Serialization;

namespace Arbor
{
#if ARBOR_DOC_JA
	/// <summary>
	/// SendMessageの引数。
	/// </summary>
#else
	/// <summary>
	/// Argument of SendMessage.
	/// </summary>
#endif
	[System.Serializable]
	[Internal.Documentable]
	public sealed class SendMessageArgument : ISerializationCallbackReceiver
	{
		#region enum

		public enum Type
		{
			None,
			Int,
			Float,
			Bool,
			String,
			Long,
			Enum,
			GameObject,
			Vector2,
			Vector3,
			Quaternion,
			Rect,
			Bounds,
			Color,
			Component,

			Slot = 0x1000,
		}

		#endregion // enum

		#region Serialize fields

#if ARBOR_DOC_JA
		/// <summary>
		/// 送信する値の型。
		/// </summary>
#else
		/// <summary>
		/// The type of the value to be transmitted.
		/// </summary>
#endif
		[SerializeField]
		internal Type _Type = Type.None;

#if ARBOR_DOC_JA
		/// <summary>
		/// Int型の値
		/// </summary>
#else
		/// <summary>
		/// Int type value
		/// </summary>
#endif
		[SerializeField]
		internal FlexibleInt _IntValue = new FlexibleInt();

#if ARBOR_DOC_JA
		/// <summary>
		/// Float型の値
		/// </summary>
#else
		/// <summary>
		/// Float type value
		/// </summary>
#endif
		[SerializeField]
		internal FlexibleFloat _FloatValue = new FlexibleFloat();

#if ARBOR_DOC_JA
		/// <summary>
		/// Bool型の値
		/// </summary>
#else
		/// <summary>
		/// Bool type value
		/// </summary>
#endif
		[SerializeField]
		internal FlexibleBool _BoolValue = new FlexibleBool();

#if ARBOR_DOC_JA
		/// <summary>
		/// String型の値
		/// </summary>
#else
		/// <summary>
		/// String type value
		/// </summary>
#endif
		[SerializeField]
		internal FlexibleString _StringValue = new FlexibleString();

#if ARBOR_DOC_JA
		/// <summary>
		/// Long型の値
		/// </summary>
#else
		/// <summary>
		/// Long type value
		/// </summary>
#endif
		[SerializeField]
		private FlexibleLong _LongValue = new FlexibleLong();

#if ARBOR_DOC_JA
		/// <summary>
		/// Enum型の値
		/// </summary>
#else
		/// <summary>
		/// Enum type value
		/// </summary>
#endif
		[SerializeField]
		private FlexibleEnumAny _EnumValue = new FlexibleEnumAny();

#if ARBOR_DOC_JA
		/// <summary>
		/// GameObject型の値
		/// </summary>
#else
		/// <summary>
		/// GameObject type value
		/// </summary>
#endif
		[SerializeField]
		private FlexibleGameObject _GameObjectValue = new FlexibleGameObject();

#if ARBOR_DOC_JA
		/// <summary>
		/// Vector2型の値
		/// </summary>
#else
		/// <summary>
		/// Vector2 type value
		/// </summary>
#endif
		[SerializeField]
		private FlexibleVector2 _Vector2Value = new FlexibleVector2();

#if ARBOR_DOC_JA
		/// <summary>
		/// Vector3型の値
		/// </summary>
#else
		/// <summary>
		/// Vector3 type value
		/// </summary>
#endif
		[SerializeField]
		private FlexibleVector3 _Vector3Value = new FlexibleVector3();

#if ARBOR_DOC_JA
		/// <summary>
		/// Quaternion型の値
		/// </summary>
#else
		/// <summary>
		/// Quaternion type value
		/// </summary>
#endif
		[SerializeField]
		private FlexibleQuaternion _QuaternionValue = new FlexibleQuaternion();

#if ARBOR_DOC_JA
		/// <summary>
		/// Rect型の値
		/// </summary>
#else
		/// <summary>
		/// Rect type value
		/// </summary>
#endif
		[SerializeField]
		private FlexibleRect _RectValue = new FlexibleRect();

#if ARBOR_DOC_JA
		/// <summary>
		/// Bounds型の値
		/// </summary>
#else
		/// <summary>
		/// Bounds type value
		/// </summary>
#endif
		[SerializeField]
		private FlexibleBounds _BoundsValue = new FlexibleBounds();

#if ARBOR_DOC_JA
		/// <summary>
		/// Color型の値
		/// </summary>
#else
		/// <summary>
		/// Color type value
		/// </summary>
#endif
		[SerializeField]
		private FlexibleColor _ColorValue = new FlexibleColor(Color.white);

#if ARBOR_DOC_JA
		/// <summary>
		/// Component型の値
		/// </summary>
#else
		/// <summary>
		/// Component type value
		/// </summary>
#endif
		[SerializeField]
		private FlexibleComponent _ComponentValue = new FlexibleComponent();

#if ARBOR_DOC_JA
		/// <summary>
		/// InputSlot型の値
		/// </summary>
#else
		/// <summary>
		/// InputSlot type value
		/// </summary>
#endif
		[SerializeField]
		[HideSlotFields]
		private InputSlotTypable _InputSlotValue = new InputSlotTypable();

#if ARBOR_DOC_JA
		/// <summary>
		/// 参照する型。Enum、Component、Slotで使用。
		/// </summary>
#else
		/// <summary>
		/// Type to reference. Used in Enum, Component, Slot.
		/// </summary>
#endif
		[SerializeField]
		private ClassTypeReference _ReferenceType = new ClassTypeReference();

		#endregion // Serialize fields

		object GetValue()
		{
			switch (_Type)
			{
				case Type.None:
					return null;
				case Type.Int:
					return _IntValue.value;
				case Type.Float:
					return _FloatValue.value;
				case Type.Bool:
					return _BoolValue.value;
				case Type.String:
					return _StringValue.value;
				case Type.Long:
					return _LongValue.value;
				case Type.Enum:
					if (EnumFieldUtility.IsEnum(_ReferenceType.type))
					{
						return EnumFieldUtility.ToEnum(_ReferenceType.type, _EnumValue.value);
					}
					return _EnumValue.value;
				case Type.GameObject:
					return _GameObjectValue.value;
				case Type.Vector2:
					return _Vector2Value.value;
				case Type.Vector3:
					return _Vector3Value.value;
				case Type.Quaternion:
					return _QuaternionValue.value;
				case Type.Rect:
					return _RectValue.value;
				case Type.Bounds:
					return _BoundsValue.value;
				case Type.Color:
					return _ColorValue.value;
				case Type.Component:
					return _ComponentValue.value;
				case Type.Slot:
					object value = null;
					if (_InputSlotValue.GetValue(ref value))
					{
						value = DynamicReflection.DynamicUtility.Rebox(value);
						return value;
					}
					else if (_InputSlotValue.dataType != null)
					{
						return DynamicReflection.DynamicUtility.GetDefault(_InputSlotValue.dataType);
					}
					break;
			}

			return null;
		}

		public void SendMessage(GameObject gameObject, string methodName, SendMessageOptions options)
		{
			if (gameObject == null)
			{
				return;
			}

			if (_Type == Type.None)
			{
				gameObject.SendMessage(methodName, options);
			}
			else
			{
				gameObject.SendMessage(methodName, GetValue(), options);
			}
		}

		public void SendMessageUpwards(GameObject gameObject, string methodName, SendMessageOptions options)
		{
			if (gameObject == null)
			{
				return;
			}

			if (_Type == Type.None)
			{
				gameObject.SendMessageUpwards(methodName, options);
			}
			else
			{
				gameObject.SendMessageUpwards(methodName, GetValue(), options);
			}
		}

		public void BroadcastMessage(GameObject gameObject, string methodName, SendMessageOptions options)
		{
			if (gameObject == null)
			{
				return;
			}

			if (_Type == Type.None)
			{
				gameObject.BroadcastMessage(methodName, options);
			}
			else
			{
				gameObject.BroadcastMessage(methodName, GetValue(), options);
			}
		}

		void ISerializationCallbackReceiver.OnBeforeSerialize()
		{
		}

		void ISerializationCallbackReceiver.OnAfterDeserialize()
		{
			IOverrideConstraint parameterOverrideConstraint = null;
			switch (_Type)
			{
				case Type.Enum:
					{
						parameterOverrideConstraint = _EnumValue;
					}
					break;
				case Type.Component:
					{
						parameterOverrideConstraint = _ComponentValue;
					}
					break;
			}
			if (parameterOverrideConstraint != null)
			{
				var referenceType = _ReferenceType.type;
				var typeConstraint = referenceType != null ? new ClassConstraintInfo() { baseType = referenceType } : null;
				parameterOverrideConstraint.overrideConstraint = typeConstraint;
			}
		}
	}
}
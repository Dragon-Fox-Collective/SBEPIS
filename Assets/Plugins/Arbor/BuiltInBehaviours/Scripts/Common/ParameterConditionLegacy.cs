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
	/// パラメータの状態チェッククラス
	/// </summary>
#else
	/// <summary>
	/// Condition check class of Parameter
	/// </summary>
#endif
	[System.Serializable]
	[Arbor.Internal.Documentable]
	public sealed class ParameterConditionLegacy : ISerializationCallbackReceiver, ISerializeVersionCallbackReceiver
	{
		#region enum

#if ARBOR_DOC_JA
		/// <summary>
		/// 比較タイプ
		/// </summary>
#else
		/// <summary>
		/// Compare type
		/// </summary>
#endif
		public enum CompareTypeOld
		{
			/// <summary>
			/// Value1 &gt; Value2
			/// </summary>
			Greater,

			/// <summary>
			/// Value1 &lt; Value2
			/// </summary>
			Less,

			/// <summary>
			/// Value1 == Value2
			/// </summary>
			Equals,

			/// <summary>
			/// Value1 != Value2
			/// </summary>
			NotEquals,
		}

		#endregion // enum

		#region Serialize fields

#if ARBOR_DOC_JA
		/// <summary>
		/// パラメータの参照
		/// </summary>
#else
		/// <summary>
		/// Parameter reference
		/// </summary>
#endif
		[FormerlySerializedAs("reference")]
		[SerializeField]
		internal ParameterReference _Reference = new ParameterReference();

#if ARBOR_DOC_JA
		/// <summary>
		/// 参照するパラメータのタイプ。reference.typeがParameterReferenceType.Constant以外の時に使用する。
		/// </summary>
#else
		/// <summary>
		/// Parameters to be referenced. Used when reference.type is other than ParameterReferenceType.Constant.
		/// </summary>
#endif
		[SerializeField]
		internal Parameter.Type _ParameterType = Parameter.Type.Int;

#if ARBOR_DOC_JA
		/// <summary>
		/// 参照するパラメータの型。reference.typeがParameterReferenceType.Constant以外の時に使用する。
		/// </summary>
#else
		/// <summary>
		/// Parameters to be referenced type. Used when reference.type is other than ParameterReferenceType.Constant.
		/// </summary>
#endif
		[SerializeField]
		internal ClassTypeReference _ReferenceType = new ClassTypeReference();

#if ARBOR_DOC_JA
		/// <summary>
		/// 比較タイプ
		/// </summary>
#else
		/// <summary>
		/// Compare type
		/// </summary>
#endif
		[SerializeField]
		internal CompareType _CompareType = CompareType.Equals;

#if ARBOR_DOC_JA
		/// <summary>
		/// 比較するint値
		/// </summary>
#else
		/// <summary>
		/// int value to compare
		/// </summary>
#endif
		[SerializeField]
		internal FlexibleInt _IntValue = new FlexibleInt();

#if ARBOR_DOC_JA
		/// <summary>
		/// 比較するlong値
		/// </summary>
#else
		/// <summary>
		/// long value to compare
		/// </summary>
#endif
		[SerializeField]
		internal FlexibleLong _LongValue = new FlexibleLong();

#if ARBOR_DOC_JA
		/// <summary>
		/// 比較するfloat値
		/// </summary>
#else
		/// <summary>
		/// float value to compare
		/// </summary>
#endif
		[SerializeField]
		internal FlexibleFloat _FloatValue = new FlexibleFloat();

#if ARBOR_DOC_JA
		/// <summary>
		/// 比較するbool値
		/// </summary>
#else
		/// <summary>
		/// bool value to compare
		/// </summary>
#endif
		[SerializeField]
		internal FlexibleBool _BoolValue = new FlexibleBool();

#if ARBOR_DOC_JA
		/// <summary>
		/// 比較するstring値
		/// </summary>
#else
		/// <summary>
		/// string value to compare
		/// </summary>
#endif
		[SerializeField]
		internal FlexibleString _StringValue = new FlexibleString();

#if ARBOR_DOC_JA
		/// <summary>
		/// 比較するEnum値
		/// </summary>
#else
		/// <summary>
		/// Enum value to compare
		/// </summary>
#endif
		[SerializeField]
		internal FlexibleEnumAny _EnumValue = new FlexibleEnumAny();

#if ARBOR_DOC_JA
		/// <summary>
		/// 比較するGameObject値
		/// </summary>
#else
		/// <summary>
		/// GameObject value to compare
		/// </summary>
#endif
		[SerializeField]
		internal FlexibleGameObject _GameObjectValue = new FlexibleGameObject();

#if ARBOR_DOC_JA
		/// <summary>
		/// 比較するVector2値
		/// </summary>
#else
		/// <summary>
		/// Vector2 value to compare
		/// </summary>
#endif
		[SerializeField]
		internal FlexibleVector2 _Vector2Value = new FlexibleVector2();

#if ARBOR_DOC_JA
		/// <summary>
		/// 比較するVector3値
		/// </summary>
#else
		/// <summary>
		/// Vector3 value to compare
		/// </summary>
#endif
		[SerializeField]
		internal FlexibleVector3 _Vector3Value = new FlexibleVector3();

#if ARBOR_DOC_JA
		/// <summary>
		/// 比較するQuaternion値
		/// </summary>
#else
		/// <summary>
		/// Quaternion value to compare
		/// </summary>
#endif
		[SerializeField]
		internal FlexibleQuaternion _QuaternionValue = new FlexibleQuaternion();

#if ARBOR_DOC_JA
		/// <summary>
		/// 比較するRect値
		/// </summary>
#else
		/// <summary>
		/// Rect value to compare
		/// </summary>
#endif
		[SerializeField]
		internal FlexibleRect _RectValue = new FlexibleRect();

#if ARBOR_DOC_JA
		/// <summary>
		/// 比較するBounds値
		/// </summary>
#else
		/// <summary>
		/// Bounds value to compare
		/// </summary>
#endif
		[SerializeField]
		internal FlexibleBounds _BoundsValue = new FlexibleBounds();

#if ARBOR_DOC_JA
		/// <summary>
		/// 比較するColor値
		/// </summary>
#else
		/// <summary>
		/// Color value to compare
		/// </summary>
#endif
		[SerializeField]
		internal FlexibleColor _ColorValue = new FlexibleColor(Color.white);

#if ARBOR_DOC_JA
		/// <summary>
		/// 比較するTransform値
		/// </summary>
#else
		/// <summary>
		/// Transform value to compare
		/// </summary>
#endif
		[SerializeField]
		internal FlexibleTransform _TransformValue = new FlexibleTransform();

#if ARBOR_DOC_JA
		/// <summary>
		/// 比較するRectTransform値
		/// </summary>
#else
		/// <summary>
		/// RectTransform value to compare
		/// </summary>
#endif
		[SerializeField]
		internal FlexibleRectTransform _RectTransformValue = new FlexibleRectTransform();

#if ARBOR_DOC_JA
		/// <summary>
		/// 比較するRigidbody値
		/// </summary>
#else
		/// <summary>
		/// Rigidbody value to compare
		/// </summary>
#endif
		[SerializeField]
		internal FlexibleRigidbody _RigidbodyValue = new FlexibleRigidbody();

#if ARBOR_DOC_JA
		/// <summary>
		/// 比較するRigidbody2D値
		/// </summary>
#else
		/// <summary>
		/// Rigidbody2D value to compare
		/// </summary>
#endif
		[SerializeField]
		internal FlexibleRigidbody2D _Rigidbody2DValue = new FlexibleRigidbody2D();

#if ARBOR_DOC_JA
		/// <summary>
		/// 比較するComponent値
		/// </summary>
#else
		/// <summary>
		/// Component value to compare
		/// </summary>
#endif
		[SerializeField]
		internal FlexibleComponent _ComponentValue = new FlexibleComponent();

#if ARBOR_DOC_JA
		/// <summary>
		/// 比較するAssetObject値
		/// </summary>
#else
		/// <summary>
		/// Component value to compare
		/// </summary>
#endif
		[SerializeField]
		internal FlexibleAssetObject _AssetObjectValue = new FlexibleAssetObject();

		[SerializeField]
		[HideInInspector]
		private SerializeVersion _SerializeVersion = new SerializeVersion();

		#region old

		[SerializeField]
		[HideInInspector]
		[FormerlySerializedAs("_SerializeVersion")]
		private int _SerializeVersionOld = 0;

		[SerializeField]
		[HideInInspector]
		[FormerlySerializedAs("_IsInitialized")]
		private bool _IsInitializedOld = true;

		// _SerializeVersion == 0
		[FormerlySerializedAs("type")]
		[FormerlySerializedAs("_Type")]
		[SerializeField]
		[HideInInspector]
		private CompareTypeOld _OldType = CompareTypeOld.Greater;

		// ParameterTransition._SerializeVersion == 0
		[FormerlySerializedAs("intValue")]
		[SerializeField]
		[HideInInspector]
		private int _OldIntValue = 0;

		// ParameterTransition._SerializeVersion == 0
		[FormerlySerializedAs("floatValue")]
		[SerializeField]
		[HideInInspector]
		private float _OldFloatValue = 0f;

		// ParameterTransition._SerializeVersion == 0
		[FormerlySerializedAs("boolValue")]
		[SerializeField]
		[HideInInspector]
		private bool _OldBoolValue = false;

		#endregion // old

		#endregion // Serialize fields

		private const int kCurrentSerializeVersion = 1;

#if ARBOR_DOC_JA
		/// <summary>
		/// パラメータへの参照。
		/// </summary>
#else
		/// <summary>
		/// Reference to parameter.
		/// </summary>
#endif
		public ParameterReference reference
		{
			get
			{
				return _Reference;
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 参照するパラメータのタイプ。reference.typeがParameterReferenceType.Constant以外の時に使用する。
		/// </summary>
#else
		/// <summary>
		/// Parameters to be referenced. Used when reference.type is other than ParameterReferenceType.Constant.
		/// </summary>
#endif
		public Parameter.Type parameterType
		{
			get
			{
				return _ParameterType;
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 参照するパラメータの型。reference.typeがParameterReferenceType.Constant以外の時に使用する。
		/// </summary>
#else
		/// <summary>
		/// Parameters to be referenced type. Used when reference.type is other than ParameterReferenceType.Constant.
		/// </summary>
#endif
		public System.Type referenceType
		{
			get
			{
				return _ReferenceType;
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 比較タイプ
		/// </summary>
#else
		/// <summary>
		/// Compare type
		/// </summary>
#endif
		public CompareType compareType
		{
			get
			{
				return _CompareType;
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 比較対象のint値
		/// </summary>
#else
		/// <summary>
		/// The int value to be compared
		/// </summary>
#endif
		public int intValue
		{
			get
			{
				return (int)_IntValue;
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 比較対象のlong値
		/// </summary>
#else
		/// <summary>
		/// The long value to be compared
		/// </summary>
#endif
		public long longValue
		{
			get
			{
				return (long)_LongValue;
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 比較対象のfloat値
		/// </summary>
#else
		/// <summary>
		/// The float value to be compared
		/// </summary>
#endif
		public float floatValue
		{
			get
			{
				return (float)_FloatValue;
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 比較対象のbool値
		/// </summary>
#else
		/// <summary>
		/// The bool value to be compared
		/// </summary>
#endif
		public bool boolValue
		{
			get
			{
				return (bool)_BoolValue;
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 比較対象のstring値
		/// </summary>
#else
		/// <summary>
		/// The string value to be compared
		/// </summary>
#endif
		public string stringValue
		{
			get
			{
				return (string)_StringValue;
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 比較対象のEnum値
		/// </summary>
#else
		/// <summary>
		/// The enum value to be compared
		/// </summary>
#endif
		public int enumValue
		{
			get
			{
				return _EnumValue.value;
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 比較対象のGameObject値
		/// </summary>
#else
		/// <summary>
		/// The GameObject value to be compared
		/// </summary>
#endif
		public GameObject gameObjectValue
		{
			get
			{
				return (GameObject)_GameObjectValue;
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 比較対象のVector2値
		/// </summary>
#else
		/// <summary>
		/// The Vector2 value to be compared
		/// </summary>
#endif
		public Vector2 vector2Value
		{
			get
			{
				return (Vector2)_Vector2Value;
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 比較対象のVector3値
		/// </summary>
#else
		/// <summary>
		/// The Vector3 value to be compared
		/// </summary>
#endif
		public Vector3 vector3Value
		{
			get
			{
				return (Vector3)_Vector3Value;
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 比較対象のQuaternion値
		/// </summary>
#else
		/// <summary>
		/// The Quaternion value to be compared
		/// </summary>
#endif
		public Quaternion quaternionValue
		{
			get
			{
				return (Quaternion)_QuaternionValue;
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 比較対象のRect値
		/// </summary>
#else
		/// <summary>
		/// The Rect value to be compared
		/// </summary>
#endif
		public Rect rectValue
		{
			get
			{
				return (Rect)_RectValue;
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 比較対象のBounds値
		/// </summary>
#else
		/// <summary>
		/// The Bounds value to be compared
		/// </summary>
#endif
		public Bounds boundsValue
		{
			get
			{
				return (Bounds)_BoundsValue;
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 比較対象のColor値
		/// </summary>
#else
		/// <summary>
		/// The Color value to be compared
		/// </summary>
#endif
		public Color colorValue
		{
			get
			{
				return (Color)_ColorValue;
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 比較対象のTransform値
		/// </summary>
#else
		/// <summary>
		/// The Transform value to be compared
		/// </summary>
#endif
		public Transform transformValue
		{
			get
			{
				return (Transform)_TransformValue;
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 比較対象のRectTransform値
		/// </summary>
#else
		/// <summary>
		/// The RectTransform value to be compared
		/// </summary>
#endif
		public RectTransform rectTransformValue
		{
			get
			{
				return (RectTransform)_RectTransformValue;
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 比較対象のRigidbody値
		/// </summary>
#else
		/// <summary>
		/// The Rigidbody value to be compared
		/// </summary>
#endif
		public Rigidbody rigidbodyValue
		{
			get
			{
				return (Rigidbody)_RigidbodyValue;
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 比較対象のRigidbody2D値
		/// </summary>
#else
		/// <summary>
		/// The Rigidbody2D value to be compared
		/// </summary>
#endif
		public Rigidbody2D rigidbody2DValue
		{
			get
			{
				return (Rigidbody2D)_Rigidbody2DValue;
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 比較対象のComponent値
		/// </summary>
#else
		/// <summary>
		/// The Component value to be compared
		/// </summary>
#endif
		public Component componentValue
		{
			get
			{
				return (Component)_ComponentValue;
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 比較対象のComponent値
		/// </summary>
#else
		/// <summary>
		/// The Component value to be compared
		/// </summary>
#endif
		public Object assetObjectValue
		{
			get
			{
				return (Object)_AssetObjectValue;
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// ParameterConditionLegacyコンストラクタ
		/// </summary>
#else
		/// <summary>
		/// ParameterConditionLegacy constructor
		/// </summary>
#endif
		public ParameterConditionLegacy()
		{
			// Initialize when calling from script.
			_SerializeVersion.Initialize(this);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 条件チェック
		/// </summary>
		/// <returns>条件が一致する場合はtrueを返す。</returns>
#else
		/// <summary>
		/// Condition check
		/// </summary>
		/// <returns>Returns true if the conditions match.</returns>
#endif
		public bool CheckCondition()
		{
			Parameter parameter = _Reference.parameter;
			if (parameter == null)
			{
				return true;
			}

			switch (parameter.type)
			{
				case Parameter.Type.Int:
					{
						int intValue = _IntValue.value;
						return CompareUtility.Compare(parameter.intValue, intValue, _CompareType);
					}
				case Parameter.Type.Long:
					{
						long longValue = _LongValue.value;
						return CompareUtility.Compare(parameter.longValue, longValue, _CompareType);
					}
				case Parameter.Type.Float:
					{
						float floatValue = _FloatValue.value;
						return CompareUtility.Compare(parameter.floatValue, floatValue, _CompareType);
					}
				case Parameter.Type.Bool:
					{
						bool boolValue = _BoolValue.value;
						if (parameter.boolValue == boolValue)
						{
							return true;
						}
					}
					break;
				case Parameter.Type.String:
					{
						string stringValue = _StringValue.value;
						return CompareUtility.Compare(parameter.stringValue, stringValue, _CompareType);
					}
				case Parameter.Type.Enum:
					{
						int enumValue = _EnumValue.value;
						if (parameter.enumIntValue == enumValue)
						{
							return true;
						}
					}
					break;
				case Parameter.Type.GameObject:
					{
						GameObject gameObjectValue = _GameObjectValue.value;
						if (parameter.gameObjectValue == gameObjectValue)
						{
							return true;
						}
					}
					break;
				case Parameter.Type.Vector2:
					{
						Vector2 vector2Value = _Vector2Value.value;
						if (parameter.vector2Value == vector2Value)
						{
							return true;
						}
					}
					break;
				case Parameter.Type.Vector3:
					{
						Vector3 vector3Value = _Vector3Value.value;
						if (parameter.vector3Value == vector3Value)
						{
							return true;
						}
					}
					break;
				case Parameter.Type.Quaternion:
					{
						Quaternion quaternionValue = _QuaternionValue.value;
						if (parameter.quaternionValue == quaternionValue)
						{
							return true;
						}
					}
					break;
				case Parameter.Type.Rect:
					{
						Rect rectValue = _RectValue.value;
						if (parameter.rectValue == rectValue)
						{
							return true;
						}
					}
					break;
				case Parameter.Type.Bounds:
					{
						Bounds boundsValue = _BoundsValue.value;
						if (parameter.boundsValue == boundsValue)
						{
							return true;
						}
					}
					break;
				case Parameter.Type.Color:
					{
						Color colorValue = _ColorValue.value;
						if (parameter.colorValue == colorValue)
						{
							return true;
						}
					}
					break;
				case Parameter.Type.Transform:
					{
						Transform transformValue = _TransformValue.value;
						if (parameter.transformValue == transformValue)
						{
							return true;
						}
					}
					break;
				case Parameter.Type.RectTransform:
					{
						RectTransform rectTransformValue = _RectTransformValue.value;
						if (parameter.rectTransformValue == rectTransformValue)
						{
							return true;
						}
					}
					break;
				case Parameter.Type.Rigidbody:
					{
						Rigidbody rigidbodyValue = _RigidbodyValue.value;
						if (parameter.rigidbodyValue == rigidbodyValue)
						{
							return true;
						}
					}
					break;
				case Parameter.Type.Rigidbody2D:
					{
						Rigidbody2D rigidbody2DValue = _Rigidbody2DValue.value;
						if (parameter.rigidbody2DValue == rigidbody2DValue)
						{
							return true;
						}
					}
					break;
				case Parameter.Type.Component:
					{
						Component componentValue = _ComponentValue.value;
						if (parameter.componentValue == componentValue)
						{
							return true;
						}
					}
					break;
				case Parameter.Type.AssetObject:
					{
						Object assetObjectValue = _AssetObjectValue.value;
						if (parameter.assetObjectValue == assetObjectValue)
						{
							return true;
						}
					}
					break;
			}

			return false;
		}

		internal void ParameterTransitionSerializeVer1()
		{
			_IntValue = (FlexibleInt)_OldIntValue;
			_FloatValue = (FlexibleFloat)_OldFloatValue;
			_BoolValue = (FlexibleBool)_OldBoolValue;
		}

		#region ISerializeVersionCallbackReceiver

		int ISerializeVersionCallbackReceiver.newestVersion
		{
			get
			{
				return kCurrentSerializeVersion;
			}
		}

		void ISerializeVersionCallbackReceiver.OnInitialize()
		{
			_SerializeVersion.version = kCurrentSerializeVersion;
		}

		void SerializeVer1()
		{
			switch (_OldType)
			{
				case CompareTypeOld.Greater:
					_CompareType = CompareType.Greater;
					break;
				case CompareTypeOld.Less:
					_CompareType = CompareType.Less;
					break;
				case CompareTypeOld.Equals:
					_CompareType = CompareType.Equals;
					break;
				case CompareTypeOld.NotEquals:
					_CompareType = CompareType.NotEquals;
					break;
			}
		}

		void ISerializeVersionCallbackReceiver.OnSerialize(int version)
		{
			switch (version)
			{
				case 0:
					SerializeVer1();
					break;
			}
		}

		void ISerializeVersionCallbackReceiver.OnVersioning()
		{
			if (_IsInitializedOld)
			{
				_SerializeVersion.version = _SerializeVersionOld;
			}
		}

		#endregion // ISerializeVersionCallbackReceiver

		#region ISerializationCallbackReceiver

		void ISerializationCallbackReceiver.OnBeforeSerialize()
		{
			_SerializeVersion.BeforeDeserialize();
		}

		void ISerializationCallbackReceiver.OnAfterDeserialize()
		{
			_SerializeVersion.AfterDeserialize();

			if (_ParameterReferenceConstrainter == null)
			{
				_ParameterReferenceConstrainter = new ParameterReferenceConstrainter(OnConstraintChangedType, OnConstraintDestroyParameter);
			}

			_ParameterReferenceConstrainter.Constraint(_Reference, _ParameterType, _ReferenceType.type);
		}

		[System.NonSerialized]
		private ParameterReferenceConstrainter _ParameterReferenceConstrainter;

		IOverrideConstraint GetParameterOverrideConstraint(Parameter.Type parameterType)
		{
			switch (parameterType)
			{
				case Parameter.Type.Component:
					return _ComponentValue;
				case Parameter.Type.Enum:
					return _EnumValue;
				case Parameter.Type.AssetObject:
					return _AssetObjectValue;
			}

			return null;
		}

		void OnConstraintChangedType(Parameter.Type parameterType, System.Type referenceType)
		{
			IOverrideConstraint parameterOverrideConstraint = GetParameterOverrideConstraint(parameterType);
			if (parameterOverrideConstraint != null)
			{
				var typeConstraint = referenceType != null ? new ClassConstraintInfo() { baseType = referenceType } : null;
				parameterOverrideConstraint.overrideConstraint = typeConstraint;
			}
		}

		void OnConstraintDestroyParameter(Parameter.Type parameterType)
		{
			Disconnect(parameterType);

			IOverrideConstraint parameterOverrideConstraint = GetParameterOverrideConstraint(parameterType);
			if (parameterOverrideConstraint != null)
			{
				parameterOverrideConstraint.overrideConstraint = null;
			}
		}

		public void Destroy()
		{
			if (_ParameterReferenceConstrainter != null)
			{
				_ParameterReferenceConstrainter.Destroy();
				_ParameterReferenceConstrainter = null;
			}
		}

		void Disconnect(Parameter.Type parameterType)
		{
			switch (parameterType)
			{
				case Parameter.Type.Int:
					_IntValue.Disconnect();
					break;
				case Parameter.Type.Long:
					_LongValue.Disconnect();
					break;
				case Parameter.Type.Float:
					_FloatValue.Disconnect();
					break;
				case Parameter.Type.Bool:
					_BoolValue.Disconnect();
					break;
				case Parameter.Type.String:
					_StringValue.Disconnect();
					break;
				case Parameter.Type.Enum:
					_EnumValue.Disconnect();
					break;
				case Parameter.Type.GameObject:
					_GameObjectValue.Disconnect();
					break;
				case Parameter.Type.Vector2:
					_Vector2Value.Disconnect();
					break;
				case Parameter.Type.Vector3:
					_Vector3Value.Disconnect();
					break;
				case Parameter.Type.Quaternion:
					_QuaternionValue.Disconnect();
					break;
				case Parameter.Type.Rect:
					_RectValue.Disconnect();
					break;
				case Parameter.Type.Bounds:
					_BoundsValue.Disconnect();
					break;
				case Parameter.Type.Color:
					_ColorValue.Disconnect();
					break;
				case Parameter.Type.Transform:
					_TransformValue.Disconnect();
					break;
				case Parameter.Type.RectTransform:
					_RectTransformValue.Disconnect();
					break;
				case Parameter.Type.Rigidbody:
					_RigidbodyValue.Disconnect();
					break;
				case Parameter.Type.Rigidbody2D:
					_Rigidbody2DValue.Disconnect();
					break;
				case Parameter.Type.Component:
					_ComponentValue.Disconnect();
					break;
				case Parameter.Type.AssetObject:
					_AssetObjectValue.Disconnect();
					break;
			}
		}

		#endregion ISerializationCallbackReceiver
	}
}
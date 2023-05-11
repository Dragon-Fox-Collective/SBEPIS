//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using UnityEngine.Serialization;

namespace Arbor.BehaviourTree.Actions
{
#if ARBOR_DOC_JA
	/// <summary>
	/// Parameterの値を演算して変更する。
	/// </summary>
#else
	/// <summary>
	/// Change by calculating the value of the Parameter.
	/// </summary>
#endif
	[AddBehaviourMenu("Parameter/CalcParameter")]
	[BuiltInBehaviour]
	[AddComponentMenu("")]
	public sealed class CalcParameter : ActionBehaviour, INodeBehaviourSerializationCallbackReceiver
	{
		#region Serialize fields

#if ARBOR_DOC_JA
		/// <summary>
		/// 参照するパラメータ。
		/// </summary>
#else
		/// <summary>
		/// Parameters to be referenced.
		/// </summary>
#endif
		[SerializeField]
		[FormerlySerializedAs("reference")]
		private ParameterReference _Reference = new ParameterReference();

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
		private Parameter.Type _ParameterType = Parameter.Type.Int;

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
		private ClassTypeReference _ReferenceType = new ClassTypeReference();

#if ARBOR_DOC_JA
		/// <summary>
		/// 演算するタイプ(Int、Long、Float、Stringのみ)。
		/// </summary>
#else
		/// <summary>
		/// Type to calculate (Int, Long, Float, String only).
		/// </summary>
#endif
		[SerializeField]
		[Internal.DocumentType(typeof(CalcFunction))]
		private FlexibleCalcFunction _Function = new FlexibleCalcFunction(CalcFunction.Assign);

#if ARBOR_DOC_JA
		/// <summary>
		/// 演算するInt値
		/// </summary>
#else
		/// <summary>
		/// Int value to be computed
		/// </summary>
#endif
		[SerializeField]
		private FlexibleInt _IntValue = new FlexibleInt();

#if ARBOR_DOC_JA
		/// <summary>
		/// 演算するLong値
		/// </summary>
#else
		/// <summary>
		/// Long value to be computed
		/// </summary>
#endif
		[SerializeField]
		private FlexibleLong _LongValue = new FlexibleLong();

#if ARBOR_DOC_JA
		/// <summary>
		/// 演算するFloat値
		/// </summary>
#else
		/// <summary>
		/// Float value to be computed
		/// </summary>
#endif
		[SerializeField]
		private FlexibleFloat _FloatValue = new FlexibleFloat();

#if ARBOR_DOC_JA
		/// <summary>
		/// 演算するBool値
		/// </summary>
#else
		/// <summary>
		/// Bool value to be computed
		/// </summary>
#endif
		[SerializeField]
		private FlexibleBool _BoolValue = new FlexibleBool();

#if ARBOR_DOC_JA
		/// <summary>
		/// 演算するString値
		/// </summary>
#else
		/// <summary>
		/// String value to be computed
		/// </summary>
#endif
		[SerializeField]
		private FlexibleString _StringValue = new FlexibleString();

#if ARBOR_DOC_JA
		/// <summary>
		/// 演算するEnum値
		/// </summary>
#else
		/// <summary>
		/// Enum value to be computed
		/// </summary>
#endif
		[SerializeField]
		private FlexibleEnumAny _EnumValue = new FlexibleEnumAny();

#if ARBOR_DOC_JA
		/// <summary>
		/// 演算するGameObject値
		/// </summary>
#else
		/// <summary>
		/// GameObject value to be computed
		/// </summary>
#endif
		[SerializeField]
		private FlexibleGameObject _GameObjectValue = new FlexibleGameObject();

#if ARBOR_DOC_JA
		/// <summary>
		/// 演算するVector2値
		/// </summary>
#else
		/// <summary>
		/// Vector2 value to be computed
		/// </summary>
#endif
		[SerializeField]
		private FlexibleVector2 _Vector2Value = new FlexibleVector2();

#if ARBOR_DOC_JA
		/// <summary>
		/// 演算するVector3値
		/// </summary>
#else
		/// <summary>
		/// Vector3 value to be computed
		/// </summary>
#endif
		[SerializeField]
		private FlexibleVector3 _Vector3Value = new FlexibleVector3();

#if ARBOR_DOC_JA
		/// <summary>
		/// 演算するQuaternion値
		/// </summary>
#else
		/// <summary>
		/// Quaternion value to be computed
		/// </summary>
#endif
		[SerializeField]
		private FlexibleQuaternion _QuaternionValue = new FlexibleQuaternion();

#if ARBOR_DOC_JA
		/// <summary>
		/// 演算するRect値
		/// </summary>
#else
		/// <summary>
		/// Rect value to be computed
		/// </summary>
#endif
		[SerializeField]
		private FlexibleRect _RectValue = new FlexibleRect();

#if ARBOR_DOC_JA
		/// <summary>
		/// 演算するBounds値
		/// </summary>
#else
		/// <summary>
		/// Bounds value to be computed
		/// </summary>
#endif
		[SerializeField]
		private FlexibleBounds _BoundsValue = new FlexibleBounds();

#if ARBOR_DOC_JA
		/// <summary>
		/// 演算するColor値
		/// </summary>
#else
		/// <summary>
		/// Color value to be computed
		/// </summary>
#endif
		[SerializeField]
		private FlexibleColor _ColorValue = new FlexibleColor(Color.white);

#if ARBOR_DOC_JA
		/// <summary>
		/// 演算するVector4値
		/// </summary>
#else
		/// <summary>
		/// Vector4 value to be computed
		/// </summary>
#endif
		[SerializeField]
		private FlexibleVector4 _Vector4Value = new FlexibleVector4();

#if ARBOR_DOC_JA
		/// <summary>
		/// 演算するVector2Int値
		/// </summary>
#else
		/// <summary>
		/// Vector2Int value to be computed
		/// </summary>
#endif
		[SerializeField]
		private FlexibleVector2Int _Vector2IntValue = new FlexibleVector2Int();

#if ARBOR_DOC_JA
		/// <summary>
		/// 演算するVector3Int値
		/// </summary>
#else
		/// <summary>
		/// Vector3Int value to be computed
		/// </summary>
#endif
		[SerializeField]
		private FlexibleVector3Int _Vector3IntValue = new FlexibleVector3Int();

#if ARBOR_DOC_JA
		/// <summary>
		/// 演算するRectInt値
		/// </summary>
#else
		/// <summary>
		/// RectInt value to be computed
		/// </summary>
#endif
		[SerializeField]
		private FlexibleRectInt _RectIntValue = new FlexibleRectInt();

#if ARBOR_DOC_JA
		/// <summary>
		/// 演算するBoundsInt値
		/// </summary>
#else
		/// <summary>
		/// BoundsInt value to be computed
		/// </summary>
#endif
		[SerializeField]
		private FlexibleBoundsInt _BoundsIntValue = new FlexibleBoundsInt();

#if ARBOR_DOC_JA
		/// <summary>
		/// 演算するTransform値
		/// </summary>
#else
		/// <summary>
		/// Transform value to be computed
		/// </summary>
#endif
		[SerializeField]
		private FlexibleTransform _TransformValue = new FlexibleTransform();

#if ARBOR_DOC_JA
		/// <summary>
		/// 演算するRectTransform値
		/// </summary>
#else
		/// <summary>
		/// RectTransform value to be computed
		/// </summary>
#endif
		[SerializeField]
		private FlexibleRectTransform _RectTransformValue = new FlexibleRectTransform();

#if ARBOR_DOC_JA
		/// <summary>
		/// 演算するRigidbody値
		/// </summary>
#else
		/// <summary>
		/// Rigidbody value to be computed
		/// </summary>
#endif
		[SerializeField]
		private FlexibleRigidbody _RigidbodyValue = new FlexibleRigidbody();

#if ARBOR_DOC_JA
		/// <summary>
		/// 演算するRigidbody2D値
		/// </summary>
#else
		/// <summary>
		/// Rigidbody2D value to be computed
		/// </summary>
#endif
		[SerializeField]
		private FlexibleRigidbody2D _Rigidbody2DValue = new FlexibleRigidbody2D();

#if ARBOR_DOC_JA
		/// <summary>
		/// 演算するComponent値
		/// </summary>
#else
		/// <summary>
		/// Component value to be computed
		/// </summary>
#endif
		[SerializeField]
		private FlexibleComponent _ComponentValue = new FlexibleComponent();

#if ARBOR_DOC_JA
		/// <summary>
		/// 演算するAssetObject値
		/// </summary>
#else
		/// <summary>
		/// AssetObject value to be computed
		/// </summary>
#endif
		[SerializeField]
		private FlexibleAssetObject _AssetObjectValue = new FlexibleAssetObject();

		[SerializeField]
		[HideInInspector]
		private int _SerializeVersion = 0;

		#region old

		[SerializeField]
		[HideInInspector]
		[FormerlySerializedAs("function")]
		private CalcFunction _OldFunction = CalcFunction.Assign;

		#endregion

		#endregion

		private const int kCurrentSerializeVersion = 1;

		public Parameter.Type parameterType
		{
			get
			{
				return _ParameterType;
			}
		}

		public System.Type referenceType
		{
			get
			{
				return _ReferenceType;
			}
		}

		public int intValue
		{
			get
			{
				return _IntValue.value;
			}
		}

		public long longValue
		{
			get
			{
				return _LongValue.value;
			}
		}

		public float floatValue
		{
			get
			{
				return _FloatValue.value;
			}
		}

		public bool boolValue
		{
			get
			{
				return _BoolValue.value;
			}
		}

		public string stringValue
		{
			get
			{
				return _StringValue.value;
			}
		}

		public int enumValue
		{
			get
			{
				return _EnumValue.value;
			}
		}

		public GameObject gameObjectValue
		{
			get
			{
				return _GameObjectValue.value;
			}
		}

		public Vector2 vector2Value
		{
			get
			{
				return _Vector2Value.value;
			}
		}

		public Vector3 vector3Value
		{
			get
			{
				return _Vector3Value.value;
			}
		}

		public Quaternion quaternionValue
		{
			get
			{
				return _QuaternionValue.value;
			}
		}

		public Rect rectValue
		{
			get
			{
				return _RectValue.value;
			}
		}

		public Bounds boundsValue
		{
			get
			{
				return _BoundsValue.value;
			}
		}

		public Color colorValue
		{
			get
			{
				return _ColorValue.value;
			}
		}

		public Vector4 vector4Value
		{
			get
			{
				return _Vector4Value.value;
			}
		}

		public Vector2Int vector2IntValue
		{
			get
			{
				return _Vector2IntValue.value;
			}
		}

		public Vector3Int vector3IntValue
		{
			get
			{
				return _Vector3IntValue.value;
			}
		}

		public RectInt rectIntValue
		{
			get
			{
				return _RectIntValue.value;
			}
		}

		public BoundsInt boundsIntValue
		{
			get
			{
				return _BoundsIntValue.value;
			}
		}

		public Transform transformValue
		{
			get
			{
				return _TransformValue.value;
			}
		}

		public RectTransform rectTransformValue
		{
			get
			{
				return _RectTransformValue.value;
			}
		}

		public Rigidbody rigidbodyValue
		{
			get
			{
				return _RigidbodyValue.value;
			}
		}

		public Rigidbody2D rigidbody2DValue
		{
			get
			{
				return _Rigidbody2DValue.value;
			}
		}

		public Component componentValue
		{
			get
			{
				return _ComponentValue.value;
			}
		}

		public Object assetObjectValue
		{
			get
			{
				return _AssetObjectValue.value;
			}
		}

		protected override void OnExecute()
		{
			Parameter parameter = _Reference.parameter;

			if (parameter == null)
			{
				FinishExecute(false);
				return;
			}

			switch (parameter.type)
			{
				case Parameter.Type.Int:
					{
						int value = parameter.intValue;
						switch (_Function.value)
						{
							case CalcFunction.Assign:
								value = intValue;
								break;
							case CalcFunction.Add:
								value += intValue;
								break;
						}
						parameter.intValue = value;
					}
					break;
				case Parameter.Type.Long:
					{
						long value = parameter.longValue;
						switch (_Function.value)
						{
							case CalcFunction.Assign:
								value = longValue;
								break;
							case CalcFunction.Add:
								value += longValue;
								break;
						}
						parameter.longValue = value;
					}
					break;
				case Parameter.Type.Float:
					{
						float value = parameter.floatValue;
						switch (_Function.value)
						{
							case CalcFunction.Assign:
								value = floatValue;
								break;
							case CalcFunction.Add:
								value += floatValue;
								break;
						}
						parameter.floatValue = value;
					}
					break;
				case Parameter.Type.Bool:
					{
						parameter.boolValue = boolValue;
					}
					break;
				case Parameter.Type.String:
					{
						string value = parameter.stringValue;
						switch (_Function.value)
						{
							case CalcFunction.Assign:
								value = stringValue;
								break;
							case CalcFunction.Add:
								value += stringValue;
								break;
						}
						parameter.stringValue = value;
					}
					break;
				case Parameter.Type.Enum:
					{
						parameter.enumIntValue = enumValue;
					}
					break;
				case Parameter.Type.GameObject:
					{
						parameter.gameObjectValue = gameObjectValue;
					}
					break;
				case Parameter.Type.Vector2:
					{
						parameter.vector2Value = vector2Value;
					}
					break;
				case Parameter.Type.Vector3:
					{
						parameter.vector3Value = vector3Value;
					}
					break;
				case Parameter.Type.Quaternion:
					{
						parameter.quaternionValue = quaternionValue;
					}
					break;
				case Parameter.Type.Rect:
					{
						parameter.rectValue = rectValue;
					}
					break;
				case Parameter.Type.Bounds:
					{
						parameter.boundsValue = boundsValue;
					}
					break;
				case Parameter.Type.Color:
					{
						parameter.colorValue = colorValue;
					}
					break;
				case Parameter.Type.Vector4:
					{
						parameter.vector4Value = vector4Value;
					}
					break;
				case Parameter.Type.Vector2Int:
					{
						parameter.vector2IntValue = vector2IntValue;
					}
					break;
				case Parameter.Type.Vector3Int:
					{
						parameter.vector3IntValue = vector3IntValue;
					}
					break;
				case Parameter.Type.RectInt:
					{
						parameter.rectIntValue = rectIntValue;
					}
					break;
				case Parameter.Type.BoundsInt:
					{
						parameter.boundsIntValue = boundsIntValue;
					}
					break;
				case Parameter.Type.Transform:
					{
						parameter.transformValue = transformValue;
					}
					break;
				case Parameter.Type.RectTransform:
					{
						parameter.rectTransformValue = rectTransformValue;
					}
					break;
				case Parameter.Type.Rigidbody:
					{
						parameter.rigidbodyValue = rigidbodyValue;
					}
					break;
				case Parameter.Type.Rigidbody2D:
					{
						parameter.rigidbody2DValue = rigidbody2DValue;
					}
					break;
				case Parameter.Type.Component:
					{
						parameter.componentValue = componentValue;
					}
					break;
				case Parameter.Type.AssetObject:
					{
						parameter.assetObjectValue = assetObjectValue;
					}
					break;
			}

			FinishExecute(true);
		}

		void Reset()
		{
			_SerializeVersion = kCurrentSerializeVersion;
		}

		void SerializeVer1()
		{
			_Function = (FlexibleCalcFunction)_OldFunction;
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

			if (_ParameterReferenceConstrainter == null)
			{
				_ParameterReferenceConstrainter = new ParameterReferenceConstrainter(OnConstraintChangedType, OnConstraintDestroyParamerter);
			}

			_ParameterReferenceConstrainter.Constraint(_Reference, _ParameterType, _ReferenceType.type);
		}

		[System.NonSerialized]
		private ParameterReferenceConstrainter _ParameterReferenceConstrainter = null;

		IOverrideConstraint GetParameterOverrideConstraint(Parameter.Type parameterType)
		{
			// clear override constraint
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
				var typeConstraint = (referenceType != null) ? new ClassConstraintInfo() { baseType = referenceType } : null;
				parameterOverrideConstraint.overrideConstraint = typeConstraint;
			}
		}

		void OnConstraintDestroyParamerter(Parameter.Type parameterType)
		{
			Disconnect(parameterType);

			IOverrideConstraint parameterOverrideConstraint = GetParameterOverrideConstraint(parameterType);
			if (parameterOverrideConstraint != null)
			{
				parameterOverrideConstraint.overrideConstraint = null;
			}
		}

		void Disconnect(Parameter.Type parameterType)
		{
			switch (parameterType)
			{
				case Parameter.Type.Int:
					_Function.Disconnect();
					_IntValue.Disconnect();
					break;
				case Parameter.Type.Long:
					_Function.Disconnect();
					_LongValue.Disconnect();
					break;
				case Parameter.Type.Float:
					_Function.Disconnect();
					_FloatValue.Disconnect();
					break;
				case Parameter.Type.Bool:
					_BoolValue.Disconnect();
					break;
				case Parameter.Type.String:
					_Function.Disconnect();
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
				case Parameter.Type.Vector4:
					_Vector4Value.Disconnect();
					break;
				case Parameter.Type.Vector2Int:
					_Vector2IntValue.Disconnect();
					break;
				case Parameter.Type.Vector3Int:
					_Vector3IntValue.Disconnect();
					break;
				case Parameter.Type.RectInt:
					_RectIntValue.Disconnect();
					break;
				case Parameter.Type.BoundsInt:
					_BoundsIntValue.Disconnect();
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

		protected override void OnPreDestroy()
		{
			base.OnPreDestroy();

			if (_ParameterReferenceConstrainter != null)
			{
				_ParameterReferenceConstrainter.Destroy();
				_ParameterReferenceConstrainter = null;
			}
		}
	}
}
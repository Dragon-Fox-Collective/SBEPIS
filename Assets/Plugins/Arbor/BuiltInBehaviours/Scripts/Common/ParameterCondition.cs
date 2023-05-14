//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor
{
	using Arbor.Extensions;

#if ARBOR_DOC_JA
	/// <summary>
	/// パラメータの状態チェッククラス
	/// </summary>
	/// <param name="Value">比較する値</param>
#else
	/// <summary>
	/// Condition check class of Parameter
	/// </summary>
	/// <param name="Value">Value to compare</param>
#endif
	[System.Serializable]
	[Arbor.Internal.Documentable]
	public sealed class ParameterCondition
	{
		#region Serialize fields

#if ARBOR_DOC_JA
		/// <summary>
		/// コンディションの論理演算
		/// </summary>
#else
		/// <summary>
		/// Logical operation of condition
		/// </summary>
#endif
		[SerializeField]
		internal LogicalCondition _LogicalCondition = new LogicalCondition();

#if ARBOR_DOC_JA
		/// <summary>
		/// パラメータの参照
		/// </summary>
#else
		/// <summary>
		/// Parameter reference
		/// </summary>
#endif
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

		[SerializeField]
		[Internal.HideInDocument]
		internal int _ParameterIndex = 0;

		#endregion // Serialize fields

		[System.NonSerialized]
		private ParameterConditionList _Owner = null;

		public ParameterConditionList owner
		{
			get
			{
				return _Owner;
			}
			internal set
			{
				_Owner = value;
			}
		}

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
				FlexibleInt intParameter = owner._IntParameters[_ParameterIndex];
				return intParameter.value;
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
				FlexibleLong longParameter = owner._LongParameters[_ParameterIndex];
				return longParameter.value;
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
				FlexibleFloat floatParameter = owner._FloatParameters[_ParameterIndex];
				return floatParameter.value;
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
				FlexibleBool boolParameter = owner._BoolParameters[_ParameterIndex];
				return boolParameter.value;
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
				FlexibleString stringParameter = owner._StringParameters[_ParameterIndex];
				return stringParameter.value;
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
				FlexibleEnumAny enumParameter = owner._EnumParameters[_ParameterIndex];
				return enumParameter.value;
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
				FlexibleGameObject gameObjectParameter = owner._GameObjectParameters[_ParameterIndex];
				return gameObjectParameter.value;
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
				FlexibleVector2 vector2Parameter = owner._Vector2Parameters[_ParameterIndex];
				return vector2Parameter.value;
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
				FlexibleVector3 vector3Parameter = owner._Vector3Parameters[_ParameterIndex];
				return vector3Parameter.value;
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
				FlexibleQuaternion quaternionParameter = owner._QuaternionParameters[_ParameterIndex];
				return quaternionParameter.value;
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
				FlexibleRect rectParameter = owner._RectParameters[_ParameterIndex];
				return rectParameter.value;
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
				FlexibleBounds boundsParameter = owner._BoundsParameters[_ParameterIndex];
				return boundsParameter.value;
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
				FlexibleColor colorParameter = owner._ColorParameters[_ParameterIndex];
				return colorParameter.value;
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 比較対象のVector4値
		/// </summary>
#else
		/// <summary>
		/// The Vector4 value to be compared
		/// </summary>
#endif
		public Vector4 vector4Value
		{
			get
			{
				FlexibleVector4 vector4Parameter = owner._Vector4Parameters[_ParameterIndex];
				return vector4Parameter.value;
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 比較対象のVector2Int値
		/// </summary>
#else
		/// <summary>
		/// The Vector2Int value to be compared
		/// </summary>
#endif
		public Vector2Int vector2IntValue
		{
			get
			{
				FlexibleVector2Int vector2IntParameter = owner._Vector2IntParameters[_ParameterIndex];
				return vector2IntParameter.value;
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 比較対象のVector3Int値
		/// </summary>
#else
		/// <summary>
		/// The Vector3Int value to be compared
		/// </summary>
#endif
		public Vector3Int vector3IntValue
		{
			get
			{
				FlexibleVector3Int vector3IntParameter = owner._Vector3IntParameters[_ParameterIndex];
				return vector3IntParameter.value;
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 比較対象のRectInt値
		/// </summary>
#else
		/// <summary>
		/// The RectInt value to be compared
		/// </summary>
#endif
		public RectInt rectIntValue
		{
			get
			{
				FlexibleRectInt rectIntParameter = owner._RectIntParameters[_ParameterIndex];
				return rectIntParameter.value;
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 比較対象のBoundsInt値
		/// </summary>
#else
		/// <summary>
		/// The BoundsInt value to be compared
		/// </summary>
#endif
		public BoundsInt boundsIntValue
		{
			get
			{
				FlexibleBoundsInt boundsIntParameter = owner._BoundsIntParameters[_ParameterIndex];
				return boundsIntParameter.value;
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
				return componentValue as Transform;
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
				return componentValue as RectTransform;
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
				return componentValue as Rigidbody;
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
				return componentValue as Rigidbody2D;
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
				FlexibleComponent componentParameter = owner._ComponentParameters[_ParameterIndex];
				return componentParameter.value;
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 比較対象のAssetObject値
		/// </summary>
#else
		/// <summary>
		/// The AssetObject value to be compared
		/// </summary>
#endif
		public Object assetObjectValue
		{
			get
			{
				FlexibleAssetObject assetObjectParameter = owner._AssetObjectParameters[_ParameterIndex];
				return assetObjectParameter.value;
			}
		}

		public ConditionResult conditionResult
		{
			get;
			internal set;
		}

		bool CheckConditionInternal()
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
						int intValue = this.intValue;
						return CompareUtility.Compare(parameter.intValue, intValue, _CompareType);
					}
				case Parameter.Type.Long:
					{
						long longValue = this.longValue;
						return CompareUtility.Compare(parameter.longValue, longValue, _CompareType);
					}
				case Parameter.Type.Float:
					{
						float floatValue = this.floatValue;
						return CompareUtility.Compare(parameter.floatValue, floatValue, _CompareType);
					}
				case Parameter.Type.Bool:
					{
						bool boolValue = this.boolValue;
						if (parameter.boolValue == boolValue)
						{
							return true;
						}
					}
					break;
				case Parameter.Type.String:
					{
						string stringValue = this.stringValue;
						return CompareUtility.Compare(parameter.stringValue, stringValue, _CompareType);
					}
				case Parameter.Type.Enum:
					{
						int enumValue = this.enumValue;
						if (parameter.enumIntValue == enumValue)
						{
							return true;
						}
					}
					break;
				case Parameter.Type.GameObject:
					{
						GameObject gameObjectValue = this.gameObjectValue;
						if (parameter.gameObjectValue == gameObjectValue)
						{
							return true;
						}
					}
					break;
				case Parameter.Type.Vector2:
					{
						Vector2 vector2Value = this.vector2Value;
						if (parameter.vector2Value == vector2Value)
						{
							return true;
						}
					}
					break;
				case Parameter.Type.Vector3:
					{
						Vector3 vector3Value = this.vector3Value;
						if (parameter.vector3Value == vector3Value)
						{
							return true;
						}
					}
					break;
				case Parameter.Type.Quaternion:
					{
						Quaternion quaternionValue = this.quaternionValue;
						if (parameter.quaternionValue == quaternionValue)
						{
							return true;
						}
					}
					break;
				case Parameter.Type.Rect:
					{
						Rect rectValue = this.rectValue;
						if (parameter.rectValue == rectValue)
						{
							return true;
						}
					}
					break;
				case Parameter.Type.Bounds:
					{
						Bounds boundsValue = this.boundsValue;
						if (parameter.boundsValue == boundsValue)
						{
							return true;
						}
					}
					break;
				case Parameter.Type.Color:
					{
						Color colorValue = this.colorValue;
						if (parameter.colorValue == colorValue)
						{
							return true;
						}
					}
					break;
				case Parameter.Type.Vector4:
					{
						Vector4 vector4Value = this.vector4Value;
						if (parameter.vector4Value == vector4Value)
						{
							return true;
						}
					}
					break;
				case Parameter.Type.Vector2Int:
					{
						Vector2Int vector2IntValue = this.vector2IntValue;
						if (parameter.vector2IntValue == vector2IntValue)
						{
							return true;
						}
					}
					break;
				case Parameter.Type.Vector3Int:
					{
						Vector3Int vector3IntValue = this.vector3IntValue;
						if (parameter.vector3IntValue == vector3IntValue)
						{
							return true;
						}
					}
					break;
				case Parameter.Type.RectInt:
					{
						RectInt rectIntValue = this.rectIntValue;
						if (parameter.rectIntValue.Equals(rectIntValue))
						{
							return true;
						}
					}
					break;
				case Parameter.Type.BoundsInt:
					{
						BoundsInt boundsIntValue = this.boundsIntValue;
						if (parameter.boundsIntValue == boundsIntValue)
						{
							return true;
						}
					}
					break;
				case Parameter.Type.Transform:
				case Parameter.Type.RectTransform:
				case Parameter.Type.Rigidbody:
				case Parameter.Type.Rigidbody2D:
				case Parameter.Type.Component:
					{
						Component componentValue = this.componentValue;
						if (parameter.componentValue == componentValue)
						{
							return true;
						}
					}
					break;
				case Parameter.Type.AssetObject:
					{
						Object assetObjectValue = this.assetObjectValue;
						if (parameter.assetObjectValue == assetObjectValue)
						{
							return true;
						}
					}
					break;
			}

			return false;
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
			bool result = CheckConditionInternal();

			if (_LogicalCondition.notOp)
			{
				result = !result;
			}

			conditionResult = result ? ConditionResult.Success : ConditionResult.Failure;

			return result;
		}

		[System.NonSerialized]
		private ParameterReferenceConstrainter _ParameterReferenceConstrainter = null;

		internal void Constraint()
		{
			if (_ParameterReferenceConstrainter == null)
			{
				_ParameterReferenceConstrainter = new ParameterReferenceConstrainter(OnConstraintChangedType, OnConstraintDestroyParameter);
			}

			_ParameterReferenceConstrainter.Constraint(_Reference, _ParameterType, _ReferenceType.type);
		}

		void OnConstraintChangedType(Parameter.Type parameterType, System.Type referenceType)
		{
			switch (parameterType)
			{
				case Parameter.Type.Transform:
					{
						referenceType = typeof(Transform);
					}
					break;
				case Parameter.Type.RectTransform:
					{
						referenceType = typeof(RectTransform);
					}
					break;
				case Parameter.Type.Rigidbody:
					{
						referenceType = typeof(Rigidbody);
					}
					break;
				case Parameter.Type.Rigidbody2D:
					{
						referenceType = typeof(Rigidbody2D);
					}
					break;
			}

			IOverrideConstraint parameterOverrideConstraint = GetParameterOverrideConstraint(parameterType);
			if (parameterOverrideConstraint != null)
			{
				var typeConstraint = referenceType != null ? new ClassConstraintInfo() { baseType = referenceType } : null;
				parameterOverrideConstraint.overrideConstraint = typeConstraint;
			}
		}

		IOverrideConstraint GetParameterOverrideConstraint(Parameter.Type parameterType)
		{
			switch (parameterType)
			{
				case Parameter.Type.Transform:
				case Parameter.Type.RectTransform:
				case Parameter.Type.Rigidbody:
				case Parameter.Type.Rigidbody2D:
				case Parameter.Type.Component:
					return owner._ComponentParameters[_ParameterIndex];
				case Parameter.Type.Enum:
					return owner._EnumParameters[_ParameterIndex];
				case Parameter.Type.AssetObject:
					return owner._AssetObjectParameters[_ParameterIndex];
			}

			return null;
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
					owner._IntParameters[_ParameterIndex].Disconnect();
					break;
				case Parameter.Type.Long:
					owner._LongParameters[_ParameterIndex].Disconnect();
					break;
				case Parameter.Type.Float:
					owner._FloatParameters[_ParameterIndex].Disconnect();
					break;
				case Parameter.Type.Bool:
					owner._BoolParameters[_ParameterIndex].Disconnect();
					break;
				case Parameter.Type.String:
					owner._StringParameters[_ParameterIndex].Disconnect();
					break;
				case Parameter.Type.Enum:
					owner._EnumParameters[_ParameterIndex].Disconnect();
					break;
				case Parameter.Type.GameObject:
					owner._GameObjectParameters[_ParameterIndex].Disconnect();
					break;
				case Parameter.Type.Vector2:
					owner._Vector2Parameters[_ParameterIndex].Disconnect();
					break;
				case Parameter.Type.Vector3:
					owner._Vector3Parameters[_ParameterIndex].Disconnect();
					break;
				case Parameter.Type.Quaternion:
					owner._QuaternionParameters[_ParameterIndex].Disconnect();
					break;
				case Parameter.Type.Rect:
					owner._RectParameters[_ParameterIndex].Disconnect();
					break;
				case Parameter.Type.Bounds:
					owner._BoundsParameters[_ParameterIndex].Disconnect();
					break;
				case Parameter.Type.Color:
					owner._ColorParameters[_ParameterIndex].Disconnect();
					break;
				case Parameter.Type.Vector4:
					owner._Vector4Parameters[_ParameterIndex].Disconnect();
					break;
				case Parameter.Type.Vector2Int:
					owner._Vector2IntParameters[_ParameterIndex].Disconnect();
					break;
				case Parameter.Type.Vector3Int:
					owner._Vector3IntParameters[_ParameterIndex].Disconnect();
					break;
				case Parameter.Type.RectInt:
					owner._RectIntParameters[_ParameterIndex].Disconnect();
					break;
				case Parameter.Type.BoundsInt:
					owner._BoundsIntParameters[_ParameterIndex].Disconnect();
					break;
				case Parameter.Type.Transform:
				case Parameter.Type.RectTransform:
				case Parameter.Type.Rigidbody:
				case Parameter.Type.Rigidbody2D:
				case Parameter.Type.Component:
					owner._ComponentParameters[_ParameterIndex].Disconnect();
					break;
				case Parameter.Type.AssetObject:
					owner._AssetObjectParameters[_ParameterIndex].Disconnect();
					break;
			}
		}
	}
}
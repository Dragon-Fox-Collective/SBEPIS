//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEditor;

namespace ArborEditor
{
	using Arbor;

	internal sealed class ParameterConditionLegacyProperty
	{
		private const string kReferencePath = "_Reference";
		private const string kParameterTypePath = "_ParameterType";
		private const string kReferenceTypePath = "_ReferenceType";
		private const string kCompareTypePath = "_CompareType";

		private const string kIntValuePath = "_IntValue";
		private const string kLongValuePath = "_LongValue";
		private const string kFloatValuePath = "_FloatValue";
		private const string kBoolValuePath = "_BoolValue";
		private const string kStringValuePath = "_StringValue";
		private const string kEnumValuePath = "_EnumValue";
		private const string kGameObjectValuePath = "_GameObjectValue";
		private const string kVector2ValuePath = "_Vector2Value";
		private const string kVector3ValuePath = "_Vector3Value";
		private const string kQuaternionValuePath = "_QuaternionValue";
		private const string kRectValuePath = "_RectValue";
		private const string kBoundsValuePath = "_BoundsValue";
		private const string kColorValuePath = "_ColorValue";
		private const string kTransformValuePath = "_TransformValue";
		private const string kRectTransformValuePath = "_RectTransformValue";
		private const string kRigidbodyValuePath = "_RigidbodyValue";
		private const string kRigidbody2DValuePath = "_Rigidbody2DValue";
		private const string kComponentValuePath = "_ComponentValue";
		private const string kAssetObjectValuePath = "_AssetObjectValue";

		private ParameterReferenceProperty _ReferenceProperty;
		private SerializedProperty _ParameterTypeProperty;
		private ClassTypeReferenceProperty _ReferenceTypeProperty;
		private SerializedProperty _CompareTypeProperty;

		private SerializedProperty _IntValueProperty;
		private SerializedProperty _LongValueProperty;
		private SerializedProperty _FloatValueProperty;
		private SerializedProperty _BoolValueProperty;
		private SerializedProperty _StringValueProperty;
		private SerializedProperty _EnumValueProperty;
		private SerializedProperty _GameObjectValueProperty;
		private SerializedProperty _Vector2ValueProperty;
		private SerializedProperty _Vector3ValueProperty;
		private SerializedProperty _QuaternionValueProperty;
		private SerializedProperty _RectValueProperty;
		private SerializedProperty _BoundsValueProperty;
		private SerializedProperty _ColorValueProperty;
		private SerializedProperty _TransformValueProperty;
		private SerializedProperty _RectTransformValueProperty;
		private SerializedProperty _RigidbodyValueProperty;
		private SerializedProperty _Rigidbody2DValueProperty;
		private FlexibleComponentProperty _ComponentValueProperty;
		private FlexibleFieldProperty _AssetObjectValueProperty;

		public SerializedProperty property
		{
			get;
			private set;
		}

		public ParameterReferenceProperty referenceProperty
		{
			get
			{
				if (_ReferenceProperty == null)
				{
					_ReferenceProperty = new ParameterReferenceProperty(property.FindPropertyRelative(kReferencePath));
				}
				return _ReferenceProperty;
			}
		}

		public SerializedProperty parameterTypeProperty
		{
			get
			{
				if (_ParameterTypeProperty == null)
				{
					_ParameterTypeProperty = property.FindPropertyRelative(kParameterTypePath);
				}
				return _ParameterTypeProperty;
			}
		}

		public Parameter.Type parameterType
		{
			get
			{
				return EnumUtility.GetValueFromIndex<Parameter.Type>(parameterTypeProperty.enumValueIndex);
			}
			set
			{
				parameterTypeProperty.enumValueIndex = EnumUtility.GetIndexFromValue<Parameter.Type>(value);
			}
		}

		public ClassTypeReferenceProperty referenceTypeProperty
		{
			get
			{
				if (_ReferenceTypeProperty == null)
				{
					_ReferenceTypeProperty = new ClassTypeReferenceProperty(property.FindPropertyRelative(kReferenceTypePath));
				}
				return _ReferenceTypeProperty;
			}
		}

		public System.Type referenceType
		{
			get
			{
				return referenceTypeProperty.type;
			}
			set
			{
				referenceTypeProperty.type = value;
			}
		}

		public SerializedProperty compareTypeProperty
		{
			get
			{
				if (_CompareTypeProperty == null)
				{
					_CompareTypeProperty = property.FindPropertyRelative(kCompareTypePath);
				}
				return _CompareTypeProperty;
			}
		}

		public SerializedProperty intValueProperty
		{
			get
			{
				if (_IntValueProperty == null)
				{
					_IntValueProperty = property.FindPropertyRelative(kIntValuePath);
				}
				return _IntValueProperty;
			}
		}

		public SerializedProperty longValueProperty
		{
			get
			{
				if (_LongValueProperty == null)
				{
					_LongValueProperty = property.FindPropertyRelative(kLongValuePath);
				}
				return _LongValueProperty;
			}
		}

		public SerializedProperty floatValueProperty
		{
			get
			{
				if (_FloatValueProperty == null)
				{
					_FloatValueProperty = property.FindPropertyRelative(kFloatValuePath);
				}
				return _FloatValueProperty;
			}
		}

		public SerializedProperty boolValueProperty
		{
			get
			{
				if (_BoolValueProperty == null)
				{
					_BoolValueProperty = property.FindPropertyRelative(kBoolValuePath);
				}
				return _BoolValueProperty;
			}
		}

		public SerializedProperty stringValueProperty
		{
			get
			{
				if (_StringValueProperty == null)
				{
					_StringValueProperty = property.FindPropertyRelative(kStringValuePath);
				}
				return _StringValueProperty;
			}
		}

		public SerializedProperty enumValueProperty
		{
			get
			{
				if (_EnumValueProperty == null)
				{
					_EnumValueProperty = property.FindPropertyRelative(kEnumValuePath);
				}
				return _EnumValueProperty;
			}
		}

		public SerializedProperty gameObjectValueProperty
		{
			get
			{
				if (_GameObjectValueProperty == null)
				{
					_GameObjectValueProperty = property.FindPropertyRelative(kGameObjectValuePath);
				}
				return _GameObjectValueProperty;
			}
		}

		public SerializedProperty vector2ValueProperty
		{
			get
			{
				if (_Vector2ValueProperty == null)
				{
					_Vector2ValueProperty = property.FindPropertyRelative(kVector2ValuePath);
				}
				return _Vector2ValueProperty;
			}
		}

		public SerializedProperty vector3ValueProperty
		{
			get
			{
				if (_Vector3ValueProperty == null)
				{
					_Vector3ValueProperty = property.FindPropertyRelative(kVector3ValuePath);
				}
				return _Vector3ValueProperty;
			}
		}

		public SerializedProperty quaternionValueProperty
		{
			get
			{
				if (_QuaternionValueProperty == null)
				{
					_QuaternionValueProperty = property.FindPropertyRelative(kQuaternionValuePath);
				}
				return _QuaternionValueProperty;
			}
		}

		public SerializedProperty rectValueProperty
		{
			get
			{
				if (_RectValueProperty == null)
				{
					_RectValueProperty = property.FindPropertyRelative(kRectValuePath);
				}
				return _RectValueProperty;
			}
		}

		public SerializedProperty boundsValueProperty
		{
			get
			{
				if (_BoundsValueProperty == null)
				{
					_BoundsValueProperty = property.FindPropertyRelative(kBoundsValuePath);
				}
				return _BoundsValueProperty;
			}
		}

		public SerializedProperty colorValueProperty
		{
			get
			{
				if (_ColorValueProperty == null)
				{
					_ColorValueProperty = property.FindPropertyRelative(kColorValuePath);
				}
				return _ColorValueProperty;
			}
		}

		public SerializedProperty transformValueProperty
		{
			get
			{
				if (_TransformValueProperty == null)
				{
					_TransformValueProperty = property.FindPropertyRelative(kTransformValuePath);
				}
				return _TransformValueProperty;
			}
		}

		public SerializedProperty rectTransformValueProperty
		{
			get
			{
				if (_RectTransformValueProperty == null)
				{
					_RectTransformValueProperty = property.FindPropertyRelative(kRectTransformValuePath);
				}
				return _RectTransformValueProperty;
			}
		}

		public SerializedProperty rigidbodyValueProperty
		{
			get
			{
				if (_RigidbodyValueProperty == null)
				{
					_RigidbodyValueProperty = property.FindPropertyRelative(kRigidbodyValuePath);
				}
				return _RigidbodyValueProperty;
			}
		}

		public SerializedProperty rigidbody2DValueProperty
		{
			get
			{
				if (_Rigidbody2DValueProperty == null)
				{
					_Rigidbody2DValueProperty = property.FindPropertyRelative(kRigidbody2DValuePath);
				}
				return _Rigidbody2DValueProperty;
			}
		}

		public FlexibleComponentProperty componentValueProperty
		{
			get
			{
				if (_ComponentValueProperty == null)
				{
					_ComponentValueProperty = new FlexibleComponentProperty(property.FindPropertyRelative(kComponentValuePath));
				}
				return _ComponentValueProperty;
			}
		}

		public FlexibleFieldProperty assetObjectValueProperty
		{
			get
			{
				if (_AssetObjectValueProperty == null)
				{
					_AssetObjectValueProperty = new FlexibleFieldProperty(property.FindPropertyRelative(kAssetObjectValuePath));
				}
				return _AssetObjectValueProperty;
			}
		}

		public ParameterConditionLegacyProperty(SerializedProperty property)
		{
			this.property = property;
		}

		public Parameter.Type GetParameterType()
		{
			ParameterReferenceType parameterReferenceType = referenceProperty.type;
			Parameter.Type parameterType = Parameter.Type.Int;
			switch (parameterReferenceType)
			{
				case ParameterReferenceType.Constant:
					{
						Parameter parameter = referenceProperty.GetParameter();
						if (parameter != null)
						{
							parameterType = parameter.type;
						}
					}
					break;
				case ParameterReferenceType.DataSlot:
					{
						parameterType = this.parameterType;
					}
					break;
			}

			return parameterType;
		}

		public System.Type GetReferenceType()
		{
			ParameterReferenceType parameterReferenceType = referenceProperty.type;
			System.Type referenceType = null;
			switch (parameterReferenceType)
			{
				case ParameterReferenceType.Constant:
					{
						Parameter parameter = referenceProperty.GetParameter();
						if (parameter != null)
						{
							referenceType = parameter.referenceType;
						}
					}
					break;
				case ParameterReferenceType.DataSlot:
					{
						referenceType = this.referenceType;
					}
					break;
			}

			return referenceType;
		}
	}
}
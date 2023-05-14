//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEditor;

namespace ArborEditor.Events
{
	using Arbor.Events;

	public sealed class ParameterListProperty
	{
		private const string kIntParametersPath = "_IntParameters";
		private const string kLongParametersPath = "_LongParameters";
		private const string kFloatParametersPath = "_FloatParameters";
		private const string kBoolParametersPath = "_BoolParameters";
		private const string kStringParametersPath = "_StringParameters";
		private const string kVector2ParametersPath = "_Vector2Parameters";
		private const string kVector3ParametersPath = "_Vector3Parameters";
		private const string kQuaternionParametersPath = "_QuaternionParameters";
		private const string kRectParametersPath = "_RectParameters";
		private const string kBoundsParametersPath = "_BoundsParameters";
		private const string kColorParametersPath = "_ColorParameters";
		private const string kVector4ParametersPath = "_Vector4Parameters";
		private const string kVector2IntParametersPath = "_Vector2IntParameters";
		private const string kVector3IntParametersPath = "_Vector3IntParameters";
		private const string kRectIntParametersPath = "_RectIntParameters";
		private const string kBoundsIntParametersPath = "_BoundsIntParameters";
		private const string kGameObjectParametersPath = "_GameObjectParameters";
		private const string kComponentParametersPath = "_ComponentParameters";
		private const string kEnumParametersPath = "_EnumParameters";
		private const string kAssetObjectParametersPath = "_AssetObjectParameters";
		private const string kInputSlotParametersPath = "_InputSlotParameters";

		private SerializedProperty _IntParameters;
		private SerializedProperty _LongParameters;
		private SerializedProperty _FloatParameters;
		private SerializedProperty _BoolParameters;
		private SerializedProperty _StringParameters;
		private SerializedProperty _Vector2Parameters;
		private SerializedProperty _Vector3Parameters;
		private SerializedProperty _QuaternionParameters;
		private SerializedProperty _RectParameters;
		private SerializedProperty _BoundsParameters;
		private SerializedProperty _ColorParameters;
		private SerializedProperty _Vector4Parameters;
		private SerializedProperty _Vector2IntParameters;
		private SerializedProperty _Vector3IntParameters;
		private SerializedProperty _RectIntParameters;
		private SerializedProperty _BoundsIntParameters;
		private SerializedProperty _GameObjectParameters;
		private SerializedProperty _ComponentParameters;
		private SerializedProperty _EnumParameters;
		private SerializedProperty _AssetObjectParameters;
		private SerializedProperty _InputSlotParameters;

		public SerializedProperty property
		{
			get;
			private set;
		}

		public SerializedProperty intParametersProperty
		{
			get
			{
				if (_IntParameters == null)
				{
					_IntParameters = property.FindPropertyRelative(kIntParametersPath);
				}
				return _IntParameters;
			}
		}

		public SerializedProperty longParametersProperty
		{
			get
			{
				if (_LongParameters == null)
				{
					_LongParameters = property.FindPropertyRelative(kLongParametersPath);
				}
				return _LongParameters;
			}
		}

		public SerializedProperty floatParametersProperty
		{
			get
			{
				if (_FloatParameters == null)
				{
					_FloatParameters = property.FindPropertyRelative(kFloatParametersPath);
				}
				return _FloatParameters;
			}
		}

		public SerializedProperty boolParametersProperty
		{
			get
			{
				if (_BoolParameters == null)
				{
					_BoolParameters = property.FindPropertyRelative(kBoolParametersPath);
				}
				return _BoolParameters;
			}
		}

		public SerializedProperty stringParametersProperty
		{
			get
			{
				if (_StringParameters == null)
				{
					_StringParameters = property.FindPropertyRelative(kStringParametersPath);
				}
				return _StringParameters;
			}
		}

		public SerializedProperty vector2ParametersProperty
		{
			get
			{
				if (_Vector2Parameters == null)
				{
					_Vector2Parameters = property.FindPropertyRelative(kVector2ParametersPath);
				}
				return _Vector2Parameters;
			}
		}

		public SerializedProperty vector3ParametersProperty
		{
			get
			{
				if (_Vector3Parameters == null)
				{
					_Vector3Parameters = property.FindPropertyRelative(kVector3ParametersPath);
				}
				return _Vector3Parameters;
			}
		}

		public SerializedProperty quaternionParametersProperty
		{
			get
			{
				if (_QuaternionParameters == null)
				{
					_QuaternionParameters = property.FindPropertyRelative(kQuaternionParametersPath);
				}
				return _QuaternionParameters;
			}
		}

		public SerializedProperty rectParametersProperty
		{
			get
			{
				if (_RectParameters == null)
				{
					_RectParameters = property.FindPropertyRelative(kRectParametersPath);
				}
				return _RectParameters;
			}
		}

		public SerializedProperty boundsParametersProperty
		{
			get
			{
				if (_BoundsParameters == null)
				{
					_BoundsParameters = property.FindPropertyRelative(kBoundsParametersPath);
				}
				return _BoundsParameters;
			}
		}

		public SerializedProperty colorParametersProperty
		{
			get
			{
				if (_ColorParameters == null)
				{
					_ColorParameters = property.FindPropertyRelative(kColorParametersPath);
				}
				return _ColorParameters;
			}
		}

		public SerializedProperty vector4ParametersProperty
		{
			get
			{
				if (_Vector4Parameters == null)
				{
					_Vector4Parameters = property.FindPropertyRelative(kVector4ParametersPath);
				}
				return _Vector4Parameters;
			}
		}

		public SerializedProperty vector2IntParametersProperty
		{
			get
			{
				if (_Vector2IntParameters == null)
				{
					_Vector2IntParameters = property.FindPropertyRelative(kVector2IntParametersPath);
				}
				return _Vector2IntParameters;
			}
		}

		public SerializedProperty vector3IntParametersProperty
		{
			get
			{
				if (_Vector3IntParameters == null)
				{
					_Vector3IntParameters = property.FindPropertyRelative(kVector3IntParametersPath);
				}
				return _Vector3IntParameters;
			}
		}

		public SerializedProperty rectIntParametersProperty
		{
			get
			{
				if (_RectIntParameters == null)
				{
					_RectIntParameters = property.FindPropertyRelative(kRectIntParametersPath);
				}
				return _RectIntParameters;
			}
		}

		public SerializedProperty boundsIntParametersProperty
		{
			get
			{
				if (_BoundsIntParameters == null)
				{
					_BoundsIntParameters = property.FindPropertyRelative(kBoundsIntParametersPath);
				}
				return _BoundsIntParameters;
			}
		}

		public SerializedProperty gameObjectParametersProperty
		{
			get
			{
				if (_GameObjectParameters == null)
				{
					_GameObjectParameters = property.FindPropertyRelative(kGameObjectParametersPath);
				}
				return _GameObjectParameters;
			}
		}

		public SerializedProperty componentParametersProperty
		{
			get
			{
				if (_ComponentParameters == null)
				{
					_ComponentParameters = property.FindPropertyRelative(kComponentParametersPath);
				}
				return _ComponentParameters;
			}
		}

		public SerializedProperty enumParametersProperty
		{
			get
			{
				if (_EnumParameters == null)
				{
					_EnumParameters = property.FindPropertyRelative(kEnumParametersPath);
				}
				return _EnumParameters;
			}
		}

		public SerializedProperty assetObjectParametersProperty
		{
			get
			{
				if (_AssetObjectParameters == null)
				{
					_AssetObjectParameters = property.FindPropertyRelative(kAssetObjectParametersPath);
				}
				return _AssetObjectParameters;
			}
		}

		public SerializedProperty inputSlotParametersProperty
		{
			get
			{
				if (_InputSlotParameters == null)
				{
					_InputSlotParameters = property.FindPropertyRelative(kInputSlotParametersPath);
				}
				return _InputSlotParameters;
			}
		}

		public ParameterListProperty(SerializedProperty property)
		{
			this.property = property;
		}

		public SerializedProperty GetParametersProperty(ParameterType parameterType)
		{
			switch (parameterType)
			{
				case ParameterType.Int:
					return intParametersProperty;
				case ParameterType.Long:
					return longParametersProperty;
				case ParameterType.Float:
					return floatParametersProperty;
				case ParameterType.Bool:
					return boolParametersProperty;
				case ParameterType.String:
					return stringParametersProperty;
				case ParameterType.Vector2:
					return vector2ParametersProperty;
				case ParameterType.Vector3:
					return vector3ParametersProperty;
				case ParameterType.Quaternion:
					return quaternionParametersProperty;
				case ParameterType.Rect:
					return rectParametersProperty;
				case ParameterType.Bounds:
					return boundsParametersProperty;
				case ParameterType.Color:
					return colorParametersProperty;
				case ParameterType.Vector4:
					return vector4ParametersProperty;
				case ParameterType.Vector2Int:
					return vector2IntParametersProperty;
				case ParameterType.Vector3Int:
					return vector3IntParametersProperty;
				case ParameterType.RectInt:
					return rectIntParametersProperty;
				case ParameterType.BoundsInt:
					return boundsIntParametersProperty;
				case ParameterType.GameObject:
					return gameObjectParametersProperty;
				case ParameterType.Component:
					return componentParametersProperty;
				case ParameterType.Enum:
					return enumParametersProperty;
				case ParameterType.AssetObject:
					return assetObjectParametersProperty;
				case ParameterType.Slot:
					return inputSlotParametersProperty;
			}

			return null;
		}

		public SerializedProperty GetValueProperty(ParameterType parameterType, int index)
		{
			SerializedProperty parametersProperty = GetParametersProperty(parameterType);
			if (parametersProperty == null)
			{
				return null;
			}

			if (index < 0 || parametersProperty.arraySize <= index)
			{
				return null;
			}

			return parametersProperty.GetArrayElementAtIndex(index);
		}

		public void DisconnectParameterSlots(ParameterType parameterType)
		{
			if (parameterType == ParameterType.Unknown)
			{
				return;
			}
			SerializedProperty parametersProperty = GetParametersProperty(parameterType);
			for (int i = 0; i < parametersProperty.arraySize; i++)
			{
				SerializedProperty property = parametersProperty.GetArrayElementAtIndex(i);
				if (parameterType == ParameterType.Slot)
				{
					InputSlotTypableProperty inputSlotProperty = new InputSlotTypableProperty(property);
					inputSlotProperty.Disconnect();
				}
				else
				{
					FlexibleFieldPropertyBase flexibleProperty = GetFlexibleFieldProperty(parameterType, property);
					flexibleProperty.Disconnect();
				}
			}
		}

		public void DisconnectSlots()
		{
			var parameterTypes = EnumUtility.GetValues<ParameterType>();
			for (int i = 0; i < parameterTypes.Length; i++)
			{
				ParameterType parameterType = parameterTypes[i];
				DisconnectParameterSlots(parameterType);
			}
		}

		public void ClearArray()
		{
			var parameterTypes = EnumUtility.GetValues<ParameterType>();
			for (int i = 0; i < parameterTypes.Length; i++)
			{
				ParameterType parameterType = parameterTypes[i];
				if (parameterType == ParameterType.Unknown)
				{
					continue;
				}
				SerializedProperty parametersProperty = GetParametersProperty(parameterType);
				parametersProperty.ClearArray();
			}
		}

		public static FlexibleFieldPropertyBase GetFlexibleFieldProperty(ParameterType parameterType, SerializedProperty property)
		{
			switch (parameterType)
			{
				case ParameterType.Bool:
				case ParameterType.Int:
				case ParameterType.Long:
				case ParameterType.Float:
					return new FlexiblePrimitiveProperty(property);
				case ParameterType.Component:
				case ParameterType.GameObject:
					return new FlexibleSceneObjectProperty(property);
				default:
					return new FlexibleFieldProperty(property);
			}
		}
	}
}
//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using UnityEditor;

namespace ArborEditor
{
	using Arbor;

	internal sealed class SendMessageArgumentProperty
	{
		private const string kTypePath = "_Type";

		private const string kIntValuePath = "_IntValue";
		private const string kFloatValuePath = "_FloatValue";
		private const string kBoolValuePath = "_BoolValue";
		private const string kStringValuePath = "_StringValue";
		private const string kLongValuePath = "_LongValue";
		private const string kEnumValuePath = "_EnumValue";
		private const string kGameObjectValuePath = "_GameObjectValue";
		private const string kVector2ValuePath = "_Vector2Value";
		private const string kVector3ValuePath = "_Vector3Value";
		private const string kQuaternionValuePath = "_QuaternionValue";
		private const string kRectValuePath = "_RectValue";
		private const string kBoundsValuePath = "_BoundsValue";
		private const string kColorValuePath = "_ColorValue";
		private const string kComponentValuePath = "_ComponentValue";
		private const string kInputSlotValuePath = "_InputSlotValue";

		private const string kReferenceTypePath = "_ReferenceType";

		private SerializedProperty _TypeProperty;

		private FlexibleNumericProperty _IntValueProperty;
		private FlexibleNumericProperty _FloatValueProperty;
		private FlexibleBoolProperty _BoolValueProperty;
		private FlexibleFieldProperty _StringValueProperty;
		private FlexibleNumericProperty _LongValueProperty;
		private FlexibleFieldProperty _EnumValueProperty;
		private FlexibleFieldProperty _GameObjectValueProperty;
		private FlexibleFieldProperty _Vector2ValueProperty;
		private FlexibleFieldProperty _Vector3ValueProperty;
		private FlexibleFieldProperty _QuaternionValueProperty;
		private FlexibleFieldProperty _RectValueProperty;
		private FlexibleFieldProperty _BoundsValueProperty;
		private FlexibleFieldProperty _ColorValueProperty;
		private FlexibleFieldProperty _ComponentValueProperty;
		private InputSlotTypableProperty _InputSlotValueProperty;

		private ClassTypeReferenceProperty _ReferenceTypeProperty;

		public SerializedProperty property
		{
			get;
			private set;
		}

		public SerializedProperty typeProperty
		{
			get
			{
				if (_TypeProperty == null)
				{
					_TypeProperty = property.FindPropertyRelative(kTypePath);
				}
				return _TypeProperty;
			}
		}

		public SendMessageArgument.Type type
		{
			get
			{
				return EnumUtility.GetValueFromIndex<SendMessageArgument.Type>(typeProperty.enumValueIndex);
			}
			set
			{
				typeProperty.enumValueIndex = EnumUtility.GetIndexFromValue<SendMessageArgument.Type>(value);
			}
		}

		public FlexibleNumericProperty intValueProperty
		{
			get
			{
				if (_IntValueProperty == null)
				{
					_IntValueProperty = new FlexibleNumericProperty(property.FindPropertyRelative(kIntValuePath));
				}
				return _IntValueProperty;
			}
		}

		public FlexibleNumericProperty floatValueProperty
		{
			get
			{
				if (_FloatValueProperty == null)
				{
					_FloatValueProperty = new FlexibleNumericProperty(property.FindPropertyRelative(kFloatValuePath));
				}
				return _FloatValueProperty;
			}
		}

		public FlexibleBoolProperty boolValueProperty
		{
			get
			{
				if (_BoolValueProperty == null)
				{
					_BoolValueProperty = new FlexibleBoolProperty(property.FindPropertyRelative(kBoolValuePath));
				}
				return _BoolValueProperty;
			}
		}

		public FlexibleFieldProperty stringValueProperty
		{
			get
			{
				if (_StringValueProperty == null)
				{
					_StringValueProperty = new FlexibleFieldProperty(property.FindPropertyRelative(kStringValuePath));
				}
				return _StringValueProperty;
			}
		}

		public FlexibleNumericProperty longValueProperty
		{
			get
			{
				if (_LongValueProperty == null)
				{
					_LongValueProperty = new FlexibleNumericProperty(property.FindPropertyRelative(kLongValuePath));
				}
				return _LongValueProperty;
			}
		}

		public FlexibleFieldProperty enumValueProperty
		{
			get
			{
				if (_EnumValueProperty == null)
				{
					_EnumValueProperty = new FlexibleFieldProperty(property.FindPropertyRelative(kEnumValuePath));
				}
				return _EnumValueProperty;
			}
		}

		public FlexibleFieldProperty gameObjectValueProperty
		{
			get
			{
				if (_GameObjectValueProperty == null)
				{
					_GameObjectValueProperty = new FlexibleFieldProperty(property.FindPropertyRelative(kGameObjectValuePath));
				}
				return _GameObjectValueProperty;
			}
		}

		public FlexibleFieldProperty vector2ValueProperty
		{
			get
			{
				if (_Vector2ValueProperty == null)
				{
					_Vector2ValueProperty = new FlexibleFieldProperty(property.FindPropertyRelative(kVector2ValuePath));
				}
				return _Vector2ValueProperty;
			}
		}

		public FlexibleFieldProperty vector3ValueProperty
		{
			get
			{
				if (_Vector3ValueProperty == null)
				{
					_Vector3ValueProperty = new FlexibleFieldProperty(property.FindPropertyRelative(kVector3ValuePath));
				}
				return _Vector3ValueProperty;
			}
		}

		public FlexibleFieldProperty quaternionValueProperty
		{
			get
			{
				if (_QuaternionValueProperty == null)
				{
					_QuaternionValueProperty = new FlexibleFieldProperty(property.FindPropertyRelative(kQuaternionValuePath));
				}
				return _QuaternionValueProperty;
			}
		}

		public FlexibleFieldProperty rectValueProperty
		{
			get
			{
				if (_RectValueProperty == null)
				{
					_RectValueProperty = new FlexibleFieldProperty(property.FindPropertyRelative(kRectValuePath));
				}
				return _RectValueProperty;
			}
		}

		public FlexibleFieldProperty boundsValueProperty
		{
			get
			{
				if (_BoundsValueProperty == null)
				{
					_BoundsValueProperty = new FlexibleFieldProperty(property.FindPropertyRelative(kBoundsValuePath));
				}
				return _BoundsValueProperty;
			}
		}

		public FlexibleFieldProperty colorValueProperty
		{
			get
			{
				if (_ColorValueProperty == null)
				{
					_ColorValueProperty = new FlexibleFieldProperty(property.FindPropertyRelative(kColorValuePath));
				}
				return _ColorValueProperty;
			}
		}

		public FlexibleFieldProperty componentValueProperty
		{
			get
			{
				if (_ComponentValueProperty == null)
				{
					_ComponentValueProperty = new FlexibleFieldProperty(property.FindPropertyRelative(kComponentValuePath));
				}
				return _ComponentValueProperty;
			}
		}

		public InputSlotTypableProperty inputSlotValueProperty
		{
			get
			{
				if (_InputSlotValueProperty == null)
				{
					_InputSlotValueProperty = new InputSlotTypableProperty(property.FindPropertyRelative(kInputSlotValuePath));
				}
				return _InputSlotValueProperty;
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

		public SendMessageArgumentProperty(SerializedProperty property)
		{
			this.property = property;
		}

		private FlexibleFieldPropertyBase GetFlexibleProperty(SendMessageArgument.Type type)
		{
			switch (type)
			{
				case SendMessageArgument.Type.None:
					return null;
				case SendMessageArgument.Type.Int:
					return intValueProperty;
				case SendMessageArgument.Type.Float:
					return floatValueProperty;
				case SendMessageArgument.Type.Bool:
					return boolValueProperty;
				case SendMessageArgument.Type.String:
					return stringValueProperty;
				case SendMessageArgument.Type.Long:
					return longValueProperty;
				case SendMessageArgument.Type.Enum:
					return enumValueProperty;
				case SendMessageArgument.Type.GameObject:
					return gameObjectValueProperty;
				case SendMessageArgument.Type.Vector2:
					return vector2ValueProperty;
				case SendMessageArgument.Type.Vector3:
					return vector3ValueProperty;
				case SendMessageArgument.Type.Quaternion:
					return quaternionValueProperty;
				case SendMessageArgument.Type.Rect:
					return rectValueProperty;
				case SendMessageArgument.Type.Bounds:
					return boundsValueProperty;
				case SendMessageArgument.Type.Color:
					return colorValueProperty;
				case SendMessageArgument.Type.Component:
					return componentValueProperty;
			}

			return null;
		}

		public SerializedProperty GetValueProperty(SendMessageArgument.Type type)
		{
			if (type == SendMessageArgument.Type.Slot)
			{
				return inputSlotValueProperty.property;
			}

			FlexibleFieldPropertyBase flexibleProperty = GetFlexibleProperty(type);
			if (flexibleProperty != null)
			{
				return flexibleProperty.property;
			}

			return null;
		}

		public void Disconnect(SendMessageArgument.Type type)
		{
			if (type == SendMessageArgument.Type.Slot)
			{
				inputSlotValueProperty.Disconnect();
				return;
			}

			FlexibleFieldPropertyBase flexibleProperty = GetFlexibleProperty(type);
			if (flexibleProperty != null)
			{
				flexibleProperty.Disconnect();
			}
		}
	}

	internal sealed class SendMessageArgumentPropertyEditor : PropertyEditor
	{
		private SendMessageArgumentProperty _ArgumentProperty;
		private LayoutArea _LayoutArea = new LayoutArea();

		protected override void OnInitialize()
		{
			base.OnInitialize();

			_ArgumentProperty = new SendMessageArgumentProperty(property);
		}

		void DoGUI(GUIContent label)
		{
			EditorGUI.BeginProperty(_LayoutArea.rect, label, property);

			int indentLevel = EditorGUI.indentLevel;
			if (label != GUIContent.none)
			{
				_LayoutArea.LabelField(label);
				EditorGUI.indentLevel++;
			}

			SendMessageArgument.Type argumentType = _ArgumentProperty.type;

			EditorGUI.BeginChangeCheck();
			_LayoutArea.PropertyField(_ArgumentProperty.typeProperty);
			if (EditorGUI.EndChangeCheck())
			{
				SendMessageArgument.Type newArgumentType = _ArgumentProperty.type;

				if (argumentType != newArgumentType)
				{
					if (newArgumentType == SendMessageArgument.Type.Enum ||
						newArgumentType == SendMessageArgument.Type.Component)
					{
						_ArgumentProperty.referenceTypeProperty.Clear();
					}
					else if (newArgumentType == SendMessageArgument.Type.Slot)
					{
						_ArgumentProperty.inputSlotValueProperty.Clear();
					}

					_ArgumentProperty.Disconnect(argumentType);

					property.serializedObject.ApplyModifiedProperties();

					GUIUtility.ExitGUI();
				}
			}

			SerializedProperty valueProperty = _ArgumentProperty.GetValueProperty(argumentType);

			if (valueProperty != null)
			{
				ClassTypeReferenceProperty referenceTypeProperty = null;
				ClassTypeConstraintAttribute referenceTypeConstraint = null;
				GUIContent referenceTypeLabel = null;
				switch (argumentType)
				{
					case SendMessageArgument.Type.Enum:
						{
							referenceTypeProperty = _ArgumentProperty.referenceTypeProperty;
							referenceTypeConstraint = ClassTypeConstraintEditorUtility.enumField;
						}
						break;
					case SendMessageArgument.Type.Component:
						{
							referenceTypeProperty = _ArgumentProperty.referenceTypeProperty;
							referenceTypeConstraint = ClassTypeConstraintEditorUtility.component;
						}
						break;
					case SendMessageArgument.Type.Slot:
						{
							referenceTypeProperty = _ArgumentProperty.inputSlotValueProperty.typeProperty;
							referenceTypeLabel = GUIContentCaches.Get("Reference Type");
						}
						break;
				}

				if (referenceTypeProperty != null)
				{
					if (referenceTypeConstraint != null)
					{
						referenceTypeProperty.SetConstraint(referenceTypeConstraint);
					}

					var oldReferenceType = referenceTypeProperty.type;
					EditorGUI.BeginChangeCheck();
					if (referenceTypeLabel != null)
					{
						_LayoutArea.PropertyField(referenceTypeProperty.property, referenceTypeLabel);
					}
					else
					{
						_LayoutArea.PropertyField(referenceTypeProperty.property);
					}
					if (EditorGUI.EndChangeCheck())
					{
						var newReferenceType = referenceTypeProperty.type;
						if (oldReferenceType != newReferenceType)
						{
							_ArgumentProperty.Disconnect(argumentType);

							property.serializedObject.ApplyModifiedProperties();

							GUIUtility.ExitGUI();
						}
					}
				}

				_LayoutArea.PropertyField(valueProperty);
			}

			EditorGUI.indentLevel = indentLevel;

			EditorGUI.EndProperty();
		}

		static readonly RectOffset s_LayoutMargin = new RectOffset(0, 0, 0, 2);

		protected override void OnGUI(Rect position, GUIContent label)
		{
			_LayoutArea.Begin(position, false, s_LayoutMargin);

			DoGUI(label);

			_LayoutArea.End();
		}

		protected override float GetHeight(GUIContent label)
		{
			_LayoutArea.Begin(new Rect(), true, s_LayoutMargin);

			DoGUI(label);

			_LayoutArea.End();

			return _LayoutArea.rect.height - EditorGUIUtility.standardVerticalSpacing;
		}
	}

	[CustomPropertyDrawer(typeof(SendMessageArgument))]
	internal sealed class SendMessageArgumentPropertyDrawer : PropertyEditorDrawer<SendMessageArgumentPropertyEditor>
	{
	}
}
//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEditor;

namespace ArborEditor.Events
{
	using Arbor.Events;

	internal sealed class ArgumentProperty
	{
		private const string kTypePath = "_Type";
		private const string kNamePath = "_Name";
		private const string kAttributesPath = "_Attributes";
		private const string kParameterTypePath = "_ParameterType";
		private const string kParameterIndexPath = "_ParameterIndex";
		private const string kOutputSlotIndexPath = "_OutputSlotIndex";

		private ClassTypeReferenceProperty _TypeProperty;
		private SerializedProperty _NameProperty;
		private SerializedProperty _AttributesProperty;
		private SerializedProperty _ParameterTypeProperty;
		private SerializedProperty _ParameterIndexProperty;
		private SerializedProperty _OutputSlotIndexProperty;

		public SerializedProperty property
		{
			get;
			private set;
		}

		public ClassTypeReferenceProperty typeProperty
		{
			get
			{
				if (_TypeProperty == null)
				{
					_TypeProperty = new ClassTypeReferenceProperty(property.FindPropertyRelative(kTypePath));
				}
				return _TypeProperty;
			}
		}

		public System.Type type
		{
			get
			{
				return typeProperty.type;
			}
			set
			{
				typeProperty.type = value;
			}
		}

		public SerializedProperty nameProperty
		{
			get
			{
				if (_NameProperty == null)
				{
					_NameProperty = property.FindPropertyRelative(kNamePath);
				}
				return _NameProperty;
			}
		}

		public string name
		{
			get
			{
				return nameProperty.stringValue;
			}
			set
			{
				nameProperty.stringValue = value;
			}
		}

		public SerializedProperty attributesProperty
		{
			get
			{
				if (_AttributesProperty == null)
				{
					_AttributesProperty = property.FindPropertyRelative(kAttributesPath);
				}
				return _AttributesProperty;
			}
		}

		public ArgumentAttributes attributes
		{
			get
			{
				return (ArgumentAttributes)attributesProperty.intValue;
			}
			set
			{
				attributesProperty.intValue = (int)value;
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

		public ParameterType parameterType
		{
			get
			{
				return EnumUtility.GetValueFromIndex<ParameterType>(parameterTypeProperty.enumValueIndex);
			}
			set
			{
				parameterTypeProperty.enumValueIndex = EnumUtility.GetIndexFromValue<ParameterType>(value);
			}
		}

		public SerializedProperty parameterIndexProperty
		{
			get
			{
				if (_ParameterIndexProperty == null)
				{
					_ParameterIndexProperty = property.FindPropertyRelative(kParameterIndexPath);
				}
				return _ParameterIndexProperty;
			}
		}

		public int parameterIndex
		{
			get
			{
				return parameterIndexProperty.intValue;
			}
			set
			{
				parameterIndexProperty.intValue = value;
			}
		}

		public SerializedProperty outputSlotIndexProperty
		{
			get
			{
				if (_OutputSlotIndexProperty == null)
				{
					_OutputSlotIndexProperty = property.FindPropertyRelative(kOutputSlotIndexPath);
				}
				return _OutputSlotIndexProperty;
			}
		}

		public int outputSlotIndex
		{
			get
			{
				return outputSlotIndexProperty.intValue;
			}
			set
			{
				outputSlotIndexProperty.intValue = value;
			}
		}

		public ArgumentProperty(SerializedProperty property)
		{
			this.property = property;
		}
	}
}
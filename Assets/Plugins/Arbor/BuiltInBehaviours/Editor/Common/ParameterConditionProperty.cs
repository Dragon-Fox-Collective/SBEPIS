//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEditor;

namespace ArborEditor
{
	using Arbor;

	internal sealed class ParameterConditionProperty
	{
		private const string kReferencePath = "_Reference";
		private const string kParameterTypePath = "_ParameterType";
		private const string kReferenceTypePath = "_ReferenceType";
		private const string kCompareTypePath = "_CompareType";
		private const string kParameterIndexPath = "_ParameterIndex";
		private const string kLogicalConditionPath = "_LogicalCondition";

		private ParameterReferenceProperty _ReferenceProperty;
		private SerializedProperty _ParameterTypeProperty;
		private ClassTypeReferenceProperty _ReferenceTypeProperty;
		private SerializedProperty _CompareTypeProperty;
		private SerializedProperty _ParameterIndexProperty;
		private SerializedProperty _LogicalConditionProperty;

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

		public SerializedProperty logicalConditionProperty
		{
			get
			{
				if (_LogicalConditionProperty == null)
				{
					_LogicalConditionProperty = property.FindPropertyRelative(kLogicalConditionPath);
				}
				return _LogicalConditionProperty;
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

		public ParameterConditionProperty(SerializedProperty property)
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
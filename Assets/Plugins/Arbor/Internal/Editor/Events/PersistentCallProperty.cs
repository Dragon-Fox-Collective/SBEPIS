//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using UnityEditor;
using System.Reflection;
using System.Collections.Generic;
using System.Text;

namespace ArborEditor.Events
{
	using Arbor;
	using Arbor.Events;

	internal sealed class PersistentCallProperty
	{
		// Paths
		private const string kTargetTypePath = "_TargetType";
		private const string kTargetModePath = "_TargetMode";
		private const string kTargetComponentPath = "_TargetComponent";
		private const string kTargetGameObjectPath = "_TargetGameObject";
		private const string kTargetAssetObjectPath = "_TargetAssetObject";
		private const string kTargetSlotPath = "_TargetSlot";
		private const string kMemberTypePath = "_MemberType";
		private const string kMemberNamePath = "_MemberName";
		private const string kArgumentsPath = "_Arguments";
		private const string kReturnTypePath = "_ReturnType";
		private const string kReturnOutputSlotIndexPath = "_ReturnOutputSlotIndex";
		private const string kParameterListPath = "_ParameterList";
		private const string kOutputSlotsPath = "_OutputSlots";
		private const string kOutputSlotInstancePath = "_OutputSlotInstance";

		private ClassTypeReferenceProperty _TargetType;
		private SerializedProperty _TargetMode;
		private FlexibleComponentProperty _TargetComponent;
		private FlexibleSceneObjectProperty _TargetGameObject;
		private FlexibleFieldProperty _TargetAssetObject;
		private InputSlotBaseProperty _TargetSlot;
		private SerializedProperty _MemberType;
		private SerializedProperty _MemberName;
		private SerializedProperty _Arguments;
		private ClassTypeReferenceProperty _ReturnType;
		private SerializedProperty _ReturnOutputSlotIndex;
		private ParameterListProperty _ParameterList;
		private SerializedProperty _OutputSlots;
		private OutputSlotTypableProperty _OutputSlotInstance;

		private List<ArgumentProperty> _ArgumentProperties = null;

		private MemberInfo _MemberInfo;

		private string _MemberNameCache = null;

		public SerializedProperty property
		{
			get;
			private set;
		}

		public ClassTypeReferenceProperty targetTypeProperty
		{
			get
			{
				if (_TargetType == null)
				{
					_TargetType = new ClassTypeReferenceProperty(property.FindPropertyRelative(kTargetTypePath));
				}
				return _TargetType;
			}
		}

		public SerializedProperty targetModeProperty
		{
			get
			{
				if (_TargetMode == null)
				{
					_TargetMode = property.FindPropertyRelative(kTargetModePath);
				}
				return _TargetMode;
			}
		}

		public TargetMode targetMode
		{
			get
			{
				return EnumUtility.GetValueFromIndex<TargetMode>(targetModeProperty.enumValueIndex);
			}
			set
			{
				targetModeProperty.enumValueIndex = EnumUtility.GetIndexFromValue<TargetMode>(value);
			}
		}

		public FlexibleComponentProperty targetComponentProperty
		{
			get
			{
				if (_TargetComponent == null)
				{
					_TargetComponent = new FlexibleComponentProperty(property.FindPropertyRelative(kTargetComponentPath));
				}
				return _TargetComponent;
			}
		}

		public FlexibleSceneObjectProperty targetGameObjectProperty
		{
			get
			{
				if (_TargetGameObject == null)
				{
					_TargetGameObject = new FlexibleSceneObjectProperty(property.FindPropertyRelative(kTargetGameObjectPath));
				}
				return _TargetGameObject;
			}
		}

		public FlexibleFieldProperty targetAssetObjectProperty
		{
			get
			{
				if (_TargetAssetObject == null)
				{
					_TargetAssetObject = new FlexibleFieldProperty(property.FindPropertyRelative(kTargetAssetObjectPath));
				}
				return _TargetAssetObject;
			}
		}

		public InputSlotBaseProperty targetSlotProperty
		{
			get
			{
				if (_TargetSlot == null)
				{
					_TargetSlot = new InputSlotBaseProperty(property.FindPropertyRelative(kTargetSlotPath));
				}
				return _TargetSlot;
			}
		}

		public SerializedProperty memberTypeProperty
		{
			get
			{
				if (_MemberType == null)
				{
					_MemberType = property.FindPropertyRelative(kMemberTypePath);
				}
				return _MemberType;
			}
		}

		public SerializedProperty memberNameProperty
		{
			get
			{
				if (_MemberName == null)
				{
					_MemberName = property.FindPropertyRelative(kMemberNamePath);
				}
				return _MemberName;
			}
		}

		public SerializedProperty argumentsProperty
		{
			get
			{
				if (_Arguments == null)
				{
					_Arguments = property.FindPropertyRelative(kArgumentsPath);
				}
				return _Arguments;
			}
		}

		public ClassTypeReferenceProperty returnTypeProperty
		{
			get
			{
				if (_ReturnType == null)
				{
					_ReturnType = new ClassTypeReferenceProperty(property.FindPropertyRelative(kReturnTypePath));
				}
				return _ReturnType;
			}
		}

		public SerializedProperty returnOutputSlotIndexProperty
		{
			get
			{
				if (_ReturnOutputSlotIndex == null)
				{
					_ReturnOutputSlotIndex = property.FindPropertyRelative(kReturnOutputSlotIndexPath);
				}
				return _ReturnOutputSlotIndex;
			}
		}

		public ParameterListProperty parameterListProperty
		{
			get
			{
				if (_ParameterList == null)
				{
					_ParameterList = new ParameterListProperty(property.FindPropertyRelative(kParameterListPath));
				}
				return _ParameterList;
			}
		}

		public SerializedProperty outputSlotsProperty
		{
			get
			{
				if (_OutputSlots == null)
				{
					_OutputSlots = property.FindPropertyRelative(kOutputSlotsPath);
				}
				return _OutputSlots;
			}
		}

		public OutputSlotTypableProperty outputSlotInstanceProperty
		{
			get
			{
				if (_OutputSlotInstance == null)
				{
					_OutputSlotInstance = new OutputSlotTypableProperty(property.FindPropertyRelative(kOutputSlotInstancePath));
				}
				return _OutputSlotInstance;
			}
		}

		public List<ArgumentProperty> argumentProperties
		{
			get
			{
				if (_ArgumentProperties == null || _ArgumentProperties.Count != argumentsProperty.arraySize || Event.current.type == EventType.Layout)
				{
					_ArgumentProperties = new List<ArgumentProperty>();
					int argumentCount = argumentsProperty.arraySize;
					for (int i = 0; i < argumentCount; i++)
					{
						SerializedProperty property = argumentsProperty.GetArrayElementAtIndex(i);
						ArgumentProperty argumentProperty = new ArgumentProperty(property);
						_ArgumentProperties.Add(argumentProperty);
					}
				}
				return _ArgumentProperties;
			}
		}

		public MemberType memberType
		{
			get
			{
				return EnumUtility.GetValueFromIndex<MemberType>(memberTypeProperty.enumValueIndex);
			}
			set
			{
				memberTypeProperty.enumValueIndex = EnumUtility.GetIndexFromValue<MemberType>(value);
			}
		}

		public MemberInfo memberInfo
		{
			get
			{
				if (_MemberInfo == null || Event.current.type == EventType.Layout)
				{
					_MemberInfo = GetMemberInfo();
				}
				return _MemberInfo;
			}
			set
			{
				SetMemberInfo(value);
			}
		}

		public PersistentCallProperty(SerializedProperty property)
		{
			this.property = property;
		}

		System.Type[] GetArgumentTypes()
		{
			List<ArgumentProperty> argumentProperties = this.argumentProperties;

			int argumentCount = argumentProperties.Count;
			System.Type[] argumentTypes = new System.Type[argumentCount];
			for (int i = 0; i < argumentCount; i++)
			{
				ArgumentProperty argument = argumentProperties[i];

				argumentTypes[i] = argument.type;
			}

			return argumentTypes;
		}

		public MemberInfo GetMemberInfo()
		{
			System.Type type = targetTypeProperty.type;

			if (type == null)
			{
				return null;
			}

			switch (memberType)
			{
				case MemberType.Method:
					{
						System.Type[] argumentTypes = GetArgumentTypes();

						return MemberCache.GetMethodInfo(type, memberNameProperty.stringValue, argumentTypes);
					}
				case MemberType.Field:
					{
						return MemberCache.GetFieldInfo(type, memberNameProperty.stringValue);
					}
				case MemberType.Property:
					{
						return MemberCache.GetPropertyInfo(type, memberNameProperty.stringValue);
					}
			}

			return null;
		}

		private static StringBuilder s_MethodNameBuilder = new StringBuilder();

		string GenerateMemberName()
		{
			string methodName = this.memberNameProperty.stringValue;

			if (string.IsNullOrEmpty(methodName))
			{
				return "";
			}

			StringBuilder sb = s_MethodNameBuilder;
			sb.Length = 0;

			switch (memberType)
			{
				case MemberType.Method:
					{
						sb.Append(methodName);
						sb.Append(" (");

						List<ArgumentProperty> argumentProperties = this.argumentProperties;

						int argumentCount = argumentProperties.Count;

						for (int i = 0; i < argumentCount; i++)
						{
							ArgumentProperty argument = argumentProperties[i];

							ArgumentAttributes attributes = (ArgumentAttributes)argument.attributes;
							bool isOut = (attributes & ArgumentAttributes.Out) == ArgumentAttributes.Out;
							System.Type argumentType = argument.type;

							if (isOut)
							{
								sb.Append("out ");
							}
							else if (argumentType.IsByRef)
							{
								sb.Append("ref ");
							}

							sb.Append(TypeUtility.GetTypeName(argumentType));

							if (i < argumentCount - 1)
								sb.Append(", ");
						}

						sb.Append(")");
					}
					break;
				case MemberType.Field:
				case MemberType.Property:
					{
						ArgumentProperty valueArgument = argumentProperties[0];
						sb.Append(TypeUtility.GetTypeName(valueArgument.type));
						sb.Append(" ");
						sb.Append(methodName);
					}
					break;
			}

			return sb.ToString();
		}

		public string GetMemberName()
		{
			if (_MemberNameCache == null || Event.current.type == EventType.Layout)
			{
				_MemberNameCache = GenerateMemberName();
			}
			return _MemberNameCache;
		}

		public void Clear()
		{
			property.Clear(true);
		}

		public void ClearType()
		{
			targetComponentProperty.Disconnect();
			targetGameObjectProperty.Disconnect();
			targetAssetObjectProperty.Disconnect();
			targetSlotProperty.Disconnect();
			outputSlotInstanceProperty.Disconnect();

			targetComponentProperty.Clear(false);
			targetGameObjectProperty.Clear(false);
			targetAssetObjectProperty.Clear(false);
			targetSlotProperty.Clear();
			ClearMember();
		}

		void DisconnectParameterSlots()
		{
			parameterListProperty.DisconnectSlots();
			for (int i = 0; i < outputSlotsProperty.arraySize; i++)
			{
				SerializedProperty property = outputSlotsProperty.GetArrayElementAtIndex(i);
				OutputSlotTypableProperty outputSlotProperty = new OutputSlotTypableProperty(property);
				outputSlotProperty.Disconnect();
			}
		}

		public void ClearMember()
		{
			DisconnectParameterSlots();

			_MemberInfo = null;
			memberType = MemberType.Method;
			memberNameProperty.stringValue = "";

			_ArgumentProperties = null;
			argumentsProperty.arraySize = 0;

			returnTypeProperty.Clear();
			returnOutputSlotIndexProperty.intValue = 0;

			_MemberNameCache = null;

			parameterListProperty.ClearArray();
			outputSlotsProperty.ClearArray();
		}

		public SerializedProperty GetParametersProperty(ParameterType parameterType)
		{
			return parameterListProperty.GetParametersProperty(parameterType);
		}

		public static TargetMode GetTargetMode(System.Type targetType)
		{
			if (targetType == null)
			{
				return TargetMode.Static;
			}
			else if (targetType.IsAbstract && targetType.IsSealed)  // static
			{
				return TargetMode.Static;
			}
			else if (typeof(Component).IsAssignableFrom(targetType))
			{
				return TargetMode.Component;
			}
			else if (typeof(GameObject).IsAssignableFrom(targetType))
			{
				return TargetMode.GameObject;
			}
			else if (typeof(Object).IsAssignableFrom(targetType))
			{
				return TargetMode.AssetObject;
			}
			else
			{
				return TargetMode.Slot;
			}
		}

		public void SetReturnType(System.Type returnType)
		{
			this.returnTypeProperty.type = returnType;

			returnOutputSlotIndexProperty.intValue = outputSlotsProperty.arraySize;
			outputSlotsProperty.arraySize++;

			OutputSlotTypableProperty outputSlotProperty = new OutputSlotTypableProperty(outputSlotsProperty.GetArrayElementAtIndex(returnOutputSlotIndexProperty.intValue));
			outputSlotProperty.type = returnType;
		}

		void AddArgument(ParameterInfo parameterInfo)
		{
			List<ArgumentProperty> argumentProperties = this.argumentProperties;

			int paramIndex = argumentsProperty.arraySize;
			argumentsProperty.arraySize = paramIndex + 1;

			System.Type type = parameterInfo.ParameterType;

			ArgumentProperty argumentProperty = new ArgumentProperty(argumentsProperty.GetArrayElementAtIndex(paramIndex));
			argumentProperties.Add(argumentProperty);

			argumentProperty.name = parameterInfo.Name;
			argumentProperty.type = type;

			ArgumentAttributes argumentAttributes = ArgumentAttributes.None;
			if (parameterInfo.IsOut)
			{
				argumentAttributes |= ArgumentAttributes.Out;

				argumentProperty.outputSlotIndex = outputSlotsProperty.arraySize;
				outputSlotsProperty.arraySize++;

				OutputSlotTypableProperty outputSlotProperty = new OutputSlotTypableProperty(outputSlotsProperty.GetArrayElementAtIndex(argumentProperty.outputSlotIndex));
				outputSlotProperty.type = type.IsByRef ? type.GetElementType() : type;
			}
			else
			{
				ParameterType parameterType = ArborEventUtility.GetParameterType(type, true);

				SerializedProperty parametersProperty = GetParametersProperty(parameterType);

				if (parametersProperty != null)
				{
					argumentProperty.parameterType = parameterType;
					argumentProperty.parameterIndex = parametersProperty.arraySize;
					parametersProperty.arraySize++;

					if (parameterType == ParameterType.Slot)
					{
						SerializedProperty inputSlotParametersProperty = GetParametersProperty(ParameterType.Slot);
						SerializedProperty inputSlotParameterProperty = inputSlotParametersProperty.GetArrayElementAtIndex(inputSlotParametersProperty.arraySize - 1);
						InputSlotTypableProperty inputSlotProperty = new InputSlotTypableProperty(inputSlotParameterProperty);
						inputSlotProperty.type = type.IsByRef ? type.GetElementType() : type;
					}
				}

				if (type.IsByRef)
				{
					argumentProperty.outputSlotIndex = outputSlotsProperty.arraySize;
					outputSlotsProperty.arraySize++;

					OutputSlotTypableProperty outputSlotProperty = new OutputSlotTypableProperty(outputSlotsProperty.GetArrayElementAtIndex(argumentProperty.outputSlotIndex));
					outputSlotProperty.type = type.IsByRef ? type.GetElementType() : type;
				}
			}

			argumentProperty.attributes = argumentAttributes;
		}

		public void AddArgument(System.Type type)
		{
			List<ArgumentProperty> argumentProperties = this.argumentProperties;

			int paramIndex = argumentsProperty.arraySize;
			argumentsProperty.arraySize = paramIndex + 1;

			ArgumentProperty argumentProperty = new ArgumentProperty(argumentsProperty.GetArrayElementAtIndex(paramIndex));
			argumentProperties.Add(argumentProperty);

			argumentProperty.name = "Value";
			argumentProperty.type = type;

			ArgumentAttributes argumentAttributes = ArgumentAttributes.None;

			ParameterType parameterType = ArborEventUtility.GetParameterType(type, true);

			SerializedProperty parametersProperty = GetParametersProperty(parameterType);

			if (parametersProperty != null)
			{
				argumentProperty.parameterType = parameterType;
				argumentProperty.parameterIndex = parametersProperty.arraySize;
				parametersProperty.arraySize++;

				if (parameterType == ParameterType.Slot)
				{
					SerializedProperty inputSlotParametersProperty = GetParametersProperty(ParameterType.Slot);
					SerializedProperty inputSlotParameterProperty = inputSlotParametersProperty.GetArrayElementAtIndex(inputSlotParametersProperty.arraySize - 1);
					InputSlotTypableProperty inputSlotProperty = new InputSlotTypableProperty(inputSlotParameterProperty);
					inputSlotProperty.type = type.IsByRef ? type.GetElementType() : type;
				}
			}

			if (type.IsByRef)
			{
				argumentProperty.outputSlotIndex = outputSlotsProperty.arraySize;
				outputSlotsProperty.arraySize++;

				OutputSlotTypableProperty outputSlotProperty = new OutputSlotTypableProperty(outputSlotsProperty.GetArrayElementAtIndex(argumentProperty.outputSlotIndex));
				outputSlotProperty.type = type.IsByRef ? type.GetElementType() : type;
			}

			argumentProperty.attributes = argumentAttributes;
		}

		void SetArguments(ParameterInfo[] parameters)
		{
			int paramCount = parameters.Length;
			for (int paramIndex = 0; paramIndex < paramCount; paramIndex++)
			{
				ParameterInfo parameterInfo = parameters[paramIndex];

				AddArgument(parameterInfo);
			}
		}

		void SetMethodInfo(MethodInfo methodInfo)
		{
			memberType = MemberType.Method;
			_MemberInfo = methodInfo;

			memberNameProperty.stringValue = methodInfo.Name;

			System.Type returnType = methodInfo.ReturnType;

			if (returnType != null && returnType != typeof(void))
			{
				SetReturnType(returnType);
			}

			ParameterInfo[] parameters = methodInfo.GetParameters();
			SetArguments(parameters);
		}

		void SetFieldInfo(FieldInfo fieldInfo)
		{
			memberType = MemberType.Field;
			_MemberInfo = fieldInfo;

			memberNameProperty.stringValue = fieldInfo.Name;

			AddArgument(fieldInfo.FieldType);
		}

		void SetPropertyInfo(PropertyInfo propertyInfo)
		{
			memberType = MemberType.Property;
			_MemberInfo = propertyInfo;

			memberNameProperty.stringValue = propertyInfo.Name;

			AddArgument(propertyInfo.PropertyType);
		}

		void SetMemberInfo(MemberInfo memberInfo)
		{
			ClearMember();

			MethodInfo methodInfo = memberInfo as MethodInfo;
			if (methodInfo != null)
			{
				SetMethodInfo(methodInfo);
				return;
			}

			FieldInfo fieldInfo = memberInfo as FieldInfo;
			if (fieldInfo != null)
			{
				SetFieldInfo(fieldInfo);
				return;
			}

			PropertyInfo propertyInfo = memberInfo as PropertyInfo;
			if (propertyInfo != null)
			{
				SetPropertyInfo(propertyInfo);
				return;
			}
		}
	}
}
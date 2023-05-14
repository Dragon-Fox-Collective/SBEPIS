//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEditor;
using System.Reflection;

namespace ArborEditor.Events
{
	using Arbor.Events;

	internal sealed class ArgumentToRefRepair : InvalidRepair
	{
		private PersistentCallProperty _Call;
		//private int _ArgumentIndex;
		ParameterInfo _ParameterInfo;
		ArgumentProperty _ArgumentProperty;

		public ArgumentToRefRepair(PersistentCallProperty call, int argumentIndex)
		{
			_Call = call;
			//_ArgumentIndex = argumentIndex;

			MemberInfo memberInfo = _Call.memberInfo;

			ParameterInfo[] parameters = null;

			MethodInfo methodInfo = memberInfo as MethodInfo;
			if (methodInfo != null)
			{
				parameters = methodInfo.GetParameters();
			}

			PropertyInfo propertyInfo = memberInfo as PropertyInfo;
			if (propertyInfo != null)
			{
				parameters = propertyInfo.GetIndexParameters();
			}

			_ParameterInfo = parameters[argumentIndex];

			_ArgumentProperty = call.argumentProperties[argumentIndex];
		}

		public override string GetMessage()
		{
			return string.Format(Localization.GetWord("ArborEvent.InvalidRepair.ArgumentToRef"), ObjectNames.NicifyVariableName(_ParameterInfo.Name));
		}

		public override void OnRepair()
		{
			// Add Input Field
			System.Type type = _ParameterInfo.ParameterType;

			ParameterType parameterType = ArborEventUtility.GetParameterType(type, true);

			SerializedProperty parametersProperty = _Call.GetParametersProperty(parameterType);

			if (parametersProperty != null)
			{
				ArgumentAttributes argumentAttributes = _ArgumentProperty.attributes;
				argumentAttributes &= ~ArgumentAttributes.Out;
				_ArgumentProperty.attributes = argumentAttributes;

				_ArgumentProperty.parameterType = parameterType;
				_ArgumentProperty.parameterIndex = parametersProperty.arraySize;
				parametersProperty.arraySize++;

				if (parameterType == ParameterType.Slot)
				{
					SerializedProperty inputSlotParametersProperty = _Call.GetParametersProperty(ParameterType.Slot);
					SerializedProperty inputSlotParameterProperty = inputSlotParametersProperty.GetArrayElementAtIndex(inputSlotParametersProperty.arraySize - 1);
					InputSlotTypableProperty inputSlotProperty = new InputSlotTypableProperty(inputSlotParameterProperty);
					inputSlotProperty.type = type.IsByRef ? type.GetElementType() : type;
				}
			}
		}
	}
}
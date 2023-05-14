//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEditor;
using System.Collections.Generic;
using System.Reflection;

namespace ArborEditor.Events
{
	using Arbor.Events;

	internal sealed class ArgumentToOutRepair : InvalidRepair
	{
		private PersistentCallProperty _Call;
		//private int _ArgumentIndex;
		ParameterInfo _ParameterInfo;
		ArgumentProperty _ArgumentProperty;

		public ArgumentToOutRepair(PersistentCallProperty call, int argumentIndex)
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

			_ParameterInfo = parameters[argumentIndex];

			_ArgumentProperty = call.argumentProperties[argumentIndex];
		}

		public override string GetMessage()
		{
			return string.Format(Localization.GetWord("ArborEvent.InvalidRepair.ArgumentToOut"), ObjectNames.NicifyVariableName(_ParameterInfo.Name));
		}

		public override void OnRepair()
		{
			// Delete Input Field
			System.Type type = _ParameterInfo.ParameterType;

			ParameterType parameterType = ArborEventUtility.GetParameterType(type, true);

			SerializedProperty parametersProperty = _Call.GetParametersProperty(parameterType);

			if (parametersProperty != null)
			{
				ArgumentAttributes argumentAttributes = _ArgumentProperty.attributes;
				argumentAttributes |= ArgumentAttributes.Out;
				_ArgumentProperty.attributes = argumentAttributes;

				int parameterIndex = _ArgumentProperty.parameterIndex;

				InputSlotBaseProperty inputSlotProperty = new InputSlotBaseProperty(parametersProperty.GetArrayElementAtIndex(parameterIndex));

				inputSlotProperty.Disconnect();

				parametersProperty.DeleteArrayElementAtIndex(parameterIndex);

				List<ArgumentProperty> argumentProperties = _Call.argumentProperties;
				for (int argumentIndex = 0; argumentIndex < argumentProperties.Count; argumentIndex++)
				{
					ArgumentProperty argumentProperty = argumentProperties[argumentIndex];

					ArgumentAttributes attributes = argumentProperty.attributes;
					bool isOut = (attributes & ArgumentAttributes.Out) == ArgumentAttributes.Out;

					if (!isOut && argumentProperty.parameterType == parameterType)
					{
						if (parameterIndex < argumentProperty.parameterIndex)
						{
							argumentProperty.parameterIndex--;
						}
					}
				}
			}
		}
	}
}
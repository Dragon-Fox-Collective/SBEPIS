//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using System.Collections.Generic;
using System.Reflection;

namespace ArborEditor.Events
{
	using Arbor;
	using Arbor.Events;

	internal sealed class ReturnTypeRepair : InvalidRepair
	{
		private PersistentCallProperty _Call;

		public ReturnTypeRepair(PersistentCallProperty call)
		{
			_Call = call;
		}

		public override string GetMessage()
		{
			MethodInfo methodInfo = _Call.memberInfo as MethodInfo;

			return string.Format(Localization.GetWord("ArborEvent.InvalidRepair.ReturnType"), TypeUtility.GetTypeName(methodInfo.ReturnType));
		}

		public override void OnRepair()
		{
			System.Type returnType = _Call.returnTypeProperty.type;
			MethodInfo methodInfo = _Call.memberInfo as MethodInfo;
			System.Type methodReturnType = methodInfo.ReturnType;
			if (methodReturnType != null && methodReturnType == typeof(void))
			{
				methodReturnType = null;
			}

			if (returnType != null)
			{
				int outputSlotIndex = _Call.returnOutputSlotIndexProperty.intValue;

				OutputSlotTypableProperty outputSlotProperty = new OutputSlotTypableProperty(_Call.outputSlotsProperty.GetArrayElementAtIndex(outputSlotIndex));

				outputSlotProperty.Disconnect();

				if (methodReturnType != null)
				{
					outputSlotProperty.type = methodReturnType;
				}
				else
				{
					_Call.outputSlotsProperty.DeleteArrayElementAtIndex(outputSlotIndex);

					List<ArgumentProperty> argumentProperties = _Call.argumentProperties;
					for (int argumentIndex = 0; argumentIndex < argumentProperties.Count; argumentIndex++)
					{
						ArgumentProperty argumentProperty = argumentProperties[argumentIndex];

						ArgumentAttributes attributes = argumentProperty.attributes;
						bool isOut = (attributes & ArgumentAttributes.Out) == ArgumentAttributes.Out;

						if (isOut || argumentProperty.type.IsByRef)
						{
							if (outputSlotIndex < argumentProperty.outputSlotIndex)
							{
								argumentProperty.outputSlotIndex--;
							}
						}
					}
				}

				_Call.returnTypeProperty.type = methodReturnType;
			}
			else
			{
				_Call.SetReturnType(methodReturnType);
			}
		}
	}
}
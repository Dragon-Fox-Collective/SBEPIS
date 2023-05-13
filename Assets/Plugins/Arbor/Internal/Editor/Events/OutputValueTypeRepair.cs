//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using System.Reflection;

namespace ArborEditor.Events
{
	using Arbor;
	using Arbor.Events;

	internal sealed class OutputValueTypeRepair : InvalidRepair
	{
		private PersistentGetValueProperty _GetValueProperty;
		private System.Type _ValueType;

		public OutputValueTypeRepair(PersistentGetValueProperty getValueProperty, System.Type valueType)
		{
			_GetValueProperty = getValueProperty;
			_ValueType = valueType;
		}

		public override string GetMessage()
		{
			return string.Format(Localization.GetWord("ArborEvent.InvalidRepair.ValueType"), TypeUtility.GetTypeName(_ValueType));
		}

		public override void OnRepair()
		{
			OutputSlotTypableProperty outputValueProperty = _GetValueProperty.outputValueProperty;
			switch (_GetValueProperty.memberType)
			{
				case MemberType.Field:
					{
						FieldInfo fieldInfo = _GetValueProperty.memberInfo as FieldInfo;
						outputValueProperty.Disconnect();
						outputValueProperty.type = fieldInfo.FieldType;
					}
					break;
				case MemberType.Property:
					{
						PropertyInfo propertyInfo = _GetValueProperty.memberInfo as PropertyInfo;
						outputValueProperty.Disconnect();
						outputValueProperty.type = propertyInfo.PropertyType;
					}
					break;
			}
		}
	}
}
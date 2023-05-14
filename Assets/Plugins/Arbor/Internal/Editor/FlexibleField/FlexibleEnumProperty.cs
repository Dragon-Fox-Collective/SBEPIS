//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEditor;

namespace ArborEditor
{
	public sealed class FlexibleEnumProperty<TEnum> : FlexibleFieldProperty where TEnum : System.Enum
	{
		public TEnum value
		{
			get
			{
				return EnumUtility.GetValueFromIndex<TEnum>(valueProperty.enumValueIndex);
			}
			set
			{
				valueProperty.enumValueIndex = EnumUtility.GetIndexFromValue<TEnum>(value);
			}
		}

		public FlexibleEnumProperty(SerializedProperty property) : base(property)
		{
		}
	}
}
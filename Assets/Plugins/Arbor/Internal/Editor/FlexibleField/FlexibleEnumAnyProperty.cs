//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEditor;

namespace ArborEditor
{
	public sealed class FlexibleEnumAnyProperty<TEnum> : FlexibleFieldProperty where TEnum : System.Enum
	{
		public TEnum value
		{
			get
			{
				return (TEnum)System.Enum.ToObject(typeof(TEnum), valueProperty.intValue);
			}
			set
			{
				valueProperty.intValue = System.Convert.ToInt32(value);
			}
		}

		public FlexibleEnumAnyProperty(SerializedProperty property) : base(property)
		{
		}
	}
}
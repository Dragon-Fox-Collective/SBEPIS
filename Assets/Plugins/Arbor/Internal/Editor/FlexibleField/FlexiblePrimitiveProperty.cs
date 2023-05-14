//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEditor;

namespace ArborEditor
{
	using Arbor;

	public class FlexiblePrimitiveProperty : FlexibleFieldPropertyBase
	{
		public FlexiblePrimitiveType type
		{
			get
			{
				return EnumUtility.GetValueFromIndex<FlexiblePrimitiveType>(typeProperty.enumValueIndex);
			}
			set
			{
				FlexiblePrimitiveType type = this.type;
				if (type == value)
				{
					return;
				}

				switch (type)
				{
					case FlexiblePrimitiveType.Parameter:
						parameterProperty.Disconnect();
						break;
					case FlexiblePrimitiveType.DataSlot:
						slotProperty.Disconnect();
						break;
				}

				typeProperty.enumValueIndex = EnumUtility.GetIndexFromValue(value);
			}
		}

		public FlexiblePrimitiveProperty(SerializedProperty property) : base(property)
		{
		}

		protected override void ClearType()
		{
			type = FlexiblePrimitiveType.Constant;
		}

		public override void SetSlotType()
		{
			type = FlexiblePrimitiveType.DataSlot;
		}

		public override bool IsSlotType()
		{
			return type == FlexiblePrimitiveType.DataSlot;
		}

		public override bool IsParameterType()
		{
			return type == FlexiblePrimitiveType.Parameter;
		}
	}
}
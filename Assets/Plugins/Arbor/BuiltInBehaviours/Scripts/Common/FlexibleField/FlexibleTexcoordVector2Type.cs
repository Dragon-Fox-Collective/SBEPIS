//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
namespace Arbor
{
	[System.Serializable]
	public sealed class FlexibleTexcoordVector2Type : FlexibleField<TexcoordVector2Type>
	{
		public FlexibleTexcoordVector2Type()
		{
		}

		public FlexibleTexcoordVector2Type(TexcoordVector2Type value) : base(value)
		{
		}

		public FlexibleTexcoordVector2Type(AnyParameterReference parameter) : base(parameter)
		{
		}

		public FlexibleTexcoordVector2Type(InputSlotAny slot) : base(slot)
		{
		}

		public static explicit operator TexcoordVector2Type(FlexibleTexcoordVector2Type flexible)
		{
			return flexible.value;
		}

		public static explicit operator FlexibleTexcoordVector2Type(TexcoordVector2Type value)
		{
			return new FlexibleTexcoordVector2Type(value);
		}
	}
}
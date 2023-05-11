//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
namespace Arbor
{
	[System.Serializable]
	public sealed class FlexibleCompareType : FlexibleField<CompareType>
	{
		public FlexibleCompareType()
		{
		}

		public FlexibleCompareType(CompareType value) : base(value)
		{
		}

		public FlexibleCompareType(AnyParameterReference parameter) : base(parameter)
		{
		}

		public FlexibleCompareType(InputSlotAny slot) : base(slot)
		{
		}

		public static explicit operator CompareType(FlexibleCompareType flexible)
		{
			return flexible.value;
		}

		public static explicit operator FlexibleCompareType(CompareType value)
		{
			return new FlexibleCompareType(value);
		}
	}
}
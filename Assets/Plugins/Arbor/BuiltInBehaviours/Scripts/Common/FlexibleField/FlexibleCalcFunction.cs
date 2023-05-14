//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
namespace Arbor
{
	[System.Serializable]
	public sealed class FlexibleCalcFunction : FlexibleField<CalcFunction>
	{
		public FlexibleCalcFunction()
		{
		}

		public FlexibleCalcFunction(CalcFunction value) : base(value)
		{
		}

		public FlexibleCalcFunction(AnyParameterReference parameter) : base(parameter)
		{
		}

		public FlexibleCalcFunction(InputSlotAny slot) : base(slot)
		{
		}

		public static explicit operator CalcFunction(FlexibleCalcFunction flexible)
		{
			return flexible.value;
		}

		public static explicit operator FlexibleCalcFunction(CalcFunction value)
		{
			return new FlexibleCalcFunction(value);
		}
	}
}
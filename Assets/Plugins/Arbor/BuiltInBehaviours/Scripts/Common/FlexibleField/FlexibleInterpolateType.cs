//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
namespace Arbor
{
	[System.Serializable]
	public sealed class FlexibleInterpolateType : FlexibleField<InterpolateType>
	{
		public FlexibleInterpolateType()
		{
		}

		public FlexibleInterpolateType(InterpolateType value) : base(value)
		{
		}

		public FlexibleInterpolateType(AnyParameterReference parameter) : base(parameter)
		{
		}

		public FlexibleInterpolateType(InputSlotAny slot) : base(slot)
		{
		}

		public static explicit operator InterpolateType(FlexibleInterpolateType flexible)
		{
			return flexible.value;
		}

		public static explicit operator FlexibleInterpolateType(InterpolateType value)
		{
			return new FlexibleInterpolateType(value);
		}
	}
}
//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
namespace Arbor
{
	[System.Serializable]
	public sealed class FlexibleWaypointTimeType : FlexibleField<WaypointTimeType>
	{
		public FlexibleWaypointTimeType()
		{
		}

		public FlexibleWaypointTimeType(WaypointTimeType value) : base(value)
		{
		}

		public FlexibleWaypointTimeType(AnyParameterReference parameter) : base(parameter)
		{
		}

		public FlexibleWaypointTimeType(InputSlotAny slot) : base(slot)
		{
		}

		public static explicit operator WaypointTimeType(FlexibleWaypointTimeType flexible)
		{
			return flexible.value;
		}

		public static explicit operator FlexibleWaypointTimeType(WaypointTimeType value)
		{
			return new FlexibleWaypointTimeType(value);
		}
	}
}
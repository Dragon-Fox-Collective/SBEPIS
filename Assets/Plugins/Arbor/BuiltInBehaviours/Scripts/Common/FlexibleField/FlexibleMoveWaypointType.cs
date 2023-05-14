//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
namespace Arbor
{
	[System.Serializable]
	public sealed class FlexibleMoveWayppintType : FlexibleField<MoveWaypointType>
	{
		public FlexibleMoveWayppintType()
		{
		}

		public FlexibleMoveWayppintType(MoveWaypointType moveWaypointType) : base(moveWaypointType)
		{
		}

		public FlexibleMoveWayppintType(AnyParameterReference parameter) : base(parameter)
		{
		}

		public FlexibleMoveWayppintType(InputSlotAny slot) : base(slot)
		{
		}

		public static explicit operator MoveWaypointType(FlexibleMoveWayppintType flexible)
		{
			return flexible.value;
		}

		public static explicit operator FlexibleMoveWayppintType(MoveWaypointType value)
		{
			return new FlexibleMoveWayppintType(value);
		}
	}
}
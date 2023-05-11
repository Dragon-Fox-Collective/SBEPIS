//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
namespace Arbor
{
	[System.Serializable]
	public sealed class FlexiblePatrolCenterType : FlexibleField<PatrolCenterType>
	{
		public FlexiblePatrolCenterType()
		{
		}

		public FlexiblePatrolCenterType(PatrolCenterType value) : base(value)
		{
		}

		public FlexiblePatrolCenterType(AnyParameterReference parameter) : base(parameter)
		{
		}

		public FlexiblePatrolCenterType(InputSlotAny slot) : base(slot)
		{
		}

		public static explicit operator PatrolCenterType(FlexiblePatrolCenterType flexible)
		{
			return flexible.value;
		}

		public static explicit operator FlexiblePatrolCenterType(PatrolCenterType value)
		{
			return new FlexiblePatrolCenterType(value);
		}
	}
}
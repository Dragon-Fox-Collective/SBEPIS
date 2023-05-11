//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
namespace Arbor
{
	[System.Serializable]
	public sealed class FlexibleStateTriggerFlags : FlexibleField<StateTriggerFlags>
	{
		public FlexibleStateTriggerFlags()
		{
		}

		public FlexibleStateTriggerFlags(StateTriggerFlags stateTriggerFlags) : base(stateTriggerFlags)
		{
		}

		public FlexibleStateTriggerFlags(AnyParameterReference parameter) : base(parameter)
		{
		}

		public FlexibleStateTriggerFlags(InputSlotAny slot) : base(slot)
		{
		}

		public static explicit operator StateTriggerFlags(FlexibleStateTriggerFlags flexible)
		{
			return flexible.value;
		}

		public static explicit operator FlexibleStateTriggerFlags(StateTriggerFlags value)
		{
			return new FlexibleStateTriggerFlags(value);
		}
	}
}
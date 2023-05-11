//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
namespace Arbor
{
	[System.Serializable]
	public sealed class FlexibleAgentFinishFlags : FlexibleField<AgentFinishFlags>
	{
		public FlexibleAgentFinishFlags()
		{
		}

		public FlexibleAgentFinishFlags(AgentFinishFlags value) : base(value)
		{
		}

		public FlexibleAgentFinishFlags(AnyParameterReference parameter) : base(parameter)
		{
		}

		public FlexibleAgentFinishFlags(InputSlotAny slot) : base(slot)
		{
		}

		public static explicit operator AgentFinishFlags(FlexibleAgentFinishFlags flexible)
		{
			return flexible.value;
		}

		public static explicit operator FlexibleAgentFinishFlags(AgentFinishFlags value)
		{
			return new FlexibleAgentFinishFlags(value);
		}
	}
}
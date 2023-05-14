//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
namespace Arbor
{
	[System.Serializable]
	public sealed class FlexibleAgentUpdateType : FlexibleField<AgentUpdateType>
	{
		public FlexibleAgentUpdateType()
		{
		}

		public FlexibleAgentUpdateType(AgentUpdateType agentUpdateType) : base(agentUpdateType)
		{
		}

		public FlexibleAgentUpdateType(AnyParameterReference parameter) : base(parameter)
		{
		}

		public FlexibleAgentUpdateType(InputSlotAny slot) : base(slot)
		{
		}

		public static explicit operator AgentUpdateType(FlexibleAgentUpdateType flexible)
		{
			return flexible.value;
		}

		public static explicit operator FlexibleAgentUpdateType(AgentUpdateType value)
		{
			return new FlexibleAgentUpdateType(value);
		}
	}
}
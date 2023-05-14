//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
namespace Arbor
{
	[System.Serializable]
	public sealed class FlexibleUpdateMethodType : FlexibleField<UpdateMethodType>
	{
		public FlexibleUpdateMethodType()
		{
		}

		public FlexibleUpdateMethodType(UpdateMethodType value) : base(value)
		{
		}

		public FlexibleUpdateMethodType(AnyParameterReference parameter) : base(parameter)
		{
		}

		public FlexibleUpdateMethodType(InputSlotAny slot) : base(slot)
		{
		}

		public static explicit operator UpdateMethodType(FlexibleUpdateMethodType flexible)
		{
			return flexible.value;
		}

		public static explicit operator FlexibleUpdateMethodType(UpdateMethodType value)
		{
			return new FlexibleUpdateMethodType(value);
		}
	}
}
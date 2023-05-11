//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
namespace Arbor
{
	[System.Serializable]
	public sealed class FlexiblePostureType : FlexibleField<PostureType>
	{
		public FlexiblePostureType()
		{
		}

		public FlexiblePostureType(PostureType value) : base(value)
		{
		}

		public FlexiblePostureType(AnyParameterReference parameter) : base(parameter)
		{
		}

		public FlexiblePostureType(InputSlotAny slot) : base(slot)
		{
		}

		public static explicit operator PostureType(FlexiblePostureType flexible)
		{
			return flexible.value;
		}

		public static explicit operator FlexiblePostureType(PostureType value)
		{
			return new FlexiblePostureType(value);
		}
	}
}
//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
namespace Arbor
{
	[System.Serializable]
	public sealed class FlexibleDirectionType : FlexibleField<DirectionType>
	{
		public FlexibleDirectionType()
		{
		}

		public FlexibleDirectionType(DirectionType directionType) : base(directionType)
		{
		}

		public FlexibleDirectionType(AnyParameterReference parameter) : base(parameter)
		{
		}

		public FlexibleDirectionType(InputSlotAny slot) : base(slot)
		{
		}

		public static explicit operator DirectionType(FlexibleDirectionType flexible)
		{
			return flexible.value;
		}

		public static explicit operator FlexibleDirectionType(DirectionType value)
		{
			return new FlexibleDirectionType(value);
		}
	}
}
//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
namespace Arbor
{
	[System.Serializable]
	public sealed class FlexibleTweenMoveType : FlexibleField<TweenMoveType>
	{
		public FlexibleTweenMoveType()
		{
		}

		public FlexibleTweenMoveType(TweenMoveType value) : base(value)
		{
		}

		public FlexibleTweenMoveType(AnyParameterReference parameter) : base(parameter)
		{
		}

		public FlexibleTweenMoveType(InputSlotAny slot) : base(slot)
		{
		}

		public static explicit operator TweenMoveType(FlexibleTweenMoveType flexible)
		{
			return flexible.value;
		}

		public static explicit operator FlexibleTweenMoveType(TweenMoveType value)
		{
			return new FlexibleTweenMoveType(value);
		}
	}
}
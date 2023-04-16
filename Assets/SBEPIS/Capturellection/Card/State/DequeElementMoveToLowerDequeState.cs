using SBEPIS.Utils;

namespace SBEPIS.Capturellection.CardState
{
	public class DequeElementMoveToLowerDequeState : DequeElementTargettingState
	{
		protected override LerpTarget TargetToTargetTo => State.Card.Deque.lowerTarget;
	}
}

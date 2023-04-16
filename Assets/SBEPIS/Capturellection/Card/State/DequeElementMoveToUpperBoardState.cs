using SBEPIS.Utils;

namespace SBEPIS.Capturellection.CardState
{
	public class DequeElementMoveToUpperBoardState : DequeElementTargettingState
	{
		protected override LerpTarget TargetToTargetTo => State.Card.Deque.diajector.upperTarget;
	}
}

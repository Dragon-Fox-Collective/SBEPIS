using SBEPIS.Utils;

namespace SBEPIS.Capturellection.CardState
{
	public class DequeElementMoveToLowerBoardState : DequeElementTargettingState
	{
		protected override LerpTarget TargetToTargetTo => State.Card.Deque.diajector.GetLerpTarget(State.Card);
	}
}

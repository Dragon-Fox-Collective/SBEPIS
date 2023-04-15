using SBEPIS.Utils;

namespace SBEPIS.Capturellection.CardState
{
	public class CardMoveToLowerBoardState : CardTargettingState
	{
		protected override LerpTarget GetTargetToTargetTo(DequeElement card) => card.Deque.diajector.GetLerpTarget(card);
	}
}

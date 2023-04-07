using SBEPIS.Utils;

namespace SBEPIS.Capturellection.CardState
{
	public class CardMoveToLowerBoardState : CardTargettingState
	{
		protected override LerpTarget GetTargetToTargetTo(DequeStorable card) => card.Deque.diajector.GetLerpTarget(card);
	}
}

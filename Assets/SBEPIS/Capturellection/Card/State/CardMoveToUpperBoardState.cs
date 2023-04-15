using SBEPIS.Utils;

namespace SBEPIS.Capturellection.CardState
{
	public class CardMoveToUpperBoardState : CardTargettingState
	{
		protected override LerpTarget GetTargetToTargetTo(DequeElement card) => card.Deque.diajector.upperTarget;
	}
}

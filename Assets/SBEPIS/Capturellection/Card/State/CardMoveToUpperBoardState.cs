using SBEPIS.Utils;

namespace SBEPIS.Capturellection.CardState
{
	public class CardMoveToUpperBoardState : CardTargettingState
	{
		protected override LerpTarget GetTargetToTargetTo(DequeStorable card) => card.Deque.diajector.upperTarget;
	}
}

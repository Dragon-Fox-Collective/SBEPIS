using SBEPIS.Utils;

namespace SBEPIS.Capturellection.CardState
{
	public class CardMoveToUpperDequeState : CardTargettingState
	{
		protected override LerpTarget GetTargetToTargetTo(DequeElement card) => card.Deque.upperTarget;
	}
}

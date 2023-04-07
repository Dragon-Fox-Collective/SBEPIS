using SBEPIS.Utils;

namespace SBEPIS.Capturellection.CardState
{
	public class CardMoveToUpperDequeState : CardTargettingState
	{
		protected override LerpTarget GetTargetToTargetTo(DequeStorable card) => card.Deque.upperTarget;
	}
}

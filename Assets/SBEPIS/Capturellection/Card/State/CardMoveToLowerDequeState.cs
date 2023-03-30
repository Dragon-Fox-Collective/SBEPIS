using SBEPIS.Utils;

namespace SBEPIS.Capturellection.CardState
{
	public class CardMoveToLowerDequeState : CardTargettingState
	{
		protected override LerpTarget GetTargetToTargetTo(DequeStorable card) => card.DequeOwner.Deque.lowerTarget;
	}
}

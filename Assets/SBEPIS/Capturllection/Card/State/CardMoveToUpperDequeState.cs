using SBEPIS.Utils;

namespace SBEPIS.Capturllection.CardState
{
	public class CardMoveToUpperDequeState : CardTargettingState
	{
		protected override LerpTarget GetTargetToTargetTo(DequeStorable card) => card.owner.dequeBox.upperTarget;
	}
}

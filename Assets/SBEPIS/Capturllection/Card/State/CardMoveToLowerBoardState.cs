using SBEPIS.Utils;

namespace SBEPIS.Capturllection.CardState
{
	public class CardMoveToLowerBoardState : CardTargettingState
	{
		protected override LerpTarget GetTargetToTargetTo(DequeStorable card) => card.owner.diajector.GetLerpTarget(card);
	}
}

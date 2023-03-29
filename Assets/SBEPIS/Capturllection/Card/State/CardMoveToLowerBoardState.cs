using SBEPIS.Utils;

namespace SBEPIS.Capturllection.CardState
{
	public class CardMoveToLowerBoardState : CardTargettingState
	{
		protected override LerpTarget GetTargetToTargetTo(Card card) => card.DequeOwner.diajector.GetLerpTarget(card);
	}
}

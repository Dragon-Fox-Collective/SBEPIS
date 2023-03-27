using SBEPIS.Utils;

namespace SBEPIS.Capturllection.CardState
{
	public class CardMoveToUpperBoardState : CardTargettingState
	{
		protected override LerpTarget GetTargetToTargetTo(Card card) => card.Owner.diajector.upperTarget;
	}
}

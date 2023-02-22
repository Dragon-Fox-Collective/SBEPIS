using SBEPIS.Utils;

namespace SBEPIS.Capturllection.CardState
{
	public class CardMoveToUpperBoardState : CardTargettingState
	{
		protected override LerpTarget GetTargetToTargetTo(DequeStorable card) => card.owner.diajector.upperTarget;
	}
}

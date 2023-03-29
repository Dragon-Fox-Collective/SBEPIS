using SBEPIS.Utils;
using UnityEngine;

namespace SBEPIS.Capturllection.CardState
{
	public class CardBoardElectricArcState : CardElectricArcState
	{
		public override Transform GetPoint(Card card) => card.DequeOwner.diajector.GetLerpTarget(card).transform;
	}
}

using SBEPIS.Utils;
using UnityEngine;

namespace SBEPIS.Capturllection.CardState
{
	public class CardBoardElectricArcState : CardElectricArcState
	{
		public override Transform GetPoint(DequeStorable card) => card.owner.diajector.GetLerpTarget(card).transform;
	}
}

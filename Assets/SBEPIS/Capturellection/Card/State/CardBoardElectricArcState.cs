using SBEPIS.Utils;
using UnityEngine;

namespace SBEPIS.Capturellection.CardState
{
	public class CardBoardElectricArcState : CardElectricArcState
	{
		public override Transform GetPoint(DequeStorable card) => card.DequeOwner.diajector.GetLerpTarget(card).transform;
	}
}

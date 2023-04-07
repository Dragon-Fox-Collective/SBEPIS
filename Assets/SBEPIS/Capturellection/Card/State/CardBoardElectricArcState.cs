using SBEPIS.Utils;
using UnityEngine;

namespace SBEPIS.Capturellection.CardState
{
	public class CardBoardElectricArcState : CardElectricArcState
	{
		public override Transform GetPoint(DequeStorable card) => card.Deque.diajector.GetLerpTarget(card).transform;
	}
}

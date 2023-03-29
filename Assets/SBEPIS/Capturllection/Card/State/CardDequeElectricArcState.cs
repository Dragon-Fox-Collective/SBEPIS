using SBEPIS.UI;
using UnityEngine;

namespace SBEPIS.Capturllection.CardState
{
	public class CardDequeElectricArcState : CardElectricArcState
	{
		public override Transform GetPoint(Card card) => card.DequeOwner.Deque.transform;
	}
}

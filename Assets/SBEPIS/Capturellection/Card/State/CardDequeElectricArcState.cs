using SBEPIS.UI;
using UnityEngine;

namespace SBEPIS.Capturellection.CardState
{
	public class CardDequeElectricArcState : CardElectricArcState
	{
		public override Transform GetPoint(DequeElement card) => card.Deque.transform;
	}
}

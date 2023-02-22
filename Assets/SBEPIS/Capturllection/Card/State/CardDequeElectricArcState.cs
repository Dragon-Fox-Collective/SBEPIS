using SBEPIS.UI;
using UnityEngine;

namespace SBEPIS.Capturllection.CardState
{
	public class CardDequeElectricArcState : CardElectricArcState
	{
		public override Transform GetPoint(DequeStorable card) => card.owner.dequeBox.transform;
	}
}

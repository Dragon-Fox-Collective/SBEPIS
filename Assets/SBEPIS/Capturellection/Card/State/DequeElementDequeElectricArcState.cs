using UnityEngine;

namespace SBEPIS.Capturellection.CardState
{
	public class DequeElementDequeElectricArcState : DequeElementElectricArcState
	{
		protected override Transform Point => State.Card.Deque.transform;
	}
}

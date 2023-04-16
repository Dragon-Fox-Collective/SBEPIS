using UnityEngine;

namespace SBEPIS.Capturellection.CardState
{
	public class DequeElementBoardElectricArcState : DequeElementElectricArcState
	{
		protected override Transform Point => State.Card.Deque.diajector.GetLerpTarget(State.Card).transform;
	}
}

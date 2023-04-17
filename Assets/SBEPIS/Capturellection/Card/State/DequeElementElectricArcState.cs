using SBEPIS.UI;
using SBEPIS.Utils;
using UnityEngine;

namespace SBEPIS.Capturellection.CardState
{
	public abstract class DequeElementElectricArcState : StateMachineBehaviour<DequeElementStateMachine>
	{
		private ElectricArc arc;
		
		protected abstract Transform Point { get; }
		
		protected override void OnEnter()
		{
			arc = Instantiate(State.ElectricArcPrefab, Point);
			arc.otherPoint = State.Card.transform;
		}
		
		protected override void OnExit()
		{
			Destroy(arc);
		}
	}
}

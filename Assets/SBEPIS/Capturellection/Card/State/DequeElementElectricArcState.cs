using SBEPIS.UI;
using SBEPIS.Utils;
using UnityEngine;

namespace SBEPIS.Capturellection.CardState
{
	public abstract class DequeElementElectricArcState : StateMachineBehaviour<DequeElementStateMachine>
	{
		private ElectricArc arc;
		
		protected abstract Transform Point { get; }
		
		public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			arc = Instantiate(State.ElectricArcPrefab, Point);
			arc.otherPoint = State.Card.transform;
		}
		
		public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			Destroy(arc);
		}
	}
}

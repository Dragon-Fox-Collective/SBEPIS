using SBEPIS.UI;
using UnityEngine;

namespace SBEPIS.Capturllection.CardState
{
	public abstract class CardElectricArcState : StateMachineBehaviour
	{
		private ElectricArc arc;
		
		public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			Card card = animator.GetComponent<Card>();
			arc = Instantiate(card.DequeOwner.diajector.electricArcPrefab, GetPoint(card));
			arc.otherPoint = card.transform;
		}

		public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			Destroy(arc);
		}

		public abstract Transform GetPoint(Card card);
	}
}

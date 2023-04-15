using SBEPIS.UI;
using UnityEngine;

namespace SBEPIS.Capturellection.CardState
{
	public abstract class CardElectricArcState : StateMachineBehaviour
	{
		private ElectricArc arc;
		
		public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			DequeElement card = animator.GetComponent<DequeElement>();
			arc = Instantiate(card.Deque.diajector.electricArcPrefab, GetPoint(card));
			arc.otherPoint = card.transform;
		}

		public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			Destroy(arc);
		}

		public abstract Transform GetPoint(DequeElement card);
	}
}

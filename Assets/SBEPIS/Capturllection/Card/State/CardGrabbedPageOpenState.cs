using SBEPIS.Controller;
using SBEPIS.Utils;
using UnityEngine;

namespace SBEPIS.Capturllection.CardState
{
	public class CardGrabbedPageOpenState : StateMachineBehaviour
	{
		private CardTarget target;
		
		public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			DequeStorable card = animator.GetComponent<DequeStorable>();
			target = card.owner.diajector.GetCardTarget(card);
			target.onGrab.Invoke();
		}

		public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			target.onDrop.Invoke();
		}
	}
}

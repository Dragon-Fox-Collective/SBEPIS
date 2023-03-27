using SBEPIS.Utils;
using UnityEngine;

namespace SBEPIS.Capturllection.CardState
{
	public abstract class CardTargettingState : StateMachineBehaviour
	{
		public int startNumber;
		public int endNumber;

		public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			Card card = animator.GetComponent<Card>();
			card.State.TargetNumber = startNumber;
			
			LerpTarget target = GetTargetToTargetTo(card);
			card.Animator.TargetTo(target, SetEndNumber);
		}

		private void SetEndNumber(LerpTargetAnimator animator)
		{
			Card card = animator.GetComponent<Card>();
			card.State.TargetNumber = endNumber;
		}

		protected abstract LerpTarget GetTargetToTargetTo(Card card);
	}
}

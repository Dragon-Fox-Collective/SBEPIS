using SBEPIS.Utils;
using UnityEngine;

namespace SBEPIS.Capturellection.CardState
{
	public abstract class CardTargettingState : StateMachineBehaviour<DequeStorableStateMachine>
	{
		public int startNumber;
		public int endNumber;

		public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			DequeElement card = animator.GetComponent<DequeElement>();
			card.State.TargetNumber = startNumber;
			
			LerpTarget target = GetTargetToTargetTo(card);
			card.Animator.TargetTo(target, SetEndNumber);
		}

		private void SetEndNumber(LerpTargetAnimator animator)
		{
			DequeElement card = animator.GetComponent<DequeElement>();
			card.State.TargetNumber = endNumber;
		}

		protected abstract LerpTarget GetTargetToTargetTo(DequeElement card);
	}
}

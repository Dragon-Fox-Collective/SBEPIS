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
			DequeStorable card = animator.GetComponent<DequeStorable>();
			card.State.TargetNumber = startNumber;
			
			LerpTarget target = GetTargetToTargetTo(card);
			card.Animator.TargetTo(target, SetEndNumber);
		}

		private void SetEndNumber(LerpTargetAnimator animator)
		{
			DequeStorable card = animator.GetComponent<DequeStorable>();
			card.State.TargetNumber = endNumber;
		}

		protected abstract LerpTarget GetTargetToTargetTo(DequeStorable card);
	}
}

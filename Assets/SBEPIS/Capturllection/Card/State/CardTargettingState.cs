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
			DequeStorable card = animator.GetComponent<DequeStorable>();
			card.state.targetNumber = startNumber;
			
			LerpTarget target = GetTargetToTargetTo(card);
			card.animator.TargetTo(target, SetEndNumber);
		}

		private void SetEndNumber(LerpTargetAnimator animator)
		{
			DequeStorable card = animator.GetComponent<DequeStorable>();
			card.state.targetNumber = endNumber;
		}

		protected abstract LerpTarget GetTargetToTargetTo(DequeStorable card);
	}
}

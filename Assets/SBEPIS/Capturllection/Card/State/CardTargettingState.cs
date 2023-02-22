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
			animator.SetInteger(DequeStorable.TargetNumber, startNumber);
			
			DequeStorable card = animator.GetComponent<DequeStorable>();
			LerpTarget target = GetTargetToTargetTo(card);
			card.animator.TargetTo(target, SetEndNumber);
		}

		private void SetEndNumber(LerpTargetAnimator animator)
		{
			animator.GetComponent<Animator>().SetInteger(DequeStorable.TargetNumber, endNumber);
		}

		protected abstract LerpTarget GetTargetToTargetTo(DequeStorable card);
	}
}

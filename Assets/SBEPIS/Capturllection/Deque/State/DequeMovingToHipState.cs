using UnityEngine;

namespace SBEPIS.Capturllection.DequeState
{
	public class DequeMovingToHipState : StateMachineBehaviour
	{
		public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			DequeBox dequeBox = animator.GetComponent<DequeBox>();
			dequeBox.owner.dequeAnimator.TargetTo(dequeBox.owner.lerpTarget);
		}
	}
}

using SBEPIS.Utils;
using UnityEngine;

namespace SBEPIS.Capturllection.DequeState
{
	public class DequeMovingToHipState : StateMachineBehaviour<DequeBoxStateMachine>
	{
		public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			State.DequeBoxOwner.DequeAnimator.TargetTo(State.DequeBoxOwner.LerpTarget);
		}
	}
}

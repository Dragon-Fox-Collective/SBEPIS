using SBEPIS.Utils;
using UnityEngine;

namespace SBEPIS.Capturellection.DequeState
{
	public class DequeBoxMovingToHipState : StateMachineBehaviour<DequeBoxStateMachine>
	{
		public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			State.Animator.TargetTo(State.lerpTarget);
		}
	}
}

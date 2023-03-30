using SBEPIS.Utils;
using UnityEngine;

namespace SBEPIS.Capturellection.DequeState
{
	public class DequeMovingToHipState : StateMachineBehaviour
	{
		public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			State.DequeBoxOwner.DequeAnimator.TargetTo(State.DequeBoxOwner.LerpTarget);
		}
	}
}

using SBEPIS.Physics;
using SBEPIS.Utils;
using UnityEngine;

namespace SBEPIS.Capturllection.CardState
{
	public class UnsetPausedTargetState : StateMachineBehaviour
	{
		public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			LerpTargetAnimator lerpAnimator = animator.GetComponent<LerpTargetAnimator>();
			lerpAnimator.SetPausedAt(null);
		}
	}
}

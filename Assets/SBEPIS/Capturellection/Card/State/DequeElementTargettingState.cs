using SBEPIS.Utils;
using UnityEngine;

namespace SBEPIS.Capturellection.CardState
{
	public abstract class DequeElementTargettingState : StateMachineBehaviour<DequeElementStateMachine>
	{
		public int startNumber;
		public int endNumber;
		
		protected abstract LerpTarget TargetToTargetTo { get; }
		
		public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			State.Card.State.TargetNumber = startNumber;
			State.Card.Animator.TargetTo(TargetToTargetTo, SetEndNumber);
		}
		
		private void SetEndNumber(LerpTargetAnimator animator)
		{
			State.TargetNumber = endNumber;
		}
	}
}

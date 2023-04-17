using SBEPIS.Utils;
using UnityEngine;

namespace SBEPIS.Capturellection.CardState
{
	public abstract class DequeElementTargettingState : StateMachineBehaviour<DequeElementStateMachine>
	{
		public bool reversed;
		
		public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			State.Card.Animator.TargetTo(State.Card.Diajector.GetLerpTarget(State.Card, State.TargetIndex));
		}
	}
}

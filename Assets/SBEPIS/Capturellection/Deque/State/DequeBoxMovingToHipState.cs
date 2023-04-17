using SBEPIS.Utils;
using UnityEngine;

namespace SBEPIS.Capturellection.DequeState
{
	public class DequeBoxMovingToHipState : StateMachineBehaviour<DequeBoxStateMachine>
	{
		protected override void OnEnter()
		{
			State.Animator.TargetTo(State.lerpTarget);
		}
	}
}

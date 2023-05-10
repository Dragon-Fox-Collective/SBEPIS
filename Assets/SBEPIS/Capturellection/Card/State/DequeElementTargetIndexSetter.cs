using SBEPIS.Utils;
using UnityEngine;

namespace SBEPIS.Capturellection.CardState
{
	public class DequeElementTargetIndexSetter : StateMachineBehaviour<DequeElementStateMachine>
	{
		[Tooltip("Negative values count from the end of the list")]
		[SerializeField] private int index;
		
		protected override void OnEnter()
		{
			int finalTargetIndex = index >= 0 ? index : State.Card.Diajector.LerpTargetCount + index - 1;
			if (finalTargetIndex > State.TargetIndex)
				CountUp(finalTargetIndex);
			else if (finalTargetIndex < State.TargetIndex)
				CountDown(finalTargetIndex);
		}
		
		private void CountUp(int finalTargetIndex)
		{
			for (; State.TargetIndex < finalTargetIndex; State.TargetIndex++)
				State.Card.Animator.TeleportTo(State.Card.Diajector.GetLerpTarget(State.Card, State.TargetIndex + 1));
			State.Card.Animator.TeleportTo(State.Card.Diajector.GetLerpTarget(State.Card, State.TargetIndex + 1));
		}
		
		private void CountDown(int finalTargetIndex)
		{
			for (; State.TargetIndex > finalTargetIndex; State.TargetIndex--)
				State.Card.Animator.TeleportTo(State.Card.Diajector.GetLerpTarget(State.Card, State.TargetIndex));
			State.Card.Animator.TeleportTo(State.Card.Diajector.GetLerpTarget(State.Card, State.TargetIndex));
		}
	}
}

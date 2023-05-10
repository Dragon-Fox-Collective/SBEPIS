using SBEPIS.Utils;
using UnityEngine;

namespace SBEPIS.Capturellection.CardState
{
	public class DequeElementTargettingState : StateMachineBehaviour<DequeElementStateMachine>
	{
		[SerializeField] private bool reversed;
		
		protected override void OnEnter()
		{
			if (reversed) MoveBackward();
			else MoveForward();
		}
		
		private void MoveForward()
		{
			State.Card.Animator.TargetTo(State.Card.Diajector.GetLerpTarget(State.Card, State.TargetIndex + 1), State.TargetIndex < State.Card.Diajector.LerpTargetCount - 2 ? KeepMovingForward : null);
		}
		
		private void KeepMovingForward(LerpTargetAnimator animator)
		{
			State.TargetIndex++;
			MoveForward();
		}
		
		private void MoveBackward()
		{
			State.Card.Animator.TargetTo(State.Card.Diajector.GetLerpTarget(State.Card, State.TargetIndex), State.TargetIndex > 0 ? KeepMovingBackward : null);
		}
		
		private void KeepMovingBackward(LerpTargetAnimator animator)
		{
			State.TargetIndex--;
			MoveBackward();
		}
		
		protected override void OnExit()
		{
			State.Card.Animator.Cancel();
		}
	}
}

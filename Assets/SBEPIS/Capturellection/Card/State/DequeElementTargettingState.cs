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
			Debug.Log($"Forward {State.TargetIndex} {State.Card.Diajector.GetLerpTarget(State.Card, State.TargetIndex + 1)}");
			State.Card.Animator.TargetTo(State.Card.Diajector.GetLerpTarget(State.Card, State.TargetIndex + 1), State.TargetIndex < State.Card.Diajector.LerpTargetCount - 1 ? KeepMovingForward : null);
		}
		
		private void KeepMovingForward(LerpTargetAnimator animator)
		{
			State.TargetIndex++;
			MoveForward();
		}
		
		private void MoveBackward()
		{
			Debug.Log($"Backward {State.TargetIndex} {State.Card.Diajector.GetLerpTarget(State.Card, State.TargetIndex)}");
			State.Card.Animator.TargetTo(State.Card.Diajector.GetLerpTarget(State.Card, State.TargetIndex), State.TargetIndex > 0 ? KeepMovingBackward : null);
		}
		
		private void KeepMovingBackward(LerpTargetAnimator animator)
		{
			State.TargetIndex--;
			MoveBackward();
		}
		
		protected override void OnExit()
		{
			Debug.Log("Cancelled");
			State.Card.Animator.Cancel();
		}
	}
}

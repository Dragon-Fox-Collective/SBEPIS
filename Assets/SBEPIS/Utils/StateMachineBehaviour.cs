using System;
using UnityEngine;
using UnityEngine.Animations;

namespace SBEPIS.Utils
{
	public abstract class StateMachineBehaviour<TStateMachine> : StateMachineBehaviour where TStateMachine : StateMachine
	{
		protected TStateMachine State { get; private set; }
		
		public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int stateMachinePathHash, AnimatorControllerPlayable controller)
		{
			GetState(animator);
			OnEnter();
		}
		
		private void GetState(Animator animator)
		{
			if (State) return;
			
			State = animator.GetComponent<TStateMachine>();
			if (!State) throw new NullReferenceException($"State machine animator {animator} didn't have a state machine of type {typeof(TStateMachine)}");
		}
		
		protected virtual void OnEnter() { }
		
		public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex, AnimatorControllerPlayable controller)
		{
			OnExit();
		}
		
		protected virtual void OnExit() { }
	}
}

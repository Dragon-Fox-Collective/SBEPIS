using System;
using UnityEngine;
using UnityEngine.Animations;

namespace SBEPIS.Utils
{
	public abstract class StateMachineBehaviour<TStateMachine> : StateMachineBehaviour where TStateMachine : StateMachine
	{
		protected TStateMachine State { get; private set; }
		
		public override void OnStateMachineEnter(Animator animator, int stateMachinePathHash, AnimatorControllerPlayable controller)
		{
			State = animator.GetComponent<TStateMachine>();
		}
	}
}

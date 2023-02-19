using UnityEngine;

namespace SBEPIS.Capturllection.DequeState
{
	public class DequePrimedState : StateMachineBehaviour
	{
		public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			DequeBox dequeBox = animator.GetComponent<DequeBox>();
			dequeBox.collisionTrigger.StartPrime();
		}

		public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			DequeBox dequeBox = animator.GetComponent<DequeBox>();
			dequeBox.collisionTrigger.CancelPrime();
		}
	}
}

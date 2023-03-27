using SBEPIS.Utils;
using UnityEngine;

namespace SBEPIS.Capturllection.DequeState
{
	public class CollisionTriggerPrimeState : StateMachineBehaviour
	{
		public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			animator.GetComponent<CollisionTrigger>().StartPrime();
		}

		public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			animator.GetComponent<CollisionTrigger>().CancelPrime();
		}
	}
}

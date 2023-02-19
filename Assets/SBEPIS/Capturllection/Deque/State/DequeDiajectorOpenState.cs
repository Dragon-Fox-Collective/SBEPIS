using UnityEngine;

namespace SBEPIS.Capturllection.DequeState
{
	public class DequeDiajectorOpenState : StateMachineBehaviour
	{
		public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			DequeBox dequeBox = animator.GetComponent<DequeBox>();
			if (dequeBox.owner.diajector.isOpen)
				return;
			
			Vector3 position = dequeBox.transform.position;
			Vector3 upDirection = dequeBox.gravitySum.upDirection;
			Vector3 groundDelta = Vector3.ProjectOnPlane(dequeBox.owner.transform.position - position, upDirection);
			Quaternion rotation = Quaternion.LookRotation(groundDelta, upDirection);
			dequeBox.owner.diajector.StartAssembly(position, rotation);
		}

		public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			DequeBox dequeBox = animator.GetComponent<DequeBox>();
			if (!dequeBox.owner)
				return;
			
			dequeBox.owner.diajector.StartDisassembly();
		}
	}
}

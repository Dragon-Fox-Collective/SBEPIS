using SBEPIS.Physics;
using SBEPIS.Utils;
using UnityEngine;

namespace SBEPIS.Capturllection.DequeState
{
	public class DequeDiajectorOpenState : StateMachineBehaviour<DequeBoxStateMachine>
	{
		public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			if (State.DequeOwner.diajector.IsOpen)
				return;
			
			Vector3 position = State.DequeBox.transform.position;
			Vector3 upDirection = State.GravitySum.upDirection;
			Vector3 groundDelta = Vector3.ProjectOnPlane(State.DequeOwner.transform.position - position, upDirection);
			Quaternion rotation = Quaternion.LookRotation(groundDelta, upDirection);
			State.DequeOwner.diajector.StartAssembly(position, rotation);
		}
		
		public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			if (!State.DequeOwner)
				return;
			
			State.DequeOwner.diajector.StartDisassembly();
		}
	}
}

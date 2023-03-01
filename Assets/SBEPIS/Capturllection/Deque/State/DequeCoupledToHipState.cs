using SBEPIS.Controller;
using UnityEngine;

namespace SBEPIS.Capturllection.DequeState
{
	public class DequeCoupledToHipState : StateMachineBehaviour
	{
		public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			DequeBox dequeBox = animator.GetComponent<DequeBox>();
			CouplingPlug plug = dequeBox.plug;
			CouplingSocket socket = dequeBox.owner.socket;
			socket.Couple(plug);
		}
		
		public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			DequeBox dequeBox = animator.GetComponent<DequeBox>();
			CouplingPlug plug = dequeBox.plug;
			CouplingSocket socket = dequeBox.plug.coupledSocket;
			socket.Decouple(plug);
			dequeBox.transform.position += dequeBox.transform.forward * 0.1f;
		}
	}
}

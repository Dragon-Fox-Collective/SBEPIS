using SBEPIS.Controller;
using SBEPIS.Utils;
using UnityEngine;

namespace SBEPIS.Capturellection.DequeState
{
	public class DequeBoxCoupledToHipState : StateMachineBehaviour<DequeBoxStateMachine>
	{
		protected override void OnExit()
		{
			CouplingPlug plug = State.Plug;
			CouplingSocket socket = State.Plug.CoupledSocket;
			socket.Decouple(plug);
			State.DequeBox.transform.position += State.DequeBox.transform.forward * 0.1f;
		}
	}
}

using SBEPIS.Utils;
using UnityEngine;

namespace SBEPIS.Capturellection.CardState
{
	public class EnablerDisablerState : StateMachineBehaviour<DequeElementStateMachine>
	{
		protected override void OnEnter()
		{
			State.EnablerDisabler.Disable();
		}
		
		protected override void OnExit()
		{
			State.EnablerDisabler.Enable();
		}
	}
}

using SBEPIS.Utils;
using UnityEngine;

namespace SBEPIS.Capturellection.CardState
{
	public class EnablerDisablerState : StateMachineBehaviour
	{
		public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			EnablerDisabler enabler = animator.GetComponent<EnablerDisabler>();
			enabler.Disable();
		}
		
		public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			EnablerDisabler enabler = animator.GetComponent<EnablerDisabler>();
			enabler.Enable();
		}
	}
}

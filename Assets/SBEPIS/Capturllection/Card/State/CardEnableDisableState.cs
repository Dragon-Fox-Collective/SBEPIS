using SBEPIS.Utils;
using UnityEngine;

namespace SBEPIS.Capturllection.CardState
{
	public class CardEnableDisableState : StateMachineBehaviour
	{
		[Tooltip("If enabled, the gameobject is disabled on entry and enabled on exit")]
		public bool invert;
		
		public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			animator.gameObject.SetActive(!invert);
		}
		
		public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			animator.gameObject.SetActive(invert);
		}
	}
}

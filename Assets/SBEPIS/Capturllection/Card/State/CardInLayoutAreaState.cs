using UnityEngine;

namespace SBEPIS.Capturllection.CardState
{
	public class CardInLayoutAreaState : StateMachineBehaviour
	{
		public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			Card card = animator.GetComponent<Card>();
			DiajectorCaptureLayout layout = card.PopAllLayouts();
			CardTarget target = layout.AddPermanentTargetAndCard(card);
			card.Animator.TeleportTo(target.lerpTarget);
		}
	}
}

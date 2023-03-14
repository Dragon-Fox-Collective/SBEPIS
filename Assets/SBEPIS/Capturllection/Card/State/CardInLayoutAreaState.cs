using UnityEngine;

namespace SBEPIS.Capturllection.CardState
{
	public class CardInLayoutAreaState : StateMachineBehaviour
	{
		public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			DequeStorable card = animator.GetComponent<DequeStorable>();
			DiajectorCaptureLayout layout = card.PopAllLayouts();
			CardTarget target = layout.AddPermanentTargetAndCard(card);
			card.animator.TeleportTo(target.lerpTarget);
		}
	}
}

using SBEPIS.Utils;
using UnityEngine;

namespace SBEPIS.Capturellection.CardState
{
	public class CardInLayoutAreaState : StateMachineBehaviour<DequeStorableStateMachine>
	{
		public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			DiajectorCaptureLayout layout = State.LayoutAdder.PopAllLayouts();
			CardTarget target = layout.AddPermanentTargetAndCard(State.Card);
			State.Card.Animator.TeleportTo(target.LerpTarget);
		}
	}
}

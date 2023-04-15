using SBEPIS.Physics;
using SBEPIS.Utils;
using UnityEngine;

namespace SBEPIS.Capturellection.CardState
{
	public class CardOnBoardState : StateMachineBehaviour
	{
		private JointTargetter targetter;
		
		public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			DequeElement card = animator.GetComponent<DequeElement>();
			LerpTarget target = card.Deque.diajector.GetLerpTarget(card);
			if (!target)
				return;
			Rigidbody staticRigidbody = card.Deque.diajector.staticRigidbody;
			StrengthSettings cardStrength = card.Deque.diajector.cardStrength;

			targetter = staticRigidbody.gameObject.AddComponent<JointTargetter>();
			targetter.connectedBody = card.Grabbable.Rigidbody;
			targetter.target = target.transform;
			targetter.strength = cardStrength;
		}

		public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			if (targetter)
				Destroy(targetter);
		}
	}
}

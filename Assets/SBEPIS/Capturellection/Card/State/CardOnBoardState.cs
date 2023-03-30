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
			DequeStorable card = animator.GetComponent<DequeStorable>();
			LerpTarget target = card.DequeOwner.diajector.GetLerpTarget(card);
			if (!target)
				return;
			Rigidbody staticRigidbody = card.DequeOwner.diajector.staticRigidbody;
			StrengthSettings cardStrength = card.DequeOwner.diajector.cardStrength;

			targetter = staticRigidbody.gameObject.AddComponent<JointTargetter>();
			targetter.connectedBody = card.Grabbable.rigidbody;
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

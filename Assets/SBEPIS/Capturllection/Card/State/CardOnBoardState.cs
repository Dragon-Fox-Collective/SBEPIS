using SBEPIS.Physics;
using SBEPIS.Utils;
using UnityEngine;

namespace SBEPIS.Capturllection.CardState
{
	public class CardOnBoardState : StateMachineBehaviour
	{
		private JointTargetter targetter;
		
		public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			DequeStorable card = animator.GetComponent<DequeStorable>();
			LerpTarget target = card.owner.diajector.GetLerpTarget(card);
			Rigidbody staticRigidbody = card.owner.diajector.staticRigidbody;
			StrengthSettings cardStrength = card.owner.diajector.cardStrength;

			targetter = staticRigidbody.gameObject.AddComponent<JointTargetter>();
			targetter.connectedBody = card.grabbable.rigidbody;
			targetter.target = target.transform;
			targetter.strength = cardStrength;
		}

		public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			Destroy(targetter);
		}
	}
}

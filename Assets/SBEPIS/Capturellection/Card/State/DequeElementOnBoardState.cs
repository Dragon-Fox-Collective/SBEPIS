using SBEPIS.Physics;
using SBEPIS.Utils;
using UnityEngine;

namespace SBEPIS.Capturellection.CardState
{
	public class DequeElementOnBoardState : StateMachineBehaviour<DequeElementStateMachine>
	{
		private JointTargetter targetter;
		
		public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			LerpTarget target = State.Card.Diajector.GetLerpTarget(State.Card);
			if (!target)
				return;
			Rigidbody staticRigidbody = State.Card.Diajector.staticRigidbody;
			StrengthSettings cardStrength = State.Card.Diajector.cardStrength;
			
			targetter = staticRigidbody.gameObject.AddComponent<JointTargetter>();
			targetter.connectedBody = State.Card.Grabbable.Rigidbody;
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

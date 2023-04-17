using SBEPIS.Physics;
using SBEPIS.Utils;

namespace SBEPIS.Capturellection.CardState
{
	public class DequeElementOnBoardState : StateMachineBehaviour<DequeElementStateMachine>
	{
		private JointTargetter targetter;
		
		protected override void OnEnter()
		{
			LerpTarget target = State.Card.Diajector.GetLerpTarget(State.Card);
			if (!target || !State.Rigidbody)
				return;
			
			targetter = State.Card.Diajector.StaticRigidbody.gameObject.AddComponent<JointTargetter>();
			targetter.connectedBody = State.Rigidbody;
			targetter.target = target.transform;
			targetter.strength = State.CardStrength;
		}
		
		protected override void OnExit()
		{
			if (targetter)
				Destroy(targetter);
		}
	}
}

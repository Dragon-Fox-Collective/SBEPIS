using KBCore.Refs;
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
			if (!target)
				return;
			
			if (State.Rigidbody)
				CreateTargetter(target);
		}

		private void CreateTargetter(LerpTarget target)
		{
			targetter = State.Card.Diajector.StaticRigidbody.gameObject.AddComponent<JointTargetter>();
			targetter.ConnectedBody = State.Rigidbody;
			targetter.Target = target.transform;
			targetter.Strength = State.CardStrength;
		}
		
		protected override void OnExit()
		{
			if (targetter)
				Destroy(targetter);
		}
	}
}
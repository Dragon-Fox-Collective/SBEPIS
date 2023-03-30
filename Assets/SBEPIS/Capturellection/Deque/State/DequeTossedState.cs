using SBEPIS.Utils;
using UnityEngine;

namespace SBEPIS.Capturellection.DequeState
{
	public class DequeTossedState : StateMachineBehaviour
	{
		public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			Transform tossTarget = State.DequeBoxOwner.tossTarget;
			float tossHeight = State.DequeBoxOwner.tossHeight;
			
			State.GravitySum.rigidbody.velocity = CalcTossVelocity(
				State.DequeBox.transform,
				tossTarget,
				tossHeight,
				tossTarget.up,
				State.GravitySum.gravityAcceleration);
		}
		
		private static Vector3 CalcTossVelocity(Transform box, Transform tossTarget, float tossHeight, Vector3 upDirection, float gravityAcceleration)
		{
			gravityAcceleration *= -1;
			float startHeight = tossTarget.InverseTransformPoint(box.position).y;
			float upTossSpeed = CalcTossYVelocity(gravityAcceleration, startHeight, startHeight + tossHeight);
			Vector3 upwardVelocity = upDirection * upTossSpeed;
			
			float timeToHit = (-upTossSpeed - Mathf.Sqrt(upTossSpeed * upTossSpeed - 2 * gravityAcceleration * startHeight)) / gravityAcceleration;
			Vector3 groundDelta = Vector3.ProjectOnPlane(tossTarget.position - box.position, upDirection);
			Vector3 groundVelocity = groundDelta / timeToHit;
			
			return upwardVelocity + groundVelocity;
		}
		
		private static float CalcTossYVelocity(float gravity, float startHeight, float peakHeight) => Mathf.Sqrt(2 * gravity * (startHeight - peakHeight));
	}
}

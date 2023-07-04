using KBCore.Refs;
using UnityEngine;

namespace SBEPIS.Physics
{
	public class FootBall : ValidatedMonoBehaviour
	{
		[SerializeField, Anywhere] private SphereCollider ball;
		[SerializeField, Anywhere] private ConfigurableJoint joint;
		
		public void Move(Vector3 upDirection, Vector3 velocity)
		{
			Vector3 angularVelocity = velocity.magnitude / ball.radius * Vector3.Cross(upDirection, velocity).normalized;
			joint.targetAngularVelocity = -joint.transform.InverseTransformVector(angularVelocity);
			LockFootballIfStopping();
		}
		
		private void LockFootballIfStopping()
		{
			if (joint.targetAngularVelocity == Vector3.zero)
				ball.attachedRigidbody.constraints |= RigidbodyConstraints.FreezeRotation;
			else
				ball.attachedRigidbody.constraints &= ~RigidbodyConstraints.FreezeRotation;
		}
	}
}

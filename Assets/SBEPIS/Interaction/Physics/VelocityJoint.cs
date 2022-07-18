using SBEPIS.Interaction.Controller;
using UnityEngine;

namespace SBEPIS.Interaction.Physics
{
	[RequireComponent(typeof(Rigidbody))]
	public class VelocityJoint : MonoBehaviour
	{
		public Transform connectionPoint;
		public StrengthSettings strength;

		private new Rigidbody rigidbody;

		private void Awake()
		{
			rigidbody = GetComponent<Rigidbody>();
		}

		private void FixedUpdate()
		{
			UpdatePosition();
			UpdateRotation();
		}

		private void UpdatePosition()
		{
			//rigidbody.velocity = Approach(rigidbody.velocity, connectionPoint.position - rigidbody.position, strength.velocityTarget, strength.maxAcceleration, transform.lossyScale.x);
			rigidbody.velocity = OldApproach(connectionPoint.position - rigidbody.position, strength.strength, strength.maxEffectiveDistance);
		}

		private void UpdateRotation()
		{
			Vector3 delta = (connectionPoint.rotation * Quaternion.Inverse(rigidbody.rotation)).eulerAngles;
			if (delta.x > 180) delta.x -= 360;
			if (delta.y > 180) delta.y -= 360;
			if (delta.z > 180) delta.z -= 360;
			delta *= Mathf.Deg2Rad;
			//rigidbody.angularVelocity = Approach(rigidbody.angularVelocity, delta, strength.angularVelocityTarget, strength.maxAngularAcceleration, transform.lossyScale.x);
			rigidbody.angularVelocity = OldApproach(delta, strength.torque, strength.maxEffectiveAngle);
		}

		private static Vector3 Approach(Vector3 currentVelocity, Vector3 delta, float speedTarget, float maxAcceleration, float scale)
		{
			Vector3 oneFrameThreshold = delta / Time.fixedDeltaTime;
			Vector3 velocityTarget = delta.normalized * speedTarget;
			if (oneFrameThreshold.sqrMagnitude < velocityTarget.sqrMagnitude) velocityTarget = oneFrameThreshold;
			float scaledMaxAcceleration = maxAcceleration * scale;
			return scaledMaxAcceleration > 0 ? Vector3.MoveTowards(currentVelocity, velocityTarget, scaledMaxAcceleration) : velocityTarget;
		}

		private static Vector3 OldApproach(Vector3 delta, float strength, float maxEffectiveDistance)
		{
			Vector3 oneFrameThreshold = delta / Time.fixedDeltaTime;
			Vector3 velocity = strength * delta;
			float maxVelocity = strength * maxEffectiveDistance;
			if (maxVelocity > 0) velocity = Vector3.ClampMagnitude(velocity, maxVelocity);
			if (oneFrameThreshold.sqrMagnitude < velocity.sqrMagnitude) velocity = oneFrameThreshold;
			return velocity;
		}
	}
}

using SBEPIS.Interaction.Controller;
using UnityEngine;

namespace SBEPIS.Interaction.Physics
{
	[RequireComponent(typeof(Rigidbody))]
	public class VelocityJoint : MonoBehaviour
	{
		public Transform connectionPoint;
		public Rigidbody attachedRigidbody;
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
			Vector3 newVelocity = ApproachCurve(connectionPoint.position - rigidbody.position, strength.strengthCurve);
			Vector3 force = (newVelocity - rigidbody.velocity) * rigidbody.mass / Time.fixedDeltaTime;
			attachedRigidbody.AddForce(-force);
			rigidbody.velocity = newVelocity;
		}

		private void UpdateRotation()
		{
			Vector3 delta = (connectionPoint.rotation * Quaternion.Inverse(rigidbody.rotation)).eulerAngles;
			if (delta.x > 180) delta.x -= 360;
			if (delta.y > 180) delta.y -= 360;
			if (delta.z > 180) delta.z -= 360;
			delta *= Mathf.Deg2Rad;
			delta *= strength.torqueInputFactor;
			rigidbody.angularVelocity = ApproachCurve(delta, strength.strengthCurve);
		}

		private static Vector3 ApproachCurve(Vector3 delta, AnimationCurve strength)
		{
			Vector3 oneFrameThreshold = delta / Time.fixedDeltaTime;
			Vector3 velocity = strength.Evaluate(delta.magnitude) * delta.normalized;
			if (oneFrameThreshold.sqrMagnitude < velocity.sqrMagnitude) velocity = oneFrameThreshold;
			return velocity;
		}
	}
}

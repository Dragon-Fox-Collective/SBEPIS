using UnityEngine;

namespace SBEPIS.Interaction
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
			rigidbody.velocity = Vector3.zero;
			rigidbody.angularVelocity = Vector3.zero;

			Vector3 velocity = strength.velocityFactor * (connectionPoint.position - rigidbody.position);
			if (strength.maxVelocity > 0) velocity = Vector3.ClampMagnitude(velocity, strength.maxVelocity);
			rigidbody.AddForce(velocity, ForceMode.Acceleration);

			(connectionPoint.rotation * Quaternion.Inverse(rigidbody.rotation)).ToAngleAxis(out float angle, out Vector3 axis);
			if (angle > 180) angle -= 360;
			Vector3 angularVelocity = angle == 0 ? Vector3.zero : strength.angularVelocityFactor * angle * axis;
			if (strength.maxAngularVelocity > 0) angularVelocity = Vector3.ClampMagnitude(angularVelocity, strength.maxAngularVelocity);
			rigidbody.AddTorque(angularVelocity, ForceMode.Acceleration);
		}
	}
}

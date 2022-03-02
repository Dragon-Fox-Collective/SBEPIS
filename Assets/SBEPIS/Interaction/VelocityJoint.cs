using UnityEngine;

namespace SBEPIS.Interaction
{
	[RequireComponent(typeof(Rigidbody))]
	public class VelocityJoint : MonoBehaviour
	{
		public Transform connectionPoint;
		public float velocityFactor = 1;
		public float angularVelocityFactor = 1;

		private new Rigidbody rigidbody;

		private void Awake()
		{
			rigidbody = GetComponent<Rigidbody>();
		}

		private void FixedUpdate()
		{
			rigidbody.velocity = Vector3.zero;
			rigidbody.angularVelocity = Vector3.zero;

			rigidbody.AddForce(velocityFactor * (connectionPoint.position - rigidbody.position));

			(connectionPoint.rotation * Quaternion.Inverse(rigidbody.rotation)).ToAngleAxis(out float angle, out Vector3 axis);
			if (angle > 180) angle -= 360;
			print(axis + " " + angle + " " + angularVelocityFactor * angle * axis);
			rigidbody.AddTorque(angle == 0 ? Vector3.zero : angularVelocityFactor * angle * axis);
		}
	}
}

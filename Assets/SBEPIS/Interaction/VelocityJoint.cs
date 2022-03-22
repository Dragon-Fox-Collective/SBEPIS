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
			Vector3 positionDelta = connectionPoint.position - rigidbody.position;
			Vector3 maxVelocity = positionDelta / Time.fixedDeltaTime;
			Vector3 intendedVelocity = positionDelta.normalized * strength.velocity;
			Vector3 velocityTarget = maxVelocity.sqrMagnitude < intendedVelocity.sqrMagnitude ? maxVelocity : intendedVelocity;
			float maxAcceleration = strength.maxAcceleration * transform.lossyScale.x;
			rigidbody.velocity = maxAcceleration > 0 ? Vector3.MoveTowards(rigidbody.velocity, velocityTarget, maxAcceleration) : velocityTarget;

			Vector3 angularDelta = (connectionPoint.rotation * Quaternion.Inverse(rigidbody.rotation)).eulerAngles;
			if (angularDelta.x > 180) angularDelta.x -= 360;
			if (angularDelta.y > 180) angularDelta.y -= 360;
			if (angularDelta.z > 180) angularDelta.z -= 360;
			angularDelta *= Mathf.Deg2Rad;
			Vector3 maxAngularVelocity = angularDelta / Time.fixedDeltaTime;
			Vector3 intendedAngularVelocity = angularDelta.normalized * strength.angularVelocity;
			Vector3 angularVelocityTarget = maxAngularVelocity.sqrMagnitude < intendedAngularVelocity.sqrMagnitude ? maxAngularVelocity : intendedAngularVelocity;
			float maxAngularAcceleration = strength.maxAngularAcceleration * transform.lossyScale.x;
			rigidbody.angularVelocity = maxAngularAcceleration > 0 ? Vector3.MoveTowards(rigidbody.angularVelocity, angularVelocityTarget, maxAngularAcceleration) : angularVelocityTarget;
		}
	}
}

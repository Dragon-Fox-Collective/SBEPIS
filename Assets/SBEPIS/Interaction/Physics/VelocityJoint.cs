using SBEPIS.Interaction.Controller;
using System;
using UnityEngine;

namespace SBEPIS.Interaction.Physics
{
	[DisallowMultipleComponent]
	[RequireComponent(typeof(Rigidbody))]
	public class VelocityJoint : MonoBehaviour
	{
		[Serializable]
		public struct VelocityJointTarget
		{
			public Transform target;
			public StrengthSettings strength;
		}

		public Rigidbody attachedRigidbody;
		public VelocityJointTarget[] targets = new VelocityJointTarget[0];

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
			Vector3 newVelocity = targets.Sum(target => ApproachCurve(target.target.position - rigidbody.position, target.strength.strength));
			newVelocity += attachedRigidbody.velocity;
			Vector3 force = (newVelocity - rigidbody.velocity) * rigidbody.mass / Time.fixedDeltaTime;
			attachedRigidbody.AddForce(-force);
			rigidbody.velocity = newVelocity;
		}

		private void UpdateRotation()
		{
			Vector3 newVelocity = targets.Sum(target =>
			{
				Vector3 delta = (target.target.rotation * Quaternion.Inverse(rigidbody.rotation)).eulerAngles;
				if (delta.x > 180) delta.x -= 360;
				if (delta.y > 180) delta.y -= 360;
				if (delta.z > 180) delta.z -= 360;
				delta *= Mathf.Deg2Rad;
				Vector3 velocity = ApproachCurve(delta, target.strength.torque);
				velocity *= target.strength.linearDistanceTorqueFactor.Evaluate(Vector3.Distance(target.target.position, rigidbody.position));
				return velocity;
			});
			rigidbody.angularVelocity = newVelocity;
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

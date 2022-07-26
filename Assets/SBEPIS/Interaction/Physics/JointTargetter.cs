using UnityEngine;

namespace SBEPIS.Interaction.Controller
{
	[RequireComponent(typeof(Rigidbody))]
	public class JointTargetter : MonoBehaviour
	{
		public ConfigurableJoint joint;
		public Transform target;
		public Transform anchor;
		public float anchorDistance = 0.5f;
		public StrengthSettings strength;

		private new Rigidbody rigidbody;
		private Quaternion initialOffset;
		private Vector3 prevTargetPosition;
		private Quaternion prevTargetRotation;

		private void Awake()
		{
			rigidbody = GetComponent<Rigidbody>();
			rigidbody.inertiaTensorRotation = Quaternion.identity;
			rigidbody.inertiaTensor = Vector3.one * 0.02f;
		}

		private void Start()
		{
			initialOffset = (joint.transform.rotation.Inverse() * transform.rotation).Inverse();
			joint.xDrive = joint.yDrive = joint.zDrive = new JointDrive
			{
				positionSpring = strength.linearSpring,
				positionDamper = strength.linearDamper,
				maximumForce = strength.linearMaxForce,
			};
			joint.slerpDrive = new JointDrive
			{
				positionSpring = strength.angularSpring,
				positionDamper = strength.angularDamper,
				maximumForce = strength.angularMaxForce,
			};
		}

		private void FixedUpdate()
		{
			UpdatePosition();
			UpdateRotation();
		}

		private void UpdatePosition()
		{
			Vector3 targetPosition = joint.transform.InverseTransformPoint(target.position);
			if (anchor)
			{
				Vector3 anchorPosition = joint.transform.InverseTransformPoint(anchor.position);
				joint.targetPosition = anchorPosition + Vector3.ClampMagnitude(targetPosition - anchorPosition, anchorDistance);
			}
			else
				joint.targetPosition = targetPosition;
			joint.targetVelocity = (targetPosition - prevTargetPosition) / Time.fixedDeltaTime;
			prevTargetPosition = targetPosition;
		}

		private void UpdateRotation()
		{
			joint.targetRotation = joint.transform.rotation.Inverse() * target.rotation * initialOffset;
			Quaternion delta = target.rotation * prevTargetRotation.Inverse();
			if (delta.w < 0) delta = delta.Select(x => -x);
			delta.ToAngleAxis(out float angle, out Vector3 axis);
			angle *= Mathf.Deg2Rad;
			joint.targetAngularVelocity = joint.transform.rotation.Inverse() * (angle / Time.fixedDeltaTime * axis);
			prevTargetRotation = target.rotation;
		}
	}
}

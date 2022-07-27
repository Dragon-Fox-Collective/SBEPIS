using UnityEngine;

namespace SBEPIS.Interaction.Controller
{
	[RequireComponent(typeof(Rigidbody))]
	public class JointTargetter : MonoBehaviour
	{
		public Rigidbody connectedRigidbody;
		public Transform target;
		public Transform anchor;
		public float anchorDistance = 0.5f;
		public StrengthSettings strength;
		public CameraRigMover headMover;

		private new Rigidbody rigidbody;
		private ConfigurableJoint joint;
		private Quaternion initialOffset;
		private Vector3 prevTargetPosition;
		private Quaternion prevTargetRotation;
		private Quaternion prevBodyRotation;

		private void Awake()
		{
			rigidbody = GetComponent<Rigidbody>();
			rigidbody.inertiaTensorRotation = Quaternion.identity;
			rigidbody.inertiaTensor = Vector3.one * 0.02f;
		}

		private void Start()
		{
			initialOffset = (connectedRigidbody.transform.rotation.Inverse() * transform.rotation).Inverse();

			joint = connectedRigidbody.gameObject.AddComponent<ConfigurableJoint>();
			joint.connectedBody = rigidbody;
			joint.autoConfigureConnectedAnchor = false;
			joint.anchor = joint.connectedAnchor = Vector3.zero;
			joint.rotationDriveMode = RotationDriveMode.Slerp;
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
			Vector3 targetPosition = connectedRigidbody.transform.InverseTransformPoint(target.position);
			if (anchor)
			{
				Vector3 anchorPosition = connectedRigidbody.transform.InverseTransformPoint(anchor.position);
				joint.targetPosition = anchorPosition + Vector3.ClampMagnitude(targetPosition - anchorPosition, anchorDistance);
			}
			else
				joint.targetPosition = targetPosition;

			Vector3 velocity = (targetPosition - prevTargetPosition) / Time.fixedDeltaTime;
			Vector3 headVelocity = Vector3.zero;//connectedRigidbody.transform.InverseTransformVector(headMover.actualDelta) / Time.fixedDeltaTime;
			Vector3 bodyTangentialVelocity = Vector3.zero;//(connectedRigidbody.rotation * prevBodyRotation.Inverse() * targetPosition - targetPosition) / Time.fixedDeltaTime;
			joint.targetVelocity = velocity + headVelocity + bodyTangentialVelocity;

			prevTargetPosition = targetPosition;
			prevBodyRotation = connectedRigidbody.rotation;
		}

		private void UpdateRotation()
		{
			joint.targetRotation = connectedRigidbody.transform.rotation.Inverse() * target.rotation * initialOffset;

			Quaternion delta = target.rotation * prevTargetRotation.Inverse();
			if (delta.w < 0) delta = delta.Select(x => -x);
			delta.ToAngleAxis(out float angle, out Vector3 axis);
			angle *= Mathf.Deg2Rad;
			joint.targetAngularVelocity = connectedRigidbody.transform.rotation.Inverse() * (angle / Time.fixedDeltaTime * axis);

			prevTargetRotation = target.rotation;
		}
	}
}

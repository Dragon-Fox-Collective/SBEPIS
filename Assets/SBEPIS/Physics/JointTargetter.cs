using UnityEngine;

namespace SBEPIS.Physics
{
	[RequireComponent(typeof(Rigidbody))]
	public class JointTargetter : MonoBehaviour
	{
		[SerializeField] private new Rigidbody rigidbody;
		
		public Rigidbody connectedBody;
		
		public Transform target;
		
		public Transform anchor;
		public float anchorDistance = 0.5f;

		public StrengthSettings strength;

		private ConfigurableJoint joint;
		
		private Quaternion initialOffset;
		
		private Vector3 prevTargetPosition;
		private Quaternion prevTargetRotation;
		
		private Vector3 initialTensor;
		private Quaternion initialTensorRotation;
		
		private void Awake()
		{
			rigidbody = GetComponent<Rigidbody>();
		}
		
		private void Start()
		{
			Vector3 thisInitialPosition = rigidbody.position;
			
			initialOffset = transform.InverseTransformRotation(connectedBody.transform.rotation).Inverse();
			
			initialTensor = connectedBody.inertiaTensor;
			initialTensorRotation = connectedBody.inertiaTensorRotation;

			connectedBody.inertiaTensor = Vector3.one * 0.02f;
			connectedBody.inertiaTensorRotation = Quaternion.identity;
			
			joint = gameObject.AddComponent<ConfigurableJoint>();
			
			joint.connectedBody = connectedBody;
			
			joint.autoConfigureConnectedAnchor = false;
			joint.anchor = joint.connectedAnchor = Vector3.zero;

			joint.xDrive = joint.yDrive = joint.zDrive = new JointDrive
			{
				positionSpring = strength.linearSpeed * strength.linearSpeed,
				positionDamper = 2 * strength.linearSpeed,
				maximumForce = strength.linearMaxForce,
			};

			joint.rotationDriveMode = RotationDriveMode.Slerp;
			joint.slerpDrive = new JointDrive
			{
				positionSpring = strength.angularSpeed * strength.angularSpeed,
				positionDamper = 2 * strength.angularSpeed,
				maximumForce = strength.angularMaxForce,
			};

			if (!connectedBody.isKinematic)
			{
				connectedBody.velocity = Vector3.zero;
				connectedBody.angularVelocity = Vector3.zero;
			}

			prevTargetPosition = transform.InverseTransformPoint(target.position);
			prevTargetRotation = target.rotation;

			rigidbody.position = thisInitialPosition;
		}

		private void FixedUpdate()
		{
			UpdatePosition();
			UpdateRotation();
		}
		
		private void UpdatePosition()
		{
			Vector3 targetPosition = transform.InverseTransformPoint(target.position);
			if (anchor)
			{
				Vector3 anchorPosition = transform.InverseTransformPoint(anchor.position);
				targetPosition = anchorPosition + Vector3.ClampMagnitude(targetPosition - anchorPosition, anchorDistance);
			}

			joint.targetPosition = targetPosition;

			//if (accountForTargetMovement)
				//joint.targetVelocity = (targetPosition - prevTargetPosition) / Time.fixedDeltaTime;

			prevTargetPosition = targetPosition;
		}

		private void UpdateRotation()
		{
			joint.targetRotation = transform.InverseTransformRotation(target.rotation) * initialOffset;

			/*if (accountForTargetMovement)
			{
				Vector3 axis = (target.rotation * prevTargetRotation.Inverse()).ToEulersAngleAxis();
				joint.targetAngularVelocity = transform.rotation.Inverse() * (axis / Time.fixedDeltaTime);
			}*/

			prevTargetRotation = target.rotation;
		}

		private void OnDestroy()
		{
			if (joint)
			{
				Destroy(joint);

				if (connectedBody)
				{
					connectedBody.inertiaTensor = initialTensor;
					connectedBody.inertiaTensorRotation = initialTensorRotation;
				}
			}
		}
	}
}

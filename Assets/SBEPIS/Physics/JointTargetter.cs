using KBCore.Refs;
using UnityEngine;

namespace SBEPIS.Physics
{
	[RequireComponent(typeof(Rigidbody))]
	public class JointTargetter : ValidatedMonoBehaviour
	{
		[SerializeField, Self] private new Rigidbody rigidbody;
		
		[SerializeField, Anywhere] private Rigidbody connectedBody;
		public Rigidbody ConnectedBody
		{
			get => connectedBody;
			set
			{
				connectedBody = value;
				if (joint) UpdateJointConnectedBody();
			}
		}
		
		[SerializeField, Anywhere] private Transform target;
		public Transform Target
		{
			get => target;
			set
			{
				target = value;
				if (joint) UpdateJointTarget();
			}
		}
		
		[SerializeField, Anywhere(Flag.Optional)] private Transform anchor;
		public Transform Anchor
		{
			get => anchor;
			set => anchor = value;
		}
		
		[SerializeField] private float anchorDistance = 0.5f;
		public float AnchorDistance
		{
			get => anchorDistance;
			set => anchorDistance = value;
		}
		
		[SerializeField, Anywhere] private StrengthSettings strength;
		public StrengthSettings Strength
		{
			get => strength;
			set
			{
				strength = value;
				if (joint) UpdateJointDrive();
			}
		}
		
		private ConfigurableJoint joint;
		
		private Quaternion initialOffset;
		
		private Vector3 prevTargetPosition;
		private Quaternion prevTargetRotation;
		
		private Vector3 initialTensor;
		private Quaternion initialTensorRotation;
		
		private void Awake()
		{
			if (!rigidbody) rigidbody = GetComponent<Rigidbody>();
		}
		
		private void Start()
		{
			Vector3 thisInitialPosition = rigidbody.position;
			
			joint = gameObject.AddComponent<ConfigurableJoint>();
			joint.autoConfigureConnectedAnchor = false;
			joint.anchor = joint.connectedAnchor = Vector3.zero;
			UpdateJointConnectedBody();
			UpdateJointDrive();
			UpdateJointTarget();
			
			rigidbody.position = thisInitialPosition;
		}

		private void UpdateJointTarget()
		{
			prevTargetPosition = transform.InverseTransformPoint(target.position);
			prevTargetRotation = target.rotation;
		}
		
		private void UpdateJointConnectedBody()
		{
			initialOffset = transform.InverseTransformRotation(connectedBody.transform.rotation).Inverse();
			
			initialTensor = connectedBody.inertiaTensor;
			initialTensorRotation = connectedBody.inertiaTensorRotation;
			
			connectedBody.inertiaTensor = Vector3.one * 0.02f;
			connectedBody.inertiaTensorRotation = Quaternion.identity;
			
			joint.connectedBody = connectedBody;
			
			if (!connectedBody.isKinematic)
			{
				connectedBody.velocity = Vector3.zero;
				connectedBody.angularVelocity = Vector3.zero;
			}
		}
		
		private void UpdateJointDrive()
		{
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

using System;
using KBCore.Refs;
using SBEPIS.Utils;
using UnityEngine;

namespace SBEPIS.Physics
{
	[RequireComponent(typeof(Rigidbody))]
	public class JointTargetter : MonoBehaviour
	{
		[SerializeField, Self] private new Rigidbody rigidbody;
		[SerializeField, Anywhere] private Rigidbody connectedBody;
		[SerializeField, Anywhere] private Transform target;
		[SerializeField, Anywhere(Flag.Optional)] private Transform anchor;
		[SerializeField] private float anchorDistance = 0.5f;
		[SerializeField, Anywhere] private StrengthSettings strength;
		
		private ConfigurableJoint joint;
		
		private Quaternion initialOffset;
		
		private Vector3 prevTargetPosition;
		private Quaternion prevTargetRotation;
		
		private Vector3 initialTensor;
		private Quaternion initialTensorRotation;
		
		private bool hasBeenInitialized = false;
		
		public void OnValidate()
		{
			if (Application.isPlaying && Time.frameCount > 0)
				return;
			this.ValidateRefs();
		}
		
		public void Init(Rigidbody connectedBody, Transform target, StrengthSettings strength)
		{
			if (hasBeenInitialized)
				throw new InvalidOperationException($"Object {this} has already been initialized");
			hasBeenInitialized = true;
			
			this.connectedBody = connectedBody;
			this.target = target;
			this.strength = strength;
			this.ValidateRefs(true);
		}
		
		private void Start()
		{
			hasBeenInitialized = true;
			
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
			if (!target)
			{
				// Probably not best but the only way I can figure out how to deal with destroyed objects
				Destroy(this);
				return;
			}
			
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

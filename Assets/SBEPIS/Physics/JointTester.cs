using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace SBEPIS.Physics
{
	[RequireComponent(typeof(Rigidbody))]
	public class JointTester : MonoBehaviour
	{
		public Rigidbody connectedBody;
		
		public Transform target;

		public Vector3 targetPosition;
		public Vector3 targetVelocity;

		public bool updateTargetPosition = false;
		public bool updateTargetVelocity = false;

		public float spring = 0;
		public float damping = 0;
		
		public float dampingTime = 1;
		public float maxSpeed = Mathf.Infinity;

		private new Rigidbody rigidbody;
		private ConfigurableJoint joint;

		private void Awake()
		{
			rigidbody = GetComponent<Rigidbody>();
			joint = gameObject.AddComponent<ConfigurableJoint>();
		}

		private void Start()
		{
			joint.autoConfigureConnectedAnchor = false;
			
			joint.connectedBody = connectedBody;
			
			joint.targetPosition = targetPosition;
			joint.targetVelocity = targetVelocity;

			joint.xDrive = joint.yDrive = joint.zDrive = new JointDrive
			{
				positionSpring = spring,
				positionDamper = damping,
				maximumForce = Mathf.Infinity,
			};
		}

		private void FixedUpdate()
		{
			if (updateTargetPosition)
				joint.targetPosition = transform.InverseTransformPoint(target.position);

			if (updateTargetVelocity)
				joint.targetVelocity = Vector3.ClampMagnitude((target.position - connectedBody.position) / dampingTime, maxSpeed);
		}
	}
}

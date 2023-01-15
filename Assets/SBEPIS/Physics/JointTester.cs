using UnityEngine;

namespace SBEPIS.Physics
{
	[RequireComponent(typeof(Rigidbody))]
	public class JointTester : MonoBehaviour
	{
		public Rigidbody connectedBody;

		public Vector3 initialTargetPosition;
		public Vector3 initialTargetVelocity;
		
		public Transform target;

		public bool updateTargetPosition = false;
		
		public float positionAngularFrequency = 0;
		public float positionDampingRatio = 0;
		
		public bool updateTargetVelocity = false;
		
		public float velocityDampingTime = 1;
		public float velocityMaxSpeed = Mathf.Infinity;

		private new Rigidbody rigidbody;
		private ConfigurableJoint joint;

		private void Awake()
		{
			rigidbody = GetComponent<Rigidbody>();
			joint = gameObject.AddComponent<ConfigurableJoint>();
		}

		private void Start()
		{
			Vector3 initialPosition = rigidbody.position;
			
			joint.autoConfigureConnectedAnchor = false;
			
			joint.connectedBody = connectedBody;
			
			joint.targetPosition = initialTargetPosition;
			joint.targetVelocity = initialTargetVelocity;
			
			if (updateTargetVelocity)
				joint.xDrive = joint.yDrive = joint.zDrive = new JointDrive
				{
					positionSpring = 0,
					positionDamper = 1000000,
					maximumForce = Mathf.Infinity,
				};
			else
				joint.xDrive = joint.yDrive = joint.zDrive = new JointDrive
				{
					positionSpring = positionAngularFrequency * positionAngularFrequency,
					positionDamper = 2 * positionDampingRatio * positionAngularFrequency,
					maximumForce = Mathf.Infinity,
				};

			rigidbody.position = initialPosition;
		}

		private void FixedUpdate()
		{
			if (updateTargetPosition)
				joint.targetPosition = transform.InverseTransformPoint(target.position);

			if (updateTargetVelocity)
				joint.targetVelocity = Vector3.ClampMagnitude((target.position - connectedBody.position) / velocityDampingTime, velocityMaxSpeed);
			
			// Debug.Log($"Body {rigidbody.position} {rigidbody.velocity} Hand {connectedBody.position} {connectedBody.velocity} Target {target.position}");
		}
	}
}

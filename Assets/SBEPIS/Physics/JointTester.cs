using KBCore.Refs;
using UnityEngine;

namespace SBEPIS.Physics
{
	[RequireComponent(typeof(Rigidbody))]
	public class JointTester : ValidatedMonoBehaviour
	{
		[SerializeField, Self]
		private new Rigidbody rigidbody;
		
		[SerializeField, Anywhere]
		private Rigidbody connectedBody;
		
		[SerializeField]
		private Vector3 initialTargetPosition;
		[SerializeField]
		private Vector3 initialTargetVelocity;
		
		[SerializeField, Anywhere]
		private Transform target;

		[SerializeField]
		private bool updateTargetPosition = false;
		
		[SerializeField]
		private float positionAngularFrequency = 0;
		[SerializeField]
		private float positionDampingRatio = 0;
		
		[SerializeField]
		private bool updateTargetVelocity = false;
		
		[SerializeField]
		private float velocityDampingTime = 1;
		[SerializeField]
		private float velocityMaxSpeed = Mathf.Infinity;

		private ConfigurableJoint joint;

		private void Awake()
		{
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

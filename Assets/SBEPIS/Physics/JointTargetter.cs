using UnityEngine;
using UnityEngine.Serialization;

namespace SBEPIS.Physics
{
	[RequireComponent(typeof(Rigidbody))]
	public class JointTargetter : MonoBehaviour
	{
		public Rigidbody connectedBody;
		
		public Transform target;

		public StrengthSettings strength;

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

			joint.xDrive = joint.yDrive = joint.zDrive = new JointDrive
			{
				positionSpring = 0,
				positionDamper = 1000000,
				maximumForce = strength.linearMaxForce,
			};

			joint.rotationDriveMode = RotationDriveMode.Slerp;
			joint.slerpDrive = new JointDrive
			{
				positionSpring = 0,
				positionDamper = 1000000,
				maximumForce = strength.angularMaxForce,
			};

			rigidbody.position = initialPosition;
		}

		private void FixedUpdate()
		{
			joint.targetVelocity = Vector3.ClampMagnitude(transform.InverseTransformVector(target.position - connectedBody.position) / strength.linearDampingTime, strength.linearMaxSpeed);
			joint.targetAngularVelocity = Vector3.ClampMagnitude(transform.InverseTransformVector((target.rotation * connectedBody.rotation.Inverse()).ToEulersAngleAxis()) / strength.angularDampingTime, strength.angularMaxSpeed);
		}
	}
}

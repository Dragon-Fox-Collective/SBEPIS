using KBCore.Refs;
using UnityEngine;
using CallbackContext = UnityEngine.InputSystem.InputAction.CallbackContext;

namespace SBEPIS.Controller
{
	[RequireComponent(typeof(Rigidbody), typeof(Orientation))]
	public class MovementController : MonoBehaviour
	{
		[SerializeField, Self]
		private new Rigidbody rigidbody;
		[SerializeField, Self]
		private Orientation orientation;
		
		private void OnValidate() => this.ValidateRefs();
		
		public Transform moveAimer;
		
		public SphereCollider footballCollider;
		public ConfigurableJoint footballJoint;

		public float maxGroundSpeed = 8;
		public float groundAcceleration = 10;
		public float airAcceleration = 1;
		public float sprintFactor = 2;
		public float sprintControlThreshold = 0.9f;
		
		private Vector3 controlsTarget;
		private bool isTryingToSprint;
		private bool isSprinting;

		private void FixedUpdate()
		{
			MoveTick();
		}

		private void MoveTick()
		{
			UpdateSprinting();
			Accelerate(orientation.RelativeVelocity, orientation.UpDirection);
		}

		private void UpdateSprinting()
		{
			if (isSprinting && controlsTarget.magnitude < sprintControlThreshold)
				isSprinting = false;
			else if (!isSprinting && isTryingToSprint && controlsTarget.magnitude > sprintControlThreshold)
				isSprinting = true;
		}

		private void Accelerate(Vector3 currentVelocity, Vector3 upDirection)
		{
			Vector3 accelerationControl = moveAimer.right * controlsTarget.x + Vector3.Cross(moveAimer.right, upDirection) * controlsTarget.z;
			AccelerateGround(upDirection, accelerationControl);
			if (accelerationControl != Vector3.zero && !orientation.IsGrounded)
				AccelerateAir(currentVelocity, accelerationControl);
		}

		private void AccelerateGround(Vector3 upDirection, Vector3 accelerationControl)
		{
			float maxSpeed = maxGroundSpeed * accelerationControl.magnitude * (isSprinting ? sprintFactor : 1);
			Vector3 newVelocity = accelerationControl.normalized * maxSpeed;
			Vector3 angularVelocity = newVelocity.magnitude / footballCollider.radius * Vector3.Cross(upDirection, newVelocity).normalized;
			footballJoint.targetAngularVelocity = -footballJoint.transform.InverseTransformVector(angularVelocity);
			LockFootballIfStopping();
		}

		private void LockFootballIfStopping()
		{
			if (footballJoint.targetAngularVelocity == Vector3.zero)
				footballCollider.attachedRigidbody.constraints |= RigidbodyConstraints.FreezeRotation;
			else
				footballCollider.attachedRigidbody.constraints &= ~RigidbodyConstraints.FreezeRotation;
		}

		private void AccelerateAir(Vector3 currentVelocity, Vector3 accelerationControl)
		{
			//float maxSpeed = Mathf.Max(currentVelocity.magnitude, maxGroundSpeed * accelerationControl.magnitude * (isSprinting ? sprintFactor : 1));
			//Vector3 newVelocity = currentVelocity + Time.fixedDeltaTime * airAcceleration * accelerationControl.normalized;
			//Vector3 clampedNewVelocity = Vector3.ClampMagnitude(newVelocity, maxSpeed);
			//rigidbody.velocity += clampedNewVelocity - currentVelocity;
			
			Vector3 newVelocity = currentVelocity + Time.fixedDeltaTime * airAcceleration * accelerationControl;
			rigidbody.velocity += newVelocity - currentVelocity;
		}

		public void OnMove(CallbackContext context)
		{
			Vector2 controls = context.ReadValue<Vector2>();
			controlsTarget = new Vector3(controls.x, 0, controls.y);
		}

		public void OnSprint(CallbackContext context)
		{
			isTryingToSprint = context.performed;
		}
	}
}

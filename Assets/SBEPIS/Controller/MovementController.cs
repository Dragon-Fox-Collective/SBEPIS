using System;
using UnityEngine;
using CallbackContext = UnityEngine.InputSystem.InputAction.CallbackContext;

namespace SBEPIS.Controller
{
	[RequireComponent(typeof(Rigidbody), typeof(Orientation))]
	public class MovementController : MonoBehaviour
	{
		public Transform moveAimer;

		public SphereCollider footballCollider;
		public ConfigurableJoint footballJoint;

		public float maxGroundSpeed = 8;
		public float groundAcceleration = 10;
		public float airAcceleration = 1;
		public float sprintFactor = 2;
		public float sprintControlThreshold = 0.9f;

		private new Rigidbody rigidbody;
		private Orientation orientation;
		private Vector3 controlsTarget;
		private bool isTryingToSprint;
		private bool isSprinting;

		private void Awake()
		{
			rigidbody = GetComponent<Rigidbody>();
			orientation = GetComponent<Orientation>();
		}

		private void FixedUpdate()
		{
			MoveTick();
		}

		private void MoveTick()
		{
			CheckSprint();
			Accelerate(orientation.relativeVelocity, orientation.groundVelocity, orientation.upDirection);
		}

		private void CheckSprint()
		{
			if (isSprinting && controlsTarget.magnitude < sprintControlThreshold)
				isSprinting = false;
			else if (!isSprinting && isTryingToSprint && controlsTarget.magnitude > sprintControlThreshold)
				isSprinting = true;
		}

		private void Accelerate(Vector3 velocity, Vector3 groundVelocity, Vector3 upDirection)
		{
			Vector3 accelerationDirection = moveAimer.right * controlsTarget.x + Vector3.Cross(moveAimer.right, upDirection) * controlsTarget.z;
			AccelerateGround(groundVelocity, upDirection, accelerationDirection);
			if (accelerationDirection != Vector3.zero && !orientation.isGrounded)
				AccelerateAir(velocity, upDirection, accelerationDirection);
		}

		private void AccelerateGround(Vector3 groundVelocity, Vector3 upDirection, Vector3 accelerationDirection)
		{
			float maxSpeed = maxGroundSpeed * accelerationDirection.magnitude * (isSprinting ? sprintFactor : 1);
			Vector3 newVelocity = accelerationDirection.normalized * maxSpeed;
			Vector3 angularVelocity = newVelocity.magnitude / footballCollider.radius * Vector3.Cross(upDirection, newVelocity).normalized;
			footballJoint.targetAngularVelocity = -footballJoint.transform.InverseTransformVector(angularVelocity);
			
			if (angularVelocity == Vector3.zero)
				footballCollider.attachedRigidbody.constraints |= RigidbodyConstraints.FreezeRotation;
			else
				footballCollider.attachedRigidbody.constraints &= ~RigidbodyConstraints.FreezeRotation;
		}

		private void AccelerateAir(Vector3 velocity, Vector3 upDirection, Vector3 accelerationDirection)
		{
			float maxSpeed = Mathf.Max(velocity.magnitude, maxGroundSpeed * accelerationDirection.magnitude * (isSprinting ? sprintFactor : 1));
			Vector3 newVelocity = velocity + Time.fixedDeltaTime * airAcceleration * accelerationDirection.normalized;
			Vector3 clampedNewVelocity = Vector3.ClampMagnitude(newVelocity, maxSpeed);
			rigidbody.velocity += clampedNewVelocity - velocity;
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

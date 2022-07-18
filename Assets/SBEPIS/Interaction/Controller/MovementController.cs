using UnityEngine;
using CallbackContext = UnityEngine.InputSystem.InputAction.CallbackContext;

namespace SBEPIS.Interaction.Controller
{
	[RequireComponent(typeof(Rigidbody), typeof(GroundDetector))]
	public class MovementController : MonoBehaviour
	{
		public Transform moveAimer;
		public float maxGroundSpeed = 8;
		public float groundAcceleration = 10;
		public float airAcceleration = 1;
		public float frictionDeceleration = 1;

		private new Rigidbody rigidbody;
		private GroundDetector groundDetector;
		private Vector3 controlsTarget;

		private void Awake()
		{
			rigidbody = GetComponent<Rigidbody>();
			groundDetector = GetComponent<GroundDetector>();
		}

		private void Start()
		{
			Cursor.lockState = CursorLockMode.Locked;
		}

		private void FixedUpdate()
		{
			MoveTick();
		}

		private void MoveTick()
		{
			Vector3 groundVelocity = new(rigidbody.velocity.x, 0, rigidbody.velocity.z);

			// if the player is moving controls and is not grounded, accelerate in that direction until we are moving faster than the max
			// if the player is moving controls and is grounded, accelerate, probably apply friction in not the direction that the player is going
			// if the player is not moving controls and the player is not grounded, do nothing
			// if the player is not moving controls and the player is grounded, apply friction

			Accelerate(groundVelocity);
			ApplyFriction(groundVelocity);
		}

		private void Accelerate(Vector3 groundVelocity)
		{
			if (controlsTarget != Vector3.zero)
			{
				Vector3 accelerationDirection = moveAimer.right * controlsTarget.x + Vector3.Cross(moveAimer.right, Vector3.up) * controlsTarget.z;
				float maxSpeed = Mathf.Max(groundVelocity.magnitude, maxGroundSpeed * accelerationDirection.magnitude);
				Vector3 newVelocity = groundVelocity + Time.fixedDeltaTime * (groundDetector.isGrounded ? groundAcceleration : airAcceleration) * accelerationDirection.normalized;
				Vector3 clampedNewVelocity = Vector3.ClampMagnitude(newVelocity, maxSpeed);
				rigidbody.velocity += clampedNewVelocity - groundVelocity;
			}
		}

		private void ApplyFriction(Vector3 groundVelocity)
		{
			if (controlsTarget == Vector3.zero && groundDetector.isGrounded)
			{
				if (groundVelocity.magnitude < 0.01)
					rigidbody.velocity = new Vector3(0, rigidbody.velocity.y, 0);
				else
					rigidbody.AddForce(-groundVelocity.normalized * frictionDeceleration, ForceMode.Acceleration);
			}
		}

		public void OnMove(CallbackContext context)
		{
			Vector2 controls = context.ReadValue<Vector2>();
			controlsTarget = new Vector3(controls.x, 0, controls.y);
			Debug.Log(controlsTarget);
		}
	}
}

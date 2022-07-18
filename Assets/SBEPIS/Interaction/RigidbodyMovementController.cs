using UnityEngine;
using CallbackContext = UnityEngine.InputSystem.InputAction.CallbackContext;

namespace SBEPIS.Interaction
{
	[RequireComponent(typeof(Rigidbody))]
	public class RigidbodyMovementController : MonoBehaviour
	{
		public Transform pitchRotator;
		public Transform yawRotator;
		public Transform moveAimer;
		public float sensitivity = 1;
		public float maxGroundSpeed = 8;
		public float groundAcceleration = 10;
		public float airAcceleration = 1;
		public float jumpSpeed = 3;
		public float frictionDeceleration = 1;
		public Transform groundCheck;
		public float groundCheckDistance = 0.3f;
		public LayerMask groundCheckMask;

		private new Rigidbody rigidbody;
		private float camPitch;
		private float camYaw;
		private Vector3 controlsTarget;
		private bool isGrounded;
		private readonly Collider[] groundedColliders = new Collider[1];

		private void Awake()
		{
			rigidbody = GetComponent<Rigidbody>();
		}

		private void Start()
		{
			Cursor.lockState = CursorLockMode.Locked;
		}

		private void FixedUpdate()
		{
			isGrounded = Physics.OverlapSphereNonAlloc(groundCheck.position, groundCheckDistance, groundedColliders, groundCheckMask) > 0;
			Vector3 groundVelocity = new Vector3(rigidbody.velocity.x, 0, rigidbody.velocity.z);
			float currentSpeed = groundVelocity.magnitude;

			// if the player is moving controls and is not grounded, accelerate in that direction until we are moving faster than the max
			// if the player is moving controls and is grounded, accelerate, probably apply friction in not the direction that the player is going
			// if the player is not moving controls and the player is not grounded, do nothing
			// if the player is not moving controls and the player is grounded, apply friction

			// Accelerate
			if (controlsTarget != Vector3.zero)
			{
				Vector3 accelerationDirection = moveAimer.right * controlsTarget.x + Vector3.Cross(moveAimer.right, Vector3.up) * controlsTarget.z;
				float maxSpeed = Mathf.Max(currentSpeed, maxGroundSpeed);
				Vector3 newVelocity = groundVelocity + Time.fixedDeltaTime * (isGrounded ? groundAcceleration : airAcceleration) * accelerationDirection;
				Vector3 clampedNewVelocity = Vector3.ClampMagnitude(newVelocity, maxSpeed);
				rigidbody.velocity += clampedNewVelocity - groundVelocity;
				Debug.Log($"{groundVelocity} {newVelocity} {clampedNewVelocity} {rigidbody.velocity}");
			}
			
			// Apply friction
			else if (isGrounded)
			{
				if (currentSpeed < 0.01)
				{
					rigidbody.velocity = new Vector3(0, rigidbody.velocity.y, 0);
				}
				else
				{
					Vector3 decelerationDirection = -groundVelocity.normalized;
					rigidbody.AddForce(decelerationDirection * frictionDeceleration, ForceMode.Acceleration);
				}
			}
		}

		public void OnLookPitch(CallbackContext context)
		{
			float pitch = context.ReadValue<float>() * sensitivity;
			camPitch = Mathf.Clamp(camPitch - pitch, -90, 90);

			Vector3 localRotation = pitchRotator.transform.localRotation.eulerAngles;
			localRotation.x = camPitch;
			pitchRotator.transform.localRotation = Quaternion.Euler(localRotation);
		}

		public void OnLookYaw(CallbackContext context)
		{
			float yaw = context.ReadValue<float>() * sensitivity;
			camYaw += yaw;

			Vector3 localRotation = yawRotator.transform.localRotation.eulerAngles;
			localRotation.y = camYaw;
			yawRotator.transform.localRotation = Quaternion.Euler(localRotation);
		}

		public void OnMove(CallbackContext context)
		{
			Vector2 controls = context.ReadValue<Vector2>();
			controlsTarget = new Vector3(controls.x, 0, controls.y);
		}

		public void OnJump(CallbackContext context)
		{
			if (!context.performed || !isGrounded)
				return;

			rigidbody.AddForce(Physics.gravity.normalized * -jumpSpeed, ForceMode.VelocityChange);
		}
	}
}

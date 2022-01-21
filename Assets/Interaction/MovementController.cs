using UnityEngine;
using UnityEngine.InputSystem;
using CallbackContext = UnityEngine.InputSystem.InputAction.CallbackContext;

namespace SBEPIS.Interaction
{
	[RequireComponent(typeof(PlayerInput))]
	[RequireComponent(typeof(CharacterController))]
	public class MovementController : MonoBehaviour
	{
		public Transform pitchRotator;
		public Transform yawRotator;
		public Transform moveAimer;
		public float sensitivity = 1000f;
		public float groundSpeed = 8f;
		public float groundAccelTime = 0.1f;
		public float airSpeed = 60f;
		public float airAccelTime = 0.2f;
		public float jumpHeight = 3f;
		public float gravity = -9.81f;
		public Transform groundCheck;
		public float groundCheckDistance = 0.3f;
		public LayerMask groundCheckMask;

		private CharacterController controller;
		private float camXRot;
		private float yVelocity;
		private Vector2 controlsTarget;
		private Vector2 movement;
		private Vector2 movementAccel;
		private bool isGrounded;

		private void Awake()
		{
			controller = GetComponent<CharacterController>();
		}

		private void Start()
		{
			Cursor.lockState = CursorLockMode.Locked;
		}

		private void Update()
		{
			isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckDistance, groundCheckMask);

			Vector3 movementTarget3 = moveAimer.right * controlsTarget.x + moveAimer.forward * controlsTarget.y;
			Vector2 movementTarget = new Vector2(movementTarget3.x, movementTarget3.z);
			movement = Vector2.SmoothDamp(movement, movementTarget * (isGrounded ? groundSpeed : airSpeed), ref movementAccel, isGrounded ? groundAccelTime : airAccelTime);

			if (isGrounded && yVelocity < 0)
				yVelocity = -2f; // Still go down a bit
			yVelocity += gravity * Time.deltaTime;

			controller.Move(new Vector3(movement.x, yVelocity, movement.y) * Time.deltaTime);
		}

		public void OnLookPitch(CallbackContext context)
		{
			float pitch = context.ReadValue<float>() * sensitivity;
			camXRot = Mathf.Clamp(camXRot - pitch, -90, 90);
			pitchRotator.transform.localRotation = Quaternion.Euler(camXRot, 0, 0);
		}

		public void OnLookYaw(CallbackContext context)
		{
			float yaw = context.ReadValue<float>() * sensitivity;
			yawRotator.Rotate(Vector3.up * yaw);
		}

		public void OnMove(CallbackContext context)
		{
			controlsTarget = context.ReadValue<Vector2>();
		}

		public void OnJump(CallbackContext context)
		{
			if (!context.performed || !isGrounded)
				return;

			yVelocity = Mathf.Sqrt(jumpHeight * -2 * gravity);
		}
	}
}
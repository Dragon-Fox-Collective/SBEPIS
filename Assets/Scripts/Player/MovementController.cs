using UnityEngine;
using UnityEngine.InputSystem;
using WrightWay.SBEPIS.Util;

namespace WrightWay.SBEPIS.Player
{
	[RequireComponent(typeof(PlayerInput))]
	[RequireComponent(typeof(CharacterController))]
	public class MovementController : MonoBehaviour
	{
		public Transform cameraParent;
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

			Vector3 movementTarget3 = transform.right * controlsTarget.x + transform.forward * controlsTarget.y;
			Vector2 movementTarget = new Vector2(movementTarget3.x, movementTarget3.z);
			movement = Vector2.SmoothDamp(movement, movementTarget * (isGrounded ? groundSpeed : airSpeed), ref movementAccel, isGrounded ? groundAccelTime : airAccelTime);

			if (isGrounded && yVelocity < 0)
				yVelocity = -2f; // Still go down a bit
			yVelocity += gravity * Time.deltaTime;

			controller.Move(new Vector3(movement.x, yVelocity, movement.y) * Time.deltaTime);
		}

		private void OnLook(InputValue value)
		{
			Vector2 look = value.Get<Vector2>() * sensitivity;

			transform.Rotate(Vector3.up * look.x);

			camXRot = Mathf.Clamp(camXRot - look.y, -90, 90);
			cameraParent.transform.localRotation = Quaternion.Euler(camXRot, 0, 0);
		}

		private void OnMove(InputValue value)
		{
			controlsTarget = value.Get<Vector2>();
		}

		private void OnJump()
		{
			if (isGrounded)
				yVelocity = Mathf.Sqrt(jumpHeight * -2 * gravity);
		}
	}
}
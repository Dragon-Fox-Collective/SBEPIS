using UnityEngine;
using CallbackContext = UnityEngine.InputSystem.InputAction.CallbackContext;

namespace SBEPIS.Interaction.Controller
{
	[RequireComponent(typeof(Rigidbody), typeof(Orientation))]
	public class JumpController : MonoBehaviour
	{
		public float jumpSpeed = 3;
		public float groundDetectorDelay = 0.5f;

		private new Rigidbody rigidbody;
		private Orientation groundDetector;

		public const float KEY_BUFFER = 0.1f;
		public const float COYOTE_TIME = 0.1f;

		private float jumpBuffer;
		private float coyoteTime;
		private Rigidbody lastGround;

		private void Awake()
		{
			rigidbody = GetComponent<Rigidbody>();
			groundDetector = GetComponent<Orientation>();
		}

		private void FixedUpdate()
		{
			coyoteTime = groundDetector.isGrounded ? COYOTE_TIME : coyoteTime - Time.fixedDeltaTime;

			if (groundDetector.isGrounded)
				lastGround = groundDetector.groundRigidbody;

			if (jumpBuffer > 0)
				jumpBuffer = Jump() ? 0 : jumpBuffer - Time.fixedDeltaTime;
		}

		private bool Jump()
		{
			if (coyoteTime <= 0 || (!groundDetector.isFalling && groundDetector.verticalVelocity.magnitude >= jumpSpeed))
				return false;

			MovementController.AddVelocityAgainstGround(rigidbody, groundDetector.upDirection * jumpSpeed - groundDetector.verticalVelocity, lastGround);

			groundDetector.Delay(groundDetectorDelay);
			coyoteTime = 0;

			return true;
		}

		public void OnJump(CallbackContext context)
		{
			if (!context.performed)
				return;

			if(!Jump())
				jumpBuffer = KEY_BUFFER;
		}
	}
}

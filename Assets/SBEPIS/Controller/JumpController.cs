using UnityEngine;
using CallbackContext = UnityEngine.InputSystem.InputAction.CallbackContext;

namespace SBEPIS.Controller
{
	[RequireComponent(typeof(Orientation))]
	public class JumpController : MonoBehaviour
	{
		public ConfigurableJoint footballJoint;
		public float jumpSpeed = 3;
		public float groundDetectorDelay = 0.5f;
		public Vector3 connectedAnchorTarget;

		private Orientation groundDetector;

		public const float KEY_BUFFER = 0.1f;
		public const float COYOTE_TIME = 0.1f;

		private float jumpBuffer;
		private float coyoteTime;

		private Vector3 initialConnectedAnchor;
		private bool isJumping;

		private void Awake()
		{
			groundDetector = GetComponent<Orientation>();
			initialConnectedAnchor = footballJoint.connectedAnchor;
		}

		private void FixedUpdate()
		{
			coyoteTime = groundDetector.isGrounded ? COYOTE_TIME : coyoteTime - Time.fixedDeltaTime;

			if (jumpBuffer > 0)
				jumpBuffer = Jump() ? 0 : jumpBuffer - Time.fixedDeltaTime;

			if (isJumping)
				MoveFoot();
		}

		private bool Jump()
		{
			if (coyoteTime <= 0 || (!groundDetector.isFalling && groundDetector.verticalVelocity.magnitude >= jumpSpeed))
				return false;

			isJumping = true;

			groundDetector.Delay(groundDetectorDelay);
			coyoteTime = 0;

			return true;
		}

		private void MoveFoot()
		{
			if (footballJoint.connectedAnchor == connectedAnchorTarget)
			{
				isJumping = false;
				footballJoint.connectedAnchor = initialConnectedAnchor;
			}
			else
				footballJoint.connectedAnchor = Vector3.MoveTowards(footballJoint.connectedAnchor, connectedAnchorTarget, jumpSpeed * Time.fixedDeltaTime);
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

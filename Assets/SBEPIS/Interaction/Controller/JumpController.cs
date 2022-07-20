using UnityEngine;
using CallbackContext = UnityEngine.InputSystem.InputAction.CallbackContext;

namespace SBEPIS.Interaction.Controller
{
	[RequireComponent(typeof(Rigidbody), typeof(GroundDetector))]
	public class JumpController : MonoBehaviour
	{
		public float jumpSpeed = 3;
		public float groundDetectorDelay = 0.5f;

		private new Rigidbody rigidbody;
		private GroundDetector groundDetector;

		private void Awake()
		{
			rigidbody = GetComponent<Rigidbody>();
			groundDetector = GetComponent<GroundDetector>();
		}

		private void Jump()
		{
			if (!groundDetector.isGrounded || (!groundDetector.isFalling && groundDetector.verticalVelocity.magnitude >= jumpSpeed))
				return;

			MovementController.AddVelocityAgainstGround(rigidbody, groundDetector.upDirection * jumpSpeed - groundDetector.verticalVelocity, groundDetector);

			groundDetector.Delay(groundDetectorDelay);
		}

		public void OnJump(CallbackContext context)
		{
			if (!context.performed)
				return;

			Jump();
		}
	}
}

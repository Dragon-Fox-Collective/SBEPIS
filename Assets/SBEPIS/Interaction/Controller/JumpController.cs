using UnityEngine;
using CallbackContext = UnityEngine.InputSystem.InputAction.CallbackContext;

namespace SBEPIS.Interaction
{
	[RequireComponent(typeof(Rigidbody), typeof(GroundDetector))]
	public class JumpController : MonoBehaviour
	{
		public float jumpSpeed = 3;

		private new Rigidbody rigidbody;
		private GroundDetector groundDetector;

		private void Awake()
		{
			rigidbody = GetComponent<Rigidbody>();
			groundDetector = GetComponent<GroundDetector>();
		}

		private void Jump()
		{
			rigidbody.AddForce(Physics.gravity.normalized * -jumpSpeed, ForceMode.VelocityChange);
		}

		public void OnJump(CallbackContext context)
		{
			if (!context.performed || !groundDetector.isGrounded)
				return;

			Jump();
		}
	}
}

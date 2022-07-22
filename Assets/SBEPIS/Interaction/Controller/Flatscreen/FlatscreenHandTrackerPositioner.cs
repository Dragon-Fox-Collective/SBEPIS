using UnityEngine;
using UnityEngine.InputSystem;
using CallbackContext = UnityEngine.InputSystem.InputAction.CallbackContext;

namespace SBEPIS.Interaction.Controller.Flatscreen
{
	public class FlatscreenHandTrackerPositioner : MonoBehaviour
	{
		public Rigidbody rightTracker;
		public Grabber rightGrabber;
		public Rigidbody leftTracker;
		public Grabber leftGrabber;
		public LayerMask raycastMask = 1;
		public float raycastDistance = 1;

		public Transform rightHoldPosition;
		public Transform leftHoldPosition;

		public Orientation playerOrientation;

		private bool zoomed;

		private void FixedUpdate()
		{
			if (rightGrabber.heldCollider)
				rightTracker.transform.SetPositionAndRotation(transform.position + transform.forward * raycastDistance, Quaternion.LookRotation(transform.forward, transform.up));
			else if (Cast(out RaycastHit hit))
				rightTracker.transform.SetPositionAndRotation(hit.point, Quaternion.LookRotation(-hit.normal, playerOrientation.upDirection));
			else
				rightTracker.transform.SetPositionAndRotation(rightHoldPosition.position, rightHoldPosition.rotation);

			leftTracker.transform.SetPositionAndRotation(leftHoldPosition.position, leftHoldPosition.rotation);
		}

		public void OnZoom(CallbackContext context)
		{
			if (!rightGrabber.heldCollider || context.ReadValue<float>() > 0)
				zoomed = false;
			else if (context.ReadValue<float>() < 0)
				zoomed = true;
		}

		public bool Cast(out RaycastHit hit)
		{
			return UnityEngine.Physics.Raycast(transform.position, transform.forward, out hit, raycastDistance, raycastMask, QueryTriggerInteraction.Ignore);
		}

		public void OnControlsChanged(PlayerInput input)
		{
			switch (input.currentControlScheme)
			{
				case "OpenXR":
					print("Activating OpenXR input");
					enabled = false;
					break;

				default:
					print($"Activating default ({input.currentControlScheme}) input");
					enabled = true;
					break;
			}
		}
	}
}
using UnityEngine;
using UnityEngine.InputSystem;
using CallbackContext = UnityEngine.InputSystem.InputAction.CallbackContext;

namespace SBEPIS.Interaction.Controller.Flatscreen
{
	public class FlatscreenHandTrackerPositioner : MonoBehaviour
	{
		public Rigidbody tracker;
		public Grabber grabber;
		public LayerMask raycastMask = 1;
		public float raycastDistance = 1;

		public Transform holdPosition;

		private bool zoomed;

		private void FixedUpdate()
		{
			if (grabber.heldGrabbable)
				tracker.transform.position = transform.position + transform.forward * raycastDistance;
			else if (CastForGrabbables(out RaycastHit hit))
				tracker.transform.position = hit.point;
			else
				tracker.transform.SetPositionAndRotation(holdPosition.position, holdPosition.rotation);
		}

		public void OnZoom(CallbackContext context)
		{
			if (!grabber.heldGrabbable || context.ReadValue<float>() > 0)
				zoomed = false;
			else if (context.ReadValue<float>() < 0)
				zoomed = true;
		}

		public bool Cast(out RaycastHit hit, LayerMask mask)
		{
			return UnityEngine.Physics.Raycast(transform.position, transform.forward, out hit, raycastDistance, mask);
		}

		public bool CastForGrabbables(out RaycastHit hit)
		{
			return Cast(out hit, raycastMask) && hit.rigidbody && !hit.collider.isTrigger && hit.rigidbody.GetComponent<Grabbable>();
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
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

		public float farHoldDistance = 2;
		public float nearHoldDistance = 0.7f;

		private bool zoomed;

		private void Update()
		{
			tracker.position = transform.position + transform.forward * (zoomed ? nearHoldDistance : farHoldDistance);

			if (!grabber.heldGrabbable)
				if (CastForGrabbables(out RaycastHit hit))
					grabber.transform.position = hit.point;
				else
					grabber.ClearCollisions();
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
			return UnityEngine.Physics.Raycast(transform.position, transform.forward, out hit, farHoldDistance, mask);
		}

		public bool CastForGrabbables(out RaycastHit hit)
		{
			return Cast(out hit, raycastMask) && hit.rigidbody && !hit.collider.isTrigger;
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
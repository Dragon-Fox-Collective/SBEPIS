using UnityEngine;
using UnityEngine.InputSystem;
using CallbackContext = UnityEngine.InputSystem.InputAction.CallbackContext;

namespace SBEPIS.Interaction.Flatscreen
{
	public class FlatscreenHandTrackerPositioner : MonoBehaviour
	{
		public Rigidbody connectionPoint;
		public Grabber grabber;
		public LayerMask raycastMask = 1;

		public float farHoldDistance = 2;
		public float nearHoldDistance = 0.7f;

		private void Start()
		{
			connectionPoint.position = transform.position + transform.forward * farHoldDistance;
		}

		private void Update()
		{
			if (!grabber.heldGrabbable)
			{
				if (CastForGrabbables(out RaycastHit hit))
				{
					grabber.gameObject.SetActive(true);
					grabber.transform.position = hit.point;
				}
				else
				{
					grabber.gameObject.SetActive(false);
					grabber.ClearCollisions();
				}
			}
		}

		public void OnZoom(CallbackContext context)
		{
			if (!grabber.heldGrabbable || context.ReadValue<float>() > 0)
				connectionPoint.position = transform.position + transform.forward * farHoldDistance;
			else if (context.ReadValue<float>() < 0)
				connectionPoint.position = transform.position + transform.forward * nearHoldDistance;
		}

		public bool Cast(out RaycastHit hit, LayerMask mask)
		{
			return Physics.Raycast(transform.position, transform.forward, out hit, farHoldDistance, mask);
		}

		public bool CastForGrabbables(out RaycastHit hit)
		{
			return Cast(out hit, raycastMask) && hit.rigidbody;
		}

		public void OnControlsChanged(PlayerInput input)
		{
			switch (input.currentControlScheme)
			{
				case "Keyboard":
					print("Activating Keyboard input");
					enabled = true;
					break;

				default:
					enabled = false;
					break;
			}
		}
	}
}
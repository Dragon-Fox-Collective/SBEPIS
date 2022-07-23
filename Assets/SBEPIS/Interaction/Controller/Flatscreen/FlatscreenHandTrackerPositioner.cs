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
		public float raycastDistance = 2;
		public float minimumZoomDistance = 1;
		public float zoomSensitivity = 0.1f;

		public Transform rightHoldPosition;
		public Transform leftHoldPosition;

		public Orientation playerOrientation;

		private float zoomAmount;

		private void Awake()
		{
			zoomAmount = minimumZoomDistance;
		}

		private void FixedUpdate()
		{
			if (rightGrabber.heldCollider)
				rightTracker.transform.SetPositionAndRotation(transform.position + transform.forward * zoomAmount, transform.rotation);
			else if (CastHand(out RaycastHit hit, rightGrabber))
				rightTracker.transform.SetPositionAndRotation(hit.point, Quaternion.LookRotation(-hit.normal, playerOrientation.upDirection));
			else if (CastShortRangeGrab(out _, rightGrabber))
				rightTracker.transform.SetPositionAndRotation(transform.position + transform.forward * (raycastDistance - rightGrabber.shortRangeGrabDistace), transform.rotation);
			else
				rightTracker.transform.SetPositionAndRotation(rightHoldPosition.position, rightHoldPosition.rotation);

			leftTracker.transform.SetPositionAndRotation(leftHoldPosition.position, leftHoldPosition.rotation);
		}

		public void OnZoom(CallbackContext context)
		{
			zoomAmount = Mathf.Clamp(zoomAmount + context.ReadValue<float>() * zoomSensitivity, minimumZoomDistance, raycastDistance);
		}

		private bool CastHand(out RaycastHit hit, Grabber grabber)
		{
			return grabber.canShortRangeGrab = UnityEngine.Physics.Raycast(transform.position, transform.forward, out hit, raycastDistance - grabber.shortRangeGrabDistace, raycastMask, QueryTriggerInteraction.Ignore);
		}

		private bool CastShortRangeGrab(out RaycastHit hit, Grabber grabber)
		{
			return grabber.canShortRangeGrab = UnityEngine.Physics.Raycast(transform.position, transform.forward, out hit, raycastDistance, grabber.shortRangeGrabMask, QueryTriggerInteraction.Ignore);
		}

		public void OnControlsChanged(PlayerInput input)
		{
			switch (input.currentControlScheme)
			{
				case "OpenXR":
					print("Activating OpenXR input");
					enabled = false;
					rightGrabber.OverrideShortRangeGrab(null, 0);
					leftGrabber.OverrideShortRangeGrab(null, 0);
					break;

				default:
					print($"Activating default ({input.currentControlScheme}) input");
					enabled = true;
					rightGrabber.OverrideShortRangeGrab(transform, raycastDistance);
					leftGrabber.OverrideShortRangeGrab(transform, raycastDistance);
					break;
			}
		}
	}
}
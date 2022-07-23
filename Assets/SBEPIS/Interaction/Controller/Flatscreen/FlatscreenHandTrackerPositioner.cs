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
		public float bothHandsOffset = 0.5f;
		public float raycastDistance = 2;
		public float minimumZoomDistance = 1;
		public float zoomSensitivity = 0.1f;

		public Transform rightHoldPosition;
		public Transform leftHoldPosition;

		public Orientation playerOrientation;

		private float zoomAmount;
		private bool useLeftHand;

		private void Awake()
		{
			zoomAmount = minimumZoomDistance;
		}

		private void FixedUpdate()
		{
			UpdateHand(rightTracker, rightGrabber, rightHoldPosition, useLeftHand ? bothHandsOffset : 0);

			if (useLeftHand)
				UpdateHand(leftTracker, leftGrabber, leftHoldPosition, useLeftHand ? -bothHandsOffset : 0);
			else
			{
				leftTracker.transform.SetPositionAndRotation(leftHoldPosition.position, leftHoldPosition.rotation);
				leftGrabber.canShortRangeGrab = false;
			}
		}

		private void UpdateHand(Rigidbody tracker, Grabber hand, Transform emptyHoldPosition, float offset)
		{
			UpdateHand(
				tracker,
				hand,
				emptyHoldPosition,
				transform.position + transform.forward * zoomAmount + transform.right * offset,
				transform.position + transform.forward * (raycastDistance - rightGrabber.shortRangeGrabDistace) + transform.right * offset,
				transform.rotation,
				transform.position + transform.right * offset,
				transform.forward,
				playerOrientation.upDirection,
				raycastDistance);
		}

		private static void UpdateHand(
			Rigidbody tracker,
			Grabber grabber,
			Transform emptyHoldPosition,
			Vector3 fullHoldPosition,
			Vector3 shortRangeTargetPosition,
			Quaternion rotation,
			Vector3 casterPosition,
			Vector3 casterForward,
			Vector3 up,
			float raycastDistance)
		{
			grabber.OverrideShortRangeGrab(casterPosition, casterForward, raycastDistance);

			if (grabber.heldCollider)
				tracker.transform.SetPositionAndRotation(fullHoldPosition, rotation);
			else if (CastHand(out RaycastHit hit, casterPosition, casterForward, raycastDistance, grabber))
				tracker.transform.SetPositionAndRotation(hit.point, Quaternion.LookRotation(-hit.normal, up));
			else if (CastShortRangeGrab(out _, casterPosition, casterForward, raycastDistance, grabber))
				tracker.transform.SetPositionAndRotation(shortRangeTargetPosition, rotation);
			else
				tracker.transform.SetPositionAndRotation(emptyHoldPosition.position, emptyHoldPosition.rotation);
		}

		public void OnZoom(CallbackContext context)
		{
			zoomAmount = Mathf.Clamp(zoomAmount + context.ReadValue<float>() * zoomSensitivity, minimumZoomDistance, raycastDistance);
		}

		public void OnUseLeftHand(CallbackContext context)
		{
			useLeftHand = context.performed;
		}

		private static bool CastHand(out RaycastHit hit, Vector3 casterPosition, Vector3 casterForward, float raycastDistance, Grabber grabber)
		{
			return grabber.canShortRangeGrab = UnityEngine.Physics.Raycast(casterPosition, casterForward, out hit, raycastDistance - grabber.shortRangeGrabDistace, grabber.grabMask, QueryTriggerInteraction.Ignore);
		}

		private static bool CastShortRangeGrab(out RaycastHit hit, Vector3 casterPosition, Vector3 casterForward, float raycastDistance, Grabber grabber)
		{
			return grabber.canShortRangeGrab = UnityEngine.Physics.Raycast(casterPosition, casterForward, out hit, raycastDistance, grabber.grabMask, QueryTriggerInteraction.Ignore);
		}

		public void OnControlsChanged(PlayerInput input)
		{
			switch (input.currentControlScheme)
			{
				case "OpenXR":
					print("Activating OpenXR input");
					enabled = false;
					rightGrabber.ResetShortRangeGrabOverride();
					leftGrabber.ResetShortRangeGrabOverride();
					break;

				default:
					print($"Activating default ({input.currentControlScheme}) input");
					enabled = true;
					break;
			}
		}
	}
}
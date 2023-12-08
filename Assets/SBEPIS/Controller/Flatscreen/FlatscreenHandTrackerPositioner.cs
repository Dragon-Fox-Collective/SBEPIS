using KBCore.Refs;
using UnityEngine;

namespace SBEPIS.Controller.Flatscreen
{
	public class FlatscreenHandTrackerPositioner : ValidatedMonoBehaviour
	{
		[SerializeField, Anywhere]
		private Transform rightTracker;
		[SerializeField, Anywhere]
		private Grabber rightGrabber;
		[SerializeField, Anywhere]
		private Transform leftTracker;
		[SerializeField, Anywhere]
		private Grabber leftGrabber;
		[SerializeField]
		private float bothHandsOffset = 0.5f;
		[SerializeField]
		private float raycastDistance = 2;
		[SerializeField]
		private float minimumZoomDistance = 1;
		[SerializeField]
		private float startingZoomDistance = 1;
		[SerializeField]
		private float zoomSensitivity = 0.1f;
		
		[SerializeField, Anywhere]
		private Transform rightHoldPosition;
		[SerializeField, Anywhere]
		private Transform leftHoldPosition;
		
		[SerializeField, Anywhere]
		private Orientation playerOrientation;
		
		private float zoomAmount;
		public bool UseLeftHand { private get; set; }
		
		private void Awake()
		{
			zoomAmount = startingZoomDistance;
		}
		
		private void FixedUpdate()
		{
			UpdateHand(rightTracker, rightGrabber, rightHoldPosition, UseLeftHand ? bothHandsOffset : 0);
			
			if (UseLeftHand)
				UpdateHand(leftTracker, leftGrabber, leftHoldPosition, UseLeftHand ? -bothHandsOffset : 0);
			else
			{
				leftTracker.transform.SetPositionAndRotation(leftHoldPosition.position, leftHoldPosition.rotation);
				leftGrabber.CanGrab = false;
			}
		}
		
		private void UpdateHand(Transform tracker, Grabber hand, Transform emptyHoldPosition, float offset)
		{
			UpdateHand(
				tracker,
				hand,
				emptyHoldPosition,
				hand.HeldGrabPoint && hand.HeldGrabPoint.flatscreenTarget ?
					transform.TransformPoint(Vector3.forward * zoomAmount + Vector3.right * offset + hand.HeldGrabPoint.flatscreenTarget.InverseTransformPoint(hand.HeldGrabPoint.transform.position)) :
					transform.TransformPoint(Vector3.forward * zoomAmount + Vector3.right * offset),
				hand.HeldGrabPoint && hand.HeldGrabPoint.flatscreenTarget ?
					transform.TransformRotation(hand.HeldGrabPoint.flatscreenTarget.InverseTransformRotation(hand.HeldGrabPoint.transform.rotation)) :
					transform.rotation,
				transform.position + transform.forward * (raycastDistance - rightGrabber.ShortRangeGrabDistance) + transform.right * offset,
				transform.rotation,
				transform.position + transform.right * offset,
				transform.forward,
				playerOrientation.UpDirection,
				raycastDistance);
		}
		
		private static void UpdateHand(
			Transform tracker,
			Grabber grabber,
			Transform emptyHoldPosition,
			Vector3 fullHoldPosition,
			Quaternion fullHoldRotation,
			Vector3 shortRangeTargetPosition,
			Quaternion rotation,
			Vector3 casterPosition,
			Vector3 casterForward,
			Vector3 up,
			float raycastDistance)
		{
			grabber.OverrideShortRangeGrab(casterPosition, casterForward, raycastDistance);
			
			if (grabber.IsHoldingSomething)
				tracker.SetPositionAndRotation(fullHoldPosition, fullHoldRotation);
			else if (CastHand(out RaycastHit hit, casterPosition, casterForward, raycastDistance, grabber))
				tracker.SetPositionAndRotation(hit.point, Quaternion.LookRotation(-hit.normal, up));
			else if (CastShortRangeGrab(out _, casterPosition, casterForward, raycastDistance, grabber))
				tracker.SetPositionAndRotation(shortRangeTargetPosition, rotation);
			else
				tracker.SetPositionAndRotation(emptyHoldPosition.position, emptyHoldPosition.rotation);
		}
		
		public void Zoom(float amount)
		{
			zoomAmount = Mathf.Clamp(zoomAmount + amount * zoomSensitivity, minimumZoomDistance, raycastDistance);
		}
		
		private static bool CastHand(out RaycastHit hit, Vector3 casterPosition, Vector3 casterForward, float raycastDistance, Grabber grabber)
		{
			return grabber.CanGrab = UnityEngine.Physics.Raycast(casterPosition, casterForward, out hit, raycastDistance - grabber.ShortRangeGrabDistance, grabber.GrabMask, QueryTriggerInteraction.Ignore);
		}
		
		private static bool CastShortRangeGrab(out RaycastHit hit, Vector3 casterPosition, Vector3 casterForward, float raycastDistance, Grabber grabber)
		{
			return grabber.CanGrab = UnityEngine.Physics.Raycast(casterPosition, casterForward, out hit, raycastDistance, grabber.GrabMask, QueryTriggerInteraction.Ignore);
		}
		
		private void OnDisable()
		{
			rightGrabber.ResetShortRangeGrabOverride();
			leftGrabber.ResetShortRangeGrabOverride();
		}
	}
}
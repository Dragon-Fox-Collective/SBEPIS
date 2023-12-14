using KBCore.Refs;
using UnityEngine;
using UnityEngine.Serialization;

namespace SBEPIS.Controller.Flatscreen
{
	public class FlatscreenHandTrackerPositioner : ValidatedMonoBehaviour
	{
		[SerializeField, Anywhere]
		private Transform rightTracker;
		[SerializeField, Anywhere]
		private Grabber rightGrabber;
		[SerializeField, Anywhere]
		private Transform rightShoulder;
		[SerializeField, Anywhere]
		private Transform rightIndicator;
		[SerializeField, Anywhere]
		private Transform leftTracker;
		[SerializeField, Anywhere]
		private Grabber leftGrabber;
		[SerializeField, Anywhere]
		private Transform leftShoulder;
		[SerializeField, Anywhere]
		private Transform leftIndicator;
		[SerializeField]
		private float armLength = 1f;
		[SerializeField]
		private float raycastDistance = 2;
		[SerializeField]
		private float minimumZoomDistance = 1;
		[SerializeField]
		private float startingZoomDistance = 1;
		[SerializeField]
		private float zoomSensitivity = 0.1f;
		
		[FormerlySerializedAs("rightHoldPosition")]
		[SerializeField, Anywhere]
		private Transform rightEmptyHoldPosition;
		[FormerlySerializedAs("leftHoldPosition")]
		[SerializeField, Anywhere]
		private Transform leftEmptyHoldPosition;
		
		private float zoomAmount;

		private void Awake()
		{
			zoomAmount = startingZoomDistance;
		}

		private void FixedUpdate()
		{
			if (leftGrabber.IsHoldingSomething && rightGrabber.IsHoldingSomething)
				UpdateHoldingBothHands();
			else if (leftGrabber.IsHoldingSomething)
				UpdateHoldingLeftHand();
			else if (rightGrabber.IsHoldingSomething)
				UpdateHoldingRightHand();
			else
				UpdateHoldingNothing();
		}

		private void UpdateHoldingBothHands()
		{
			rightIndicator.gameObject.SetActive(false);
			leftIndicator.gameObject.SetActive(false);
			float distanceBetweenGrabbers = Vector3.Distance(leftGrabber.HeldGrabPoint.transform.position, rightGrabber.HeldGrabPoint.transform.position);
			leftTracker.SetPositionAndRotation(
				transform.TransformPoint(Vector3.forward * zoomAmount + Vector3.right * distanceBetweenGrabbers / 2),
				transform.rotation
			);
			rightTracker.SetPositionAndRotation(
				transform.TransformPoint(Vector3.forward * zoomAmount + Vector3.left * distanceBetweenGrabbers / 2),
				transform.rotation
			);
		}
		
		private void UpdateHoldingLeftHand()
		{
			rightGrabber.CanGrab = false;
			rightIndicator.gameObject.SetActive(false);
			leftIndicator.gameObject.SetActive(false);
			leftTracker.SetPositionAndRotation(
				transform.TransformPoint(Vector3.forward * zoomAmount),
				transform.rotation
			);
			rightTracker.SetPositionAndRotation(rightEmptyHoldPosition);
		}
		
		private void UpdateHoldingRightHand()
		{
			leftGrabber.CanGrab = false;
			rightIndicator.gameObject.SetActive(false);
			leftIndicator.gameObject.SetActive(false);
			leftTracker.SetPositionAndRotation(leftEmptyHoldPosition);
			rightTracker.SetPositionAndRotation(
				transform.TransformPoint(Vector3.forward * zoomAmount),
				transform.rotation
			);
		}
		
		private void UpdateHoldingNothing()
		{
			// Assumes rightGrabber.GrabMask == leftGrabber.GrabMask
			if (UnityEngine.Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, raycastDistance, rightGrabber.GrabMask, QueryTriggerInteraction.Ignore))
				UpdateHoldingNothingLookingAtObject(hit);
			else
				UpdateHoldingNothingLookingAtNothing();
		}
		
		private void UpdateHoldingNothingLookingAtObject(RaycastHit hit)
		{
			leftGrabber.CanGrab = false;
			rightGrabber.CanGrab = false;
			rightIndicator.gameObject.SetActive(true);
			leftIndicator.gameObject.SetActive(true);
			leftTracker.SetPositionAndRotation(leftEmptyHoldPosition);
			rightTracker.SetPositionAndRotation(rightEmptyHoldPosition);
			
			UpdateIndicatorAndGrabberOverrides(hit, leftShoulder, leftIndicator, -transform.right * armLength);
			UpdateIndicatorAndGrabberOverrides(hit, rightShoulder, rightIndicator, transform.right * armLength);
		}

		private void UpdateIndicatorAndGrabberOverrides(
			RaycastHit lookHit,
			Transform shoulder,
			Transform indicator,
			Vector3 armOffset
		)
		{
			Vector3 startingPoint = shoulder.position + armOffset;
			(Collider closestCollider, Vector3 closestPoint) = lookHit.rigidbody ? lookHit.rigidbody.ClosestPointPlusConcaveMesh(startingPoint) : (lookHit.collider, lookHit.collider.ClosestPointPlusConcaveMesh(startingPoint));
			Vector3 targetDirection = closestPoint - startingPoint;
			Debug.DrawRay(startingPoint, targetDirection.normalized * (targetDirection.magnitude + 1f), Color.red);
			if (closestCollider.Raycast(new Ray(startingPoint, targetDirection), out RaycastHit handHit, targetDirection.magnitude + 1f))
				indicator.SetPositionAndRotation(handHit.point, Vector3.Angle(handHit.normal, transform.up) > 5f ? Quaternion.LookRotation(-handHit.normal, transform.up) : Quaternion.LookRotation(-handHit.normal, transform.forward));
			else
				Debug.LogError($"Hand didn't hit {(lookHit.rigidbody ? lookHit.rigidbody.gameObject : lookHit.collider.gameObject)}");
		}
		
		private void UpdateHoldingNothingLookingAtNothing()
		{
			leftGrabber.CanGrab = false;
			rightGrabber.CanGrab = false;
			rightIndicator.gameObject.SetActive(false);
			leftIndicator.gameObject.SetActive(false);
			leftTracker.SetPositionAndRotation(leftEmptyHoldPosition);
			rightTracker.SetPositionAndRotation(rightEmptyHoldPosition);
		}

		// private void FixedUpdate()
		// {
		// 	UpdateHand(rightTracker, rightGrabber, rightEmptyHoldPosition, UseLeftHand ? bothHandsOffset : 0);
		// 	
		// 	if (UseLeftHand)
		// 		UpdateHand(leftTracker, leftGrabber, leftEmptyHoldPosition, UseLeftHand ? -bothHandsOffset : 0);
		// 	else
		// 	{
		// 		leftTracker.transform.SetPositionAndRotation(leftEmptyHoldPosition.position, leftEmptyHoldPosition.rotation);
		// 		leftGrabber.CanGrab = false;
		// 	}
		// }
		//
		// private void UpdateHand(Transform tracker, Grabber hand, Transform emptyHoldPosition, float offset)
		// {
		// 	UpdateHand(
		// 		tracker,
		// 		hand,
		// 		emptyHoldPosition,
		// 		hand.HeldGrabPoint && hand.HeldGrabPoint.flatscreenTarget ?
		// 			transform.TransformPoint(Vector3.forward * zoomAmount + Vector3.right * offset + hand.HeldGrabPoint.flatscreenTarget.InverseTransformPoint(hand.HeldGrabPoint.transform.position)) :
		// 			transform.TransformPoint(Vector3.forward * zoomAmount + Vector3.right * offset),
		// 		hand.HeldGrabPoint && hand.HeldGrabPoint.flatscreenTarget ?
		// 			transform.TransformRotation(hand.HeldGrabPoint.flatscreenTarget.InverseTransformRotation(hand.HeldGrabPoint.transform.rotation)) :
		// 			transform.rotation,
		// 		transform.position + transform.forward * (raycastDistance - rightGrabber.ShortRangeGrabDistance) + transform.right * offset,
		// 		transform.rotation,
		// 		transform.position + transform.right * offset,
		// 		transform.forward,
		// 		playerOrientation.UpDirection,
		// 		raycastDistance);
		// }
		//
		// private static void UpdateHand(
		// 	Transform tracker,
		// 	Grabber grabber,
		// 	Transform emptyHoldPosition,
		// 	Vector3 fullHoldPosition,
		// 	Quaternion fullHoldRotation,
		// 	Vector3 shortRangeTargetPosition,
		// 	Quaternion rotation,
		// 	Vector3 casterPosition,
		// 	Vector3 casterForward,
		// 	Vector3 up,
		// 	float raycastDistance)
		// {
		// 	grabber.OverrideShortRangeGrab(casterPosition, casterForward, raycastDistance);
		// 	
		// 	if (grabber.IsHoldingSomething)
		// 		tracker.SetPositionAndRotation(fullHoldPosition, fullHoldRotation);
		// 	else if (CastHand(out RaycastHit hit, casterPosition, casterForward, raycastDistance, grabber))
		// 		tracker.SetPositionAndRotation(hit.point, Quaternion.LookRotation(-hit.normal, up));
		// 	else if (CastShortRangeGrab(out _, casterPosition, casterForward, raycastDistance, grabber))
		// 		tracker.SetPositionAndRotation(shortRangeTargetPosition, rotation);
		// 	else
		// 		tracker.SetPositionAndRotation(emptyHoldPosition.position, emptyHoldPosition.rotation);
		// }
		
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
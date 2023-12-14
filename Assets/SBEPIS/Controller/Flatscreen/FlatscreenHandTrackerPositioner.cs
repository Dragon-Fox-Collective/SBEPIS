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

		[SerializeField, Anywhere]
		private SphereCollider dummySphere;
		
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
			(Collider closestCollider, Vector3 closestPoint) = lookHit.rigidbody ? lookHit.rigidbody.ClosestPointPlusConcaveMesh(startingPoint, dummySphere) : (lookHit.collider, lookHit.collider.ClosestPointPlusConcaveMesh(startingPoint, dummySphere));
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
		
		public void Zoom(float amount)
		{
			zoomAmount = Mathf.Clamp(zoomAmount + amount * zoomSensitivity, minimumZoomDistance, raycastDistance);
		}
		
		private void OnDisable()
		{
			rightGrabber.ResetShortRangeGrabOverride();
			leftGrabber.ResetShortRangeGrabOverride();
		}
	}
}
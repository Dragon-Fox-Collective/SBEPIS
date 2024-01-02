using KBCore.Refs;
using UnityEngine;
using UnityEngine.Serialization;

namespace SBEPIS.Controller.Flatscreen
{
	public class FlatscreenHandTrackerPositioner : ValidatedMonoBehaviour
	{
		[SerializeField, Anywhere] private Transform rightTracker;
		[SerializeField, Anywhere] private Grabber rightGrabber;
		[SerializeField, Anywhere] private Transform rightShoulder;
		[SerializeField, Anywhere] private Transform rightIndicator;
		[SerializeField, Anywhere] private Transform leftTracker;
		[SerializeField, Anywhere] private Grabber leftGrabber;
		[SerializeField, Anywhere] private Transform leftShoulder;
		[SerializeField, Anywhere] private Transform leftIndicator;
		[SerializeField] private float armLength = 1f;
		[SerializeField] private float raycastDistance = 2;
		[SerializeField] private float minimumZoomDistance = 1;
		[SerializeField] private float startingZoomDistance = 1;
		[SerializeField] private float zoomSensitivity = 0.1f;
		[SerializeField] private float sphereCastRadius = 0.1f;
		[SerializeField] private float rotationSensitivity = 0.1f;
		
		[FormerlySerializedAs("rightHoldPosition")]
		[SerializeField, Anywhere] private Transform rightEmptyHoldPosition;
		[FormerlySerializedAs("leftHoldPosition")]
		[SerializeField, Anywhere] private Transform leftEmptyHoldPosition;
		
		public Vector2 HoldRotationDrive { private get; set; }
		private Vector2 holdRotationDelta;
		public Vector2 HoldRotationDelta
		{
			set => holdRotationDelta += value;
		}
		private Quaternion holdRotation;
		private Vector2 TotalDeltaHoldRotation => holdRotationDelta * rotationSensitivity + HoldRotationDrive * rotationSensitivity * Time.fixedDeltaTime;

		private Vector3 RightFullHoldPosition => rightGrabber.HeldGrabPoint && rightGrabber.HeldGrabPoint.flatscreenTarget
			? transform.TransformPoint(Vector3.forward * zoomAmount + rightGrabber.HeldGrabPoint.flatscreenTarget.InverseTransformPoint(rightGrabber.HeldGrabPoint.transform.position))
			: transform.TransformPoint(Vector3.forward * zoomAmount);
		private Quaternion RightFullHoldRotation => rightGrabber.HeldGrabPoint && rightGrabber.HeldGrabPoint.flatscreenTarget
			? transform.rotation * holdRotation * rightGrabber.HeldGrabPoint.flatscreenTarget.InverseTransformRotation(rightGrabber.HeldGrabPoint.transform.rotation)
			: transform.rotation * holdRotation;
		private Vector3 LeftFullHoldPosition => leftGrabber.HeldGrabPoint && leftGrabber.HeldGrabPoint.flatscreenTarget
			? transform.TransformPoint(Vector3.forward * zoomAmount + leftGrabber.HeldGrabPoint.flatscreenTarget.InverseTransformPoint(leftGrabber.HeldGrabPoint.transform.position))
			: transform.TransformPoint(Vector3.forward * zoomAmount);
		private Quaternion LeftFullHoldRotation => leftGrabber.HeldGrabPoint && leftGrabber.HeldGrabPoint.flatscreenTarget
			? transform.rotation * holdRotation * leftGrabber.HeldGrabPoint.flatscreenTarget.InverseTransformRotation(leftGrabber.HeldGrabPoint.transform.rotation)
			: transform.rotation * holdRotation;
		
		private float zoomAmount;
		
		private void Awake()
		{
			zoomAmount = startingZoomDistance;
		}

		private void FixedUpdate()
		{
			leftGrabber.ResetGrabCompleteOverride();
			rightGrabber.ResetGrabCompleteOverride();

			holdRotation = Quaternion.Euler(TotalDeltaHoldRotation.y, TotalDeltaHoldRotation.x, 0) * holdRotation;
			holdRotationDelta = Vector2.zero;
			
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
			float distanceBetweenGrabbers = Vector3.Distance(leftGrabber.transform.position, rightGrabber.transform.position);
			leftTracker.SetPositionAndRotation(
				transform.TransformPoint(transform.InverseTransformPoint(LeftFullHoldPosition) + holdRotation * Vector3.left * distanceBetweenGrabbers / 2),
				LeftFullHoldRotation
			);
			rightTracker.SetPositionAndRotation(
				transform.TransformPoint(transform.InverseTransformPoint(RightFullHoldPosition) + holdRotation * Vector3.right * distanceBetweenGrabbers / 2),
				RightFullHoldRotation
			);
		}
		
		private void UpdateHoldingLeftHand()
		{
			rightGrabber.CanGrab = false;
			rightIndicator.gameObject.SetActive(false);
			leftIndicator.gameObject.SetActive(false);
			leftTracker.SetPositionAndRotation(LeftFullHoldPosition, LeftFullHoldRotation);
			rightTracker.SetPositionAndRotation(rightEmptyHoldPosition);
		}
		
		private void UpdateHoldingRightHand()
		{
			leftGrabber.CanGrab = false;
			rightIndicator.gameObject.SetActive(false);
			leftIndicator.gameObject.SetActive(false);
			leftTracker.SetPositionAndRotation(leftEmptyHoldPosition);
			rightTracker.SetPositionAndRotation(RightFullHoldPosition, RightFullHoldRotation);
		}
		
		private void UpdateHoldingNothing()
		{
			holdRotation = Quaternion.identity;
			// Assumes rightGrabber.GrabMask == leftGrabber.GrabMask
			if (UnityEngine.Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, raycastDistance, rightGrabber.GrabMask, QueryTriggerInteraction.Ignore))
				UpdateHoldingNothingLookingAtObject(hit);
			else
				UpdateHoldingNothingLookingAtNothing();
		}
		
		private void UpdateHoldingNothingLookingAtObject(RaycastHit hit)
		{
			leftTracker.SetPositionAndRotation(leftEmptyHoldPosition);
			rightTracker.SetPositionAndRotation(rightEmptyHoldPosition);
			
			bool leftGrabbed = UpdateIndicatorAndGrabberOverrides(hit, leftGrabber, leftShoulder, leftIndicator, -transform.right * armLength);
			bool rightGrabbed = UpdateIndicatorAndGrabberOverrides(hit, rightGrabber, rightShoulder, rightIndicator, transform.right * armLength);
			
			if (leftGrabbed && !rightGrabbed)
				UpdateIndicatorAndGrabberOverridesSuccess(hit, leftGrabber, leftIndicator);
			else if (!leftGrabbed)
				UpdateIndicatorAndGrabberOverridesSuccess(hit, rightGrabber, rightIndicator);
		}

		// ReSharper disable Unity.PerformanceAnalysis
		private bool UpdateIndicatorAndGrabberOverrides(
			RaycastHit lookHit,
			Grabber grabber,
			Transform shoulder,
			Transform indicator,
			Vector3 armOffset
		)
		{
			Vector3 startingPoint = shoulder.position + armOffset;
			Vector3 targetDirection = lookHit.point - startingPoint;
			if (UnityEngine.Physics.SphereCast(startingPoint, sphereCastRadius, targetDirection, out RaycastHit handHit, targetDirection.magnitude + 1f))
			{
				if ((lookHit.rigidbody && handHit.rigidbody != lookHit.rigidbody) || (!lookHit.rigidbody && handHit.collider != lookHit.collider))
				{
					UpdateIndicatorAndGrabberOverridesFail(grabber, indicator);
					return false;
				}
				
				UpdateIndicatorAndGrabberOverridesSuccess(handHit, grabber, indicator);
				return true;
			}
			else // This happens when the shoulder is inside the object
			{
				UpdateIndicatorAndGrabberOverridesFail(grabber, indicator);
				//Debug.LogError($"Hand didn't hit {(lookHit.rigidbody ? lookHit.rigidbody.gameObject : lookHit.collider.gameObject)}");
				return false;
			}
		}
		
		private void UpdateHoldingNothingLookingAtNothing()
		{
			UpdateIndicatorAndGrabberOverridesFail(leftGrabber, leftIndicator);
			UpdateIndicatorAndGrabberOverridesFail(rightGrabber, rightIndicator);
			leftTracker.SetPositionAndRotation(leftEmptyHoldPosition);
			rightTracker.SetPositionAndRotation(rightEmptyHoldPosition);
		}
		
		private void UpdateIndicatorAndGrabberOverridesSuccess(
			RaycastHit hit,
			Grabber grabber,
			Transform indicator
		)
		{
			Vector3 position = hit.point;
			Quaternion rotation = Vector3.Angle(hit.normal, transform.up) > 5f ? Quaternion.LookRotation(-hit.normal, transform.up) : Quaternion.LookRotation(-hit.normal, transform.forward);
			indicator.gameObject.SetActive(true);
			indicator.SetPositionAndRotation(position, rotation);
			grabber.CanGrab = true;
			grabber.OverrideGrabCompletely(hit.collider, position, rotation);
		}
		
		private static void UpdateIndicatorAndGrabberOverridesFail(
			Grabber grabber,
			Transform indicator
		)
		{
			indicator.gameObject.SetActive(false);
			grabber.CanGrab = false;
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
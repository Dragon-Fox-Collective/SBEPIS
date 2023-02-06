using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using CallbackContext = UnityEngine.InputSystem.InputAction.CallbackContext;

namespace SBEPIS.Controller
{
	[RequireComponent(typeof(Rigidbody))]
	public class Grabber : MonoBehaviour
	{
		public LayerMask grabMask = 1;
		[Tooltip("Should be within the hand collision box, far enough that you wouldn't reasonably be able to clip into it")]
		public Transform grabNormalCaster;
		[Tooltip("Angle between the terrain's normal and player's up past which the grabber slips off")]
		public float slipAngle = 80;
		public float shortRangeGrabDistace = 1;

		public Orientation playerOrientation;

		public bool canGrab { get; set; } = true;

		private bool overrideShortRangeGrab;
		private Vector3 overrideShortRangeGrabCasterPosition;
		private Vector3 overrideShortRangeGrabCasterForward;
		private float overrideShortRangeGrabDistance;

		public bool isHoldingSomething => heldCollider;
		public Collider heldCollider { get; private set; }
		public Grabbable heldGrabbable { get; private set; }
		public GrabPoint heldGrabPoint { get; private set; }
		private FixedJoint heldGrabbableJoint;
		private Vector3 heldPureColliderNormal;

		private readonly RaycastHit[] grabNormalHits = new RaycastHit[16];

		public new Rigidbody rigidbody { get; private set; }

		private readonly List<Collider> collidingColliders = new();

		private bool isHoldingGrab;

		private bool isSlipping => heldPureColliderNormal != Vector3.zero && Vector3.Angle(heldPureColliderNormal, playerOrientation.upDirection) > slipAngle;

		private void Awake()
		{
			rigidbody = GetComponent<Rigidbody>();
		}

		private void Update()
		{
			ClearInvalidCollisions();
			UpdateGrabAttempt();
			if (heldGrabbable)
				heldGrabbable.HoldUpdate(this);
		}

		public void OnGrab(CallbackContext context)
		{
			isHoldingGrab = context.performed;
			UpdateGrabAttempt();
		}

		public void UpdateGrabAttempt()
		{
			if (!isActiveAndEnabled)
				return;

			if (isHoldingGrab)
				if (isHoldingSomething && (!heldCollider.gameObject.activeInHierarchy || isSlipping))
					Drop();
				else
					Grab();
			else
				Drop();
		}

		public void Grab()
		{
			if (!canGrab || isHoldingSomething)
				return;
			
			if (collidingColliders.Any(collidingCollider => Grab(collidingCollider)))
				return;
			
			ShortRangeGrab();
		}

		public bool Grab(Collider collider)
		{
			if (!collider || isHoldingSomething)
				return false;
			
			Grabbable grabbable = collider.GetAttachedComponent<Grabbable>();
			if (grabbable)
				return Grab(grabbable);

			if (CastGrabNormal(out RaycastHit hit, collider))
			{
				heldPureColliderNormal = hit.normal;
				if (Vector3.Angle(heldPureColliderNormal, playerOrientation.upDirection) > slipAngle)
					return false;
			}
			else
				heldPureColliderNormal = Vector3.zero;

			heldCollider = collider;

			heldGrabbableJoint = gameObject.AddComponent<FixedJoint>();
			if (collider.attachedRigidbody)
				heldGrabbableJoint.connectedBody = collider.attachedRigidbody;

			return true;
		}

		public bool Grab(Grabbable grabbable)
		{
			if (!grabbable || isHoldingSomething || !grabbable.canGrab)
				return false;
			
			print($"Grabbing {grabbable}");

			heldPureColliderNormal = Vector3.zero;
			heldGrabbable = grabbable;

			if (grabbable.grabPoints.Count > 0)
				BindToGrabPoint(grabbable, grabbable.grabPoints[0]);
			else
				heldCollider = grabbable.rigidbody.GetComponentInChildren<Collider>();

			heldGrabbableJoint = gameObject.AddComponent<FixedJoint>();
			heldGrabbableJoint.connectedBody = grabbable.rigidbody;
			heldGrabbableJoint.autoConfigureConnectedAnchor = false;
			heldGrabbableJoint.anchor = transform.InverseTransformPoint(grabbable.transform.position);
			heldGrabbableJoint.connectedAnchor = Vector3.zero;

			grabbable.GetGrabbed(this);

			return true;
		}

		private void BindToGrabPoint(Grabbable grabbable, GrabPoint grabPoint)
		{
			heldGrabPoint = grabPoint;
			heldCollider = grabPoint.colliderToGrab;
			grabbable.transform.SetPositionAndRotation(
				transform.TransformPoint(grabPoint.transform.InverseTransformPoint(grabbable.transform.position)),
				transform.TransformRotation(grabPoint.transform.InverseTransformRotation(grabbable.transform.rotation)));
		}

		private void ShortRangeGrab()
		{
			if (CastShortRangeGrab(out RaycastHit hit))
			{
				transform.SetPositionAndRotation(hit.point, Quaternion.LookRotation(-hit.normal, transform.up));
				Grab(hit.collider);
			}
		}

		public void Drop()
		{
			if (!isHoldingSomething)
				return;
			
			print($"Dropping {(heldGrabbable ? heldGrabbable : heldCollider)}");

			Collider droppedCollider = heldCollider;
			Grabbable droppedGrabbable = heldGrabbable;
			heldCollider = null;
			heldGrabbable = null;
			heldGrabPoint = null;
			
			Destroy(heldGrabbableJoint);
			heldGrabbableJoint = null;

			if (droppedCollider.attachedRigidbody)
				droppedCollider.attachedRigidbody.AddForce(Vector3.up * 0.01f);
			if (droppedGrabbable)
				droppedGrabbable.GetDropped(this);
		}

		private bool CastShortRangeGrab(out RaycastHit hit)
		{
			return overrideShortRangeGrab ?
				UnityEngine.Physics.Raycast(overrideShortRangeGrabCasterPosition, overrideShortRangeGrabCasterForward, out hit, overrideShortRangeGrabDistance, grabMask, QueryTriggerInteraction.Ignore) :
				UnityEngine.Physics.Raycast(transform.position, transform.forward, out hit, shortRangeGrabDistace, grabMask, QueryTriggerInteraction.Ignore);
		}

		private bool CastGrabNormal(out RaycastHit hit, Collider colliderToLookFor)
		{
			int hitCount = UnityEngine.Physics.RaycastNonAlloc(grabNormalCaster.position, grabNormalCaster.forward, grabNormalHits, shortRangeGrabDistace, grabMask, QueryTriggerInteraction.Ignore);
			hit = grabNormalHits.Take(hitCount).FirstOrDefault(hit => hit.collider == colliderToLookFor);
			return grabNormalHits.Take(hitCount).Any(hit => hit.collider == colliderToLookFor);
		}

		public void OverrideShortRangeGrab(Vector3 casterPosition, Vector3 casterForward, float distance)
		{
			overrideShortRangeGrab = true;
			overrideShortRangeGrabCasterPosition = casterPosition;
			overrideShortRangeGrabCasterForward = casterForward;
			overrideShortRangeGrabDistance = distance;
		}

		public void ResetShortRangeGrabOverride()
		{
			overrideShortRangeGrab = false;
			canGrab = true;
		}

		private void OnTriggerEnter(Collider other)
		{
			if (other.gameObject.IsOnLayer(grabMask) && !collidingColliders.Contains(other))
				StartCollidingWith(other);
		}

		private void OnTriggerExit(Collider other)
		{
			if (collidingColliders.Contains(other))
				StopCollidingWith(other);
		}

		private void StartCollidingWith(Collider collider)
		{
			if (collider.isTrigger)
				return;
			
			collider.SelectGrabbable(grabbable => grabbable.onTouch.Invoke(this, grabbable));
			collidingColliders.Add(collider);
		}

		private void StopCollidingWith(Collider collider)
		{
			if (collider.isTrigger)
				return;
			
			collidingColliders.Remove(collider);
			collider.SelectGrabbable(grabbable => grabbable.onStopTouch.Invoke(this, grabbable));
		}

		public void ClearCollisions()
		{
			while (collidingColliders.Count > 0)
				StopCollidingWith(collidingColliders[0]);
		}

		public void ClearInvalidCollisions()
		{
			for (int i = 0; i < collidingColliders.Count; i++)
				if (!collidingColliders[i])
					collidingColliders.RemoveAt(i--);
				else if (!collidingColliders[i].gameObject.activeInHierarchy)
					StopCollidingWith(collidingColliders[i--]);
		}

		public void OnUse(CallbackContext context)
		{
			if (!context.performed)
				return;
			
			if (isHoldingSomething)
				UseHeldItem();
			else
			{
				Grab();
				if (isHoldingSomething)
				{
					UseHeldItem();
					Drop();
				}
			}
		}

		public void UseHeldItem()
		{
			if (!heldGrabbable)
				return;

			heldGrabbable.onUse.Invoke(this, heldGrabbable);
		}
	}

	internal static class GrabberExtensionMethods
	{
		public static TResult SelectGrabbable<TResult>(this Collider collider, Func<Grabbable, TResult> func)
		{
			Grabbable grabbable = collider.GetAttachedComponent<Grabbable>();
			return grabbable ? func(grabbable) : default;
		}

		public static void SelectGrabbable(this Collider collider, Action<Grabbable> func)
		{
			Grabbable grabbable = collider.GetAttachedComponent<Grabbable>();
			if (grabbable)
				func(grabbable);
		}
	}
}
using System;
using System.Collections.Generic;
using System.Linq;
using KBCore.Refs;
using UnityEngine;
using UnityEngine.Events;

namespace SBEPIS.Controller
{
	[RequireComponent(typeof(Rigidbody))]
	public class Grabber : ValidatedMonoBehaviour
	{
		[SerializeField, Self] private new Rigidbody rigidbody;
		public Rigidbody Rigidbody => rigidbody;
		
		[SerializeField] private LayerMask grabMask = 1;
		public LayerMask GrabMask => grabMask;
		[Tooltip("Should be within the hand collision box, far enough that you wouldn't reasonably be able to clip into it")]
		[SerializeField, Anywhere] private Transform grabNormalCaster;
		[Tooltip("Angle between the terrain's normal and player's up past which the grabber slips off")]
		[SerializeField] private float slipAngle = 80;
		[SerializeField] private float shortRangeGrabDistace = 1;
		public float ShortRangeGrabDistance => shortRangeGrabDistace;
		
		[SerializeField, Anywhere(Flag.Optional)] private Orientation playerOrientation;
		
		public GrabEvent onGrab = new();
		public ColliderGrabEvent onGrabCollider = new();
		public GrabEvent onUse = new();
		public GrabEvent onDrop = new();
		public ColliderGrabEvent onDropCollider = new();
		
		public bool CanGrab { get; set; } = true;

		private bool overrideGrabCompletely;
		private Collider overrideGrabCollider;
		private Vector3 overrideGrabPosition;
		private Quaternion overrideGrabRotation;
		
		private bool overrideShortRangeGrab;
		private Vector3 overrideShortRangeGrabCasterPosition;
		private Vector3 overrideShortRangeGrabCasterForward;
		private float overrideShortRangeGrabDistance;
		
		public bool IsHoldingSomething => HeldCollider;
		public Collider HeldCollider { get; private set; }
		public Grabbable HeldGrabbable { get; private set; }
		public GrabPoint HeldGrabPoint { get; private set; }
		private FixedJoint heldGrabbableJoint;
		private Vector3 heldPureColliderNormal;
		
		private readonly RaycastHit[] grabNormalHits = new RaycastHit[16];
		private readonly List<Collider> collidingColliders = new();
		private bool isHoldingGrab;
		public bool IsHoldingGrab
		{
			set
			{
				isHoldingGrab = value;
				UpdateGrabAttempt();
			}
		}

		private bool IsSlipping => heldPureColliderNormal != Vector3.zero && playerOrientation && Vector3.Angle(heldPureColliderNormal, playerOrientation.UpDirection) > slipAngle;
		
		private void Update()
		{
			ClearInvalidCollisions();
			UpdateGrabAttempt();
			if (HeldGrabbable)
				HeldGrabbable.HoldUpdate(this);
		}
		
		private void UpdateGrabAttempt()
		{
			if (!isActiveAndEnabled)
				return;
			
			if (IsHoldingSomething && (!isHoldingGrab || !HeldCollider.gameObject.activeInHierarchy || IsSlipping))
				Drop();
			else if (!IsHoldingSomething && isHoldingGrab)
				Grab();
		}
		
		private void Grab()
		{
			if (!CanGrab || IsHoldingSomething)
				return;
			
			if (overrideGrabCompletely)
			{
				transform.SetPositionAndRotation(overrideGrabPosition, overrideGrabRotation);
				GrabCollider(overrideGrabCollider);
				return;
			}
			
			if (collidingColliders.Any(collider => GrabCollider(collider)))
				return;
			
			ShortRangeGrab();
		}
		
		public bool GrabManually(Collider collider, bool dropIfHoldingSomething = false)
		{
			isHoldingGrab = true;
			return GrabCollider(collider, dropIfHoldingSomething);
		}
		
		private bool GrabCollider(Collider collider, bool dropIfHoldingSomething = false)
		{
			if (!collider && !collider.gameObject.activeInHierarchy)
				return false;
			
			if (IsHoldingSomething)
				if (dropIfHoldingSomething)
					Drop();
				else
					return false;
			
			Grabbable grabbable = collider.GetAttachedComponent<Grabbable>();
			if (grabbable)
				return GrabGrabbable(grabbable);
			
			if (CastGrabNormal(out RaycastHit hit, collider))
			{
				heldPureColliderNormal = hit.normal;
				if (IsSlipping)
					return false;
			}
			else
				heldPureColliderNormal = Vector3.zero;
			
			HeldCollider = collider;
			
			heldGrabbableJoint = gameObject.AddComponent<FixedJoint>();
			if (collider.attachedRigidbody)
				heldGrabbableJoint.connectedBody = collider.attachedRigidbody;
			
			onGrabCollider.Invoke(this, collider);
			
			return true;
		}
		
		public bool GrabManually(Grabbable grabbable, bool dropIfHoldingSomething = false)
		{
			isHoldingGrab = true;
			return GrabGrabbable(grabbable, dropIfHoldingSomething);
		}
		
		private bool GrabGrabbable(Grabbable grabbable, bool dropIfHoldingSomething = false)
		{
			if (!grabbable || !grabbable.gameObject.activeInHierarchy)
				return false;
			
			if (IsHoldingSomething)
				if (dropIfHoldingSomething)
					Drop();
				else
					return false;
			
			print($"Grabbing {grabbable}");
			
			heldPureColliderNormal = Vector3.zero;
			HeldGrabbable = grabbable;
			
			if (grabbable.grabPoints.Count > 0)
				BindToGrabPoint(grabbable, grabbable.grabPoints[0]);
			else
				HeldCollider = grabbable.Rigidbody.GetComponentInChildren<Collider>();
			
			heldGrabbableJoint = gameObject.AddComponent<FixedJoint>();
			heldGrabbableJoint.connectedBody = grabbable.Rigidbody;
			heldGrabbableJoint.autoConfigureConnectedAnchor = false;
			heldGrabbableJoint.anchor = transform.InverseTransformPoint(grabbable.transform.position);
			heldGrabbableJoint.connectedAnchor = Vector3.zero;
			
			onGrab.Invoke(this, grabbable);
			grabbable.OnGrabbed(this);
			
			return true;
		}

		private void BindToGrabPoint(Grabbable grabbable, GrabPoint grabPoint)
		{
			HeldGrabPoint = grabPoint;
			HeldCollider = grabPoint.colliderToGrab;
			grabbable.transform.SetPositionAndRotation(
				transform.TransformPoint(grabPoint.transform.InverseTransformPoint(grabbable.transform.position)),
				transform.TransformRotation(grabPoint.transform.InverseTransformRotation(grabbable.transform.rotation)));
		}

		private void ShortRangeGrab()
		{
			if (CastShortRangeGrab(out RaycastHit hit))
			{
				transform.SetPositionAndRotation(hit.point, Quaternion.LookRotation(-hit.normal, transform.up));
				GrabCollider(hit.collider);
			}
		}
		
		public void DropManually()
		{
			isHoldingGrab = false;
			Drop();
		}
		
		private void Drop()
		{
			if (!IsHoldingSomething)
				return;
			
			print($"Dropping {(HeldGrabbable ? HeldGrabbable : HeldCollider)}");
			
			Collider droppedCollider = HeldCollider;
			Grabbable droppedGrabbable = HeldGrabbable;
			HeldCollider = null;
			HeldGrabbable = null;
			HeldGrabPoint = null;
			
			Destroy(heldGrabbableJoint);
			heldGrabbableJoint = null;
			
			if (droppedGrabbable)
			{
				onDrop.Invoke(this, droppedGrabbable);
				droppedGrabbable.OnDropped(this);
			}
			else
			{
				onDropCollider.Invoke(this, droppedCollider);
			}
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
			CanGrab = true;
		}
		
		
		public void OverrideGrabCompletely(Collider collider, Vector3 position, Quaternion rotation)
		{
			overrideGrabCompletely = true;
			overrideGrabCollider = collider;
			overrideGrabPosition = position;
			overrideGrabRotation = rotation;
		}

		public void ResetGrabCompleteOverride()
		{
			overrideGrabCompletely = false;
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

		public void Use()
		{
			if (IsHoldingSomething)
				UseHeldItem();
			else
			{
				Grab();
				if (IsHoldingSomething)
				{
					UseHeldItem();
					Drop();
				}
			}
		}
		
		public void UseHeldItem()
		{
			if (!HeldGrabbable)
				return;
			
			onUse.Invoke(this, HeldGrabbable);
			HeldGrabbable.onUse.Invoke(this, HeldGrabbable);
		}
	}
	
	[Serializable]
	public class GrabEvent : UnityEvent<Grabber, Grabbable> { }
	[Serializable]
	public class ColliderGrabEvent : UnityEvent<Grabber, Collider> { }

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
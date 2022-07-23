using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using CallbackContext = UnityEngine.InputSystem.InputAction.CallbackContext;

namespace SBEPIS.Interaction.Controller
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

		[NonSerialized]
		public bool canShortRangeGrab = true;

		private bool overrideShortRangeGrab;
		private Vector3 overrideShortRangeGrabCasterPosition;
		private Vector3 overrideShortRangeGrabCasterForward;
		private float overrideShortRangeGrabDistance;

		public Collider heldCollider { get; private set; }
		public Grabbable heldGrabbable { get; private set; }
		private FixedJoint heldGrabbableJoint;
		private Vector3 heldGrabbableNormal;

		private RaycastHit[] grabNormalHits = new RaycastHit[16];

		public new Rigidbody rigidbody { get; private set; }

		private readonly List<Collider> collidingColliders = new();

		private bool isHoldingGrab;

		private void Awake()
		{
			rigidbody = GetComponent<Rigidbody>();
		}

		private void Start()
		{
			rigidbody.centerOfMass = Vector3.zero;
			rigidbody.inertiaTensor = Vector3.one;
			rigidbody.inertiaTensorRotation = Quaternion.identity;
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
			if (!enabled || !gameObject.activeInHierarchy)
				return;

			if (isHoldingGrab)
				if (heldCollider && heldGrabbableNormal != Vector3.zero && Vector3.Angle(heldGrabbableNormal, playerOrientation.upDirection) > slipAngle)
					Drop();
				else
					Grab();
			else
				Drop();
		}

		public void Grab()
		{
			if (heldCollider)
				return;

			foreach (Collider collidingCollider in collidingColliders)
			{
				print($"Attempting to grab {collidingCollider}");
				if (Grab(collidingCollider))
					return;
			}

			if (canShortRangeGrab)
				ShortRangeGrab();
		}

		public bool Grab(Collider collider)
		{
			if (heldCollider)
				return false;
			Grabbable grabbable = collider.SelectGrabbable();
			if (grabbable && !grabbable.canGrab)
				return false;

			if (grabbable)
				heldGrabbableNormal = Vector3.zero;
			else if (CastGrabNormal(out RaycastHit hit, collider))
			{
				heldGrabbableNormal = hit.normal;
				if (Vector3.Angle(heldGrabbableNormal, playerOrientation.upDirection) > slipAngle)
					return false;
			}
			else
				heldGrabbableNormal = Vector3.zero;

			heldCollider = collider;
			heldGrabbable = grabbable;

			heldGrabbableJoint = gameObject.AddComponent<FixedJoint>();
			if (collider.attachedRigidbody)
				heldGrabbableJoint.connectedBody = collider.attachedRigidbody;

			if (grabbable)
				grabbable.Grab(this);

			return true;
		}

		private void ShortRangeGrab()
		{
			if (CastShortRangeGrab(out RaycastHit hit))
			{
				print($"Short range grabbing {hit.collider}");
				transform.SetPositionAndRotation(hit.point, Quaternion.LookRotation(-hit.normal, transform.up));
				Grab(hit.collider);
			}
		}

		public void Drop()
		{
			if (!heldCollider)
				return;

			Collider droppedCollider = heldCollider;
			Grabbable droppedGrabbable = heldGrabbable;
			heldCollider = null;
			heldGrabbable = null;

			Destroy(heldGrabbableJoint);
			heldGrabbableJoint = null;

			if (droppedCollider.attachedRigidbody)
				droppedCollider.attachedRigidbody.AddForce(Vector3.up * 0.01f);
			if (droppedGrabbable)
				droppedGrabbable.Drop(this);
		}

		private bool CastShortRangeGrab(out RaycastHit hit)
		{
			if (overrideShortRangeGrab)
				return UnityEngine.Physics.Raycast(overrideShortRangeGrabCasterPosition, overrideShortRangeGrabCasterForward, out hit, overrideShortRangeGrabDistance, grabMask, QueryTriggerInteraction.Ignore);
			else
				return UnityEngine.Physics.Raycast(transform.position, transform.forward, out hit, shortRangeGrabDistace, grabMask, QueryTriggerInteraction.Ignore);
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

			print($"Colliding with {collider}");
			collider.SelectGrabbable(grabbable => grabbable.onTouch.Invoke(this));
			collidingColliders.Add(collider);
		}

		private void StopCollidingWith(Collider collider)
		{
			if (collider.isTrigger)
				return;

			print($"No longer colliding with {collider}");
			collidingColliders.Remove(collider);
			collider.SelectGrabbable(grabbable => grabbable.onStopTouch.Invoke(this));
		}

		public void ClearCollisions()
		{
			while (collidingColliders.Count > 0)
				StopCollidingWith(collidingColliders[0]);
		}

		public void ClearInvalidCollisions()
		{
			for (int i = 0; i < collidingColliders.Count; i++)
				if (!collidingColliders[i].gameObject.activeInHierarchy)
					StopCollidingWith(collidingColliders[i--]);
		}
	}

	internal static class GrabberExtensionMethods
	{
		public static Grabbable SelectGrabbable(this Collider collider) => collider.attachedRigidbody ? collider.attachedRigidbody.GetComponent<Grabbable>() : null;

		public static TResult SelectGrabbable<TResult>(this Collider collider, Func<Grabbable, TResult> func)
		{
			Grabbable grabbable = collider.SelectGrabbable();
			return grabbable ? func(grabbable) : default;
		}

		public static void SelectGrabbable(this Collider collider, Action<Grabbable> func)
		{
			Grabbable grabbable = collider.SelectGrabbable();
			if (grabbable)
				func(grabbable);
		}
	}
}
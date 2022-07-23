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
		public float shortRangeGrabDistace = 1;
		public LayerMask shortRangeGrabMask = 1;

		[NonSerialized]
		public bool canShortRangeGrab = true;

		private Transform overrideShortRangeGrabCaster;
		private float overrideShortRangeGrabDistance;

		private Collider[] collisionColliders = new Collider[0];

		public Collider heldCollider { get; private set; }
		public Grabbable heldGrabbable { get; private set; }

		private FixedJoint heldGrabbableJoint;
		public new Rigidbody rigidbody { get; private set; }

		private readonly List<Collider> collidingColliders = new();

		private bool isAttemptingGrab;

		private bool shouldEnableColliders;
		private bool shouldDisableColliders;

		private void Awake()
		{
			rigidbody = GetComponent<Rigidbody>();
			collisionColliders = GetComponentsInChildren<Collider>().Where(collider => collider.enabled && !collider.isTrigger).ToArray();
		}

		private void Start()
		{
			rigidbody.centerOfMass = Vector3.zero;
			rigidbody.inertiaTensor = Vector3.one;
			rigidbody.inertiaTensorRotation = Quaternion.identity;
		}

		private void Update()
		{
			HandleEnablingColliders();
			ClearInvalidCollisions();
			UpdateGrabAttempt();
			if (heldGrabbable)
				heldGrabbable.HoldUpdate(this);
		}

		public void OnGrab(CallbackContext context)
		{
			isAttemptingGrab = context.performed;
			UpdateGrabAttempt();
		}

		public void UpdateGrabAttempt()
		{
			if (!enabled || !gameObject.activeInHierarchy)
				return;

			if (isAttemptingGrab)
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
			if (overrideShortRangeGrabCaster)
				return UnityEngine.Physics.Raycast(overrideShortRangeGrabCaster.position, overrideShortRangeGrabCaster.forward, out hit, overrideShortRangeGrabDistance, shortRangeGrabMask, QueryTriggerInteraction.Ignore);
			else
				return UnityEngine.Physics.Raycast(transform.position, transform.forward, out hit, shortRangeGrabDistace, shortRangeGrabMask, QueryTriggerInteraction.Ignore);
		}

		public void OverrideShortRangeGrab(Transform caster, float distance)
		{
			overrideShortRangeGrabCaster = caster;
			overrideShortRangeGrabDistance = distance;
		}

		private void OnTriggerEnter(Collider other)
		{
			if (!collidingColliders.Contains(other))
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

		public void QueueEnableColliders()
		{
			shouldEnableColliders = true;
			shouldDisableColliders = false;
		}

		public void QueueDisableColliders()
		{
			shouldEnableColliders = false;
			shouldDisableColliders = true;
		}

		private void HandleEnablingColliders()
		{
			if (collisionColliders.Length == 0 || (!shouldEnableColliders && !shouldDisableColliders))
				return;

			if (shouldEnableColliders)
				foreach (Collider collider in collisionColliders)
					collider.enabled = true;
			shouldEnableColliders = false;

			if (shouldDisableColliders)
				foreach (Collider collider in collisionColliders)
					collider.enabled = false;
			shouldDisableColliders = false;
		}

		// Note that this happens *before* Awake is called
		public void OnControlsChanged(PlayerInput input)
		{
			switch (input.currentControlScheme)
			{
				case "OpenXR":
					//QueueEnableColliders();
					break;

				default:
					//QueueDisableColliders();
					break;
			}
		}

		public enum Scheme
		{
			NONE,
			OpenXR,
			Keyboard,
			Controller,
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
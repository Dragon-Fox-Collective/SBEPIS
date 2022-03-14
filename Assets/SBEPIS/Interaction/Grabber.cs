using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using CallbackContext = UnityEngine.InputSystem.InputAction.CallbackContext;

namespace SBEPIS.Interaction
{
	[RequireComponent(typeof(Rigidbody))]
	public class Grabber : MonoBehaviour
	{
		public Scheme enabledScheme;

		public Grabbable heldGrabbable { get; private set; }

		private FixedJoint heldGrabbableJoint;
		public new Rigidbody rigidbody { get; private set; }

		private List<Grabbable> collidingGrabbables = new List<Grabbable>();

		private void Awake()
		{
			rigidbody = GetComponent<Rigidbody>();
		}

		private void Update()
		{
			ClearInvalidCollisions();
			if (heldGrabbable)
				heldGrabbable.HoldUpdate(this);
		}

		public void OnGrab(CallbackContext context)
		{
			if (!gameObject.activeInHierarchy || !enabled)
				return;

			bool isPressed = context.performed;

			if (isPressed && !heldGrabbable && collidingGrabbables.Count > 0)
			{
				foreach (Grabbable collidingGrabbable in collidingGrabbables)
				{
					print($"Attempting to grab {collidingGrabbable}");
					if (collidingGrabbable.canGrab)
					{
						heldGrabbable = collidingGrabbable;

						heldGrabbableJoint = collidingGrabbable.gameObject.AddComponent<FixedJoint>();
						heldGrabbableJoint.connectedBody = rigidbody;

						collidingGrabbable.Grab(this);
						break;
					}
				}
			}
			else if (!isPressed && heldGrabbable)
			{
				Grabbable droppedGrabbable = heldGrabbable;
				heldGrabbable = null;

				Destroy(heldGrabbableJoint);
				heldGrabbableJoint = null;
				droppedGrabbable.rigidbody.AddForce(Vector3.up * 0.01f);

				droppedGrabbable.Drop(this);
			}
		}

		private void OnTriggerEnter(Collider other)
		{
			if (other.attachedRigidbody)
			{
				Grabbable hitGrabbable = other.attachedRigidbody.GetComponent<Grabbable>();
				if (hitGrabbable && !collidingGrabbables.Contains(hitGrabbable))
					StartCollidingWith(hitGrabbable);
			}
		}

		private void OnTriggerExit(Collider other)
		{
			if (other.attachedRigidbody)
			{
				Grabbable hitGrabbable = other.attachedRigidbody.GetComponent<Grabbable>();
				if (hitGrabbable)
					StopCollidingWith(hitGrabbable);
			}
		}

		private void StartCollidingWith(Grabbable grabbable)
		{
			print($"Colliding with {grabbable}");
			grabbable.onTouch.Invoke(this);
			collidingGrabbables.Add(grabbable);
		}

		private void StopCollidingWith(Grabbable grabbable)
		{
			print($"No longer colliding with {grabbable}");
			collidingGrabbables.Remove(grabbable);
			grabbable.onStopTouch.Invoke(this);
		}

		public void ClearCollisions()
		{
			while (collidingGrabbables.Count > 0)
				StopCollidingWith(collidingGrabbables[0]);
		}

		public void ClearInvalidCollisions()
		{
			for (int i = 0; i < collidingGrabbables.Count; i++)
				if (!collidingGrabbables[i].gameObject.activeInHierarchy)
					StopCollidingWith(collidingGrabbables[i--]);
		}

		public void OnControlsChanged(PlayerInput input)
		{
			if (input.currentControlScheme == enabledScheme.ToString())
			{
				print($"Activating {enabledScheme} input for {this}");
				gameObject.SetActive(true);
			}
			else
			{
				gameObject.SetActive(false);
			}
		}

		public enum Scheme
		{
			NONE,
			OpenXR,
			Keyboard
		}
	}
}
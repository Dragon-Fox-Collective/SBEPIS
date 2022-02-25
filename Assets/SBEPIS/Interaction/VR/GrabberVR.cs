using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.InputSystem;
using CallbackContext = UnityEngine.InputSystem.InputAction.CallbackContext;

namespace SBEPIS.Interaction.VR
{
	[RequireComponent(typeof(Rigidbody))]
	public class GrabberVR : Grabber
	{
		public Grabbable heldGrabbable { get; private set; }
		private FixedJoint heldGrabbableJoint;
		private new Rigidbody rigidbody;

		private List<Grabbable> collidingGrabbables = new List<Grabbable>();

		private void Awake()
		{
			rigidbody = GetComponent<Rigidbody>();
		}

		private void Update()
		{
			if (heldGrabbable)
				heldGrabbable.HoldUpdate(this);
		}

		public void OnGrab(CallbackContext context)
		{
			if (!gameObject.activeInHierarchy)
				return;

			bool isPressed = context.performed;
			
			if (isPressed && !heldGrabbable && collidingGrabbables.Count > 0)
			{
				foreach (Grabbable collidingGrabbable in collidingGrabbables)
				{
					print($"Attempting to grab {collidingGrabbable}");
					if (collidingGrabbable.canGrab)
					{
						print($"Grabbing {collidingGrabbable}");
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
				droppedGrabbable.rigidbody.WakeUp();

				droppedGrabbable.Drop(this);
			}
		}

		private void OnTriggerEnter(Collider other)
		{
			Grabbable hitGrabbable = other.GetComponent<Grabbable>();
			if (hitGrabbable && !collidingGrabbables.Contains(hitGrabbable))
			{
				print($"Colliding with {hitGrabbable}");
				collidingGrabbables.Add(hitGrabbable);
			}
		}

		private void OnTriggerExit(Collider other)
		{
			Grabbable hitGrabbable = other.GetComponent<Grabbable>();
			if (hitGrabbable)
				collidingGrabbables.Remove(hitGrabbable);
		}

		public void OnControlsChanged(PlayerInput input)
		{
			switch (input.currentControlScheme)
			{
				case "OpenXR":
					print("Activating OpenXR input");
					gameObject.SetActive(true);
					break;

				default:
					gameObject.SetActive(false);
					break;
			}
		}
	}
}
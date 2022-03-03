using UnityEngine;
using UnityEngine.InputSystem;
using CallbackContext = UnityEngine.InputSystem.InputAction.CallbackContext;

namespace SBEPIS.Interaction.Flatscreen
{
	public class FlatscreenGrabber : Grabber
	{
		public Rigidbody connectionPoint;
		public LayerMask raycastMask = 1;

		public float maxGrabDistance = 10;
		public float farHoldDistance = 2;
		public float nearHoldDistance = 0.7f;

		public float velocityFactor = 1;
		public float angularVelocityFactor = 1;

		public Grabbable heldGrabbable { get; private set; }

		private VelocityJoint heldGrabbableJoint;

		private void Start()
		{
			connectionPoint.position = transform.position + transform.forward * farHoldDistance;
		}

		private void Update()
		{
			if (heldGrabbable)
				heldGrabbable.HoldUpdate(this);
		}

		public void OnZoom(CallbackContext context)
		{
			if (context.ReadValue<float>() < 0)
				connectionPoint.position = transform.position + transform.forward * nearHoldDistance;
			else if (context.ReadValue<float>() > 0)
				connectionPoint.position = transform.position + transform.forward * farHoldDistance;
		}

		public void OnGrab(CallbackContext context)
		{
			if (!gameObject.activeInHierarchy)
				return;

			bool isPressed = context.performed;

			if (isPressed && !heldGrabbable && CastForGrabbables(out RaycastHit hit))
			{
				Grabbable hitGrabbable = hit.rigidbody.GetComponent<Grabbable>();
				print($"Attempting to grab {hitGrabbable}");
				if (hitGrabbable && hitGrabbable.canGrab)
				{
					heldGrabbable = hitGrabbable;

					heldGrabbableJoint = hitGrabbable.gameObject.AddComponent<VelocityJoint>();
					heldGrabbableJoint.connectionPoint = connectionPoint.transform;
					heldGrabbableJoint.velocityFactor = velocityFactor;
					heldGrabbableJoint.angularVelocityFactor = angularVelocityFactor;

					hitGrabbable.Grab(this);
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

		public bool Cast(out RaycastHit hit, LayerMask mask)
		{
			return Physics.Raycast(transform.position, transform.forward, out hit, maxGrabDistance, mask);
		}

		public bool CastForGrabbables(out RaycastHit hit)
		{
			return Cast(out hit, raycastMask) && hit.rigidbody;
		}

		public void OnControlsChanged(PlayerInput input)
		{
			switch (input.currentControlScheme)
			{
				case "Keyboard":
					print("Activating Keyboard input");
					gameObject.SetActive(true);
					break;

				default:
					gameObject.SetActive(false);
					break;
			}
		}
	}
}
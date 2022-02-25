using UnityEngine;
using UnityEngine.InputSystem;
using CallbackContext = UnityEngine.InputSystem.InputAction.CallbackContext;

namespace SBEPIS.Interaction.Flatscreen
{
	public class FlatscreenGrabber : Grabber
	{
		public new Transform camera;
		public LayerMask raycastMask = 1;

		public float maxGrabDistance = 10;
		public float farHoldDistance = 2;
		public float nearHoldDistance = 0.7f;

		public Grabbable heldGrabbable { get; private set; }
		private float holdDistance = 2;

		private Transform activeRaycaster;

		private void Awake()
		{
			activeRaycaster = camera;
		}

		private void Update()
		{
			if (heldGrabbable)
				heldGrabbable.HoldUpdate(this);
		}

		private void FixedUpdate()
		{
			if (heldGrabbable)
				UpdateGrabbable(heldGrabbable, 0.05f, 0.05f);
		}

		public void UpdateGrabbable(Grabbable grabbable, float posTime, float rotTime)
		{
			Vector3 velocity = grabbable.rigidbody.velocity;
			Vector3.SmoothDamp(grabbable.transform.position, activeRaycaster.position + activeRaycaster.forward * holdDistance, ref velocity, posTime);
			grabbable.rigidbody.velocity = velocity;

			Quaternion deriv = QuaternionUtil.AngVelToDeriv(grabbable.transform.rotation, grabbable.rigidbody.angularVelocity);
			QuaternionUtil.SmoothDamp(grabbable.transform.rotation, Quaternion.identity, ref deriv, rotTime);
			grabbable.rigidbody.angularVelocity = QuaternionUtil.DerivToAngVel(grabbable.transform.rotation, deriv);
		}

		public void OnZoom(CallbackContext context)
		{
			if (context.ReadValue<float>() < 0)
				holdDistance = nearHoldDistance;
			else if (context.ReadValue<float>() > 0)
				holdDistance = farHoldDistance;
		}

		public void OnGrab(CallbackContext context)
		{
			if (!activeRaycaster)
				return;

			bool isPressed = context.performed;

			if (isPressed && !heldGrabbable && CastForGrabbables(out RaycastHit hit))
			{
				Grabbable hitGrabbable = hit.rigidbody.GetComponent<Grabbable>();
				if (hitGrabbable && hitGrabbable.canGrab)
				{
					heldGrabbable = hitGrabbable;
					hitGrabbable.Grab(this);
				}
			}
			else if (!isPressed && heldGrabbable)
			{
				Grabbable droppedGrabbable = heldGrabbable;
				heldGrabbable = null;

				droppedGrabbable.Drop(this);
			}
		}

		public bool Cast(out RaycastHit hit, LayerMask mask)
		{
			return Physics.Raycast(activeRaycaster.transform.position, activeRaycaster.transform.forward, out hit, maxGrabDistance, mask);
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
					activeRaycaster = camera;
					break;

				default:
					activeRaycaster = null;
					break;
			}
		}
	}
}
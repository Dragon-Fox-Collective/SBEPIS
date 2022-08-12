using SBEPIS.Controller;
using UnityEngine;
using CallbackContext = UnityEngine.InputSystem.InputAction.CallbackContext;

namespace SBEPIS.Capturllection
{
	[RequireComponent(typeof(Grabber))]
	public class CaptureDequeSummoner : MonoBehaviour
	{
		public CaptureDeque deque;

		private Grabber grabber;

		private void Awake()
		{
			grabber = GetComponent<Grabber>();
		}

		public void OnToggleDeque(CallbackContext context)
		{
			if (!isActiveAndEnabled || !context.performed)
				return;

			ToggleDeque();
		}

		public void ToggleDeque()
		{
			if (deque.gameObject.activeSelf)
				DesummonDeque();
			else
				SummonDeque();
		}

		public void SummonDeque()
		{
			deque.gameObject.SetActive(true);
			deque.transform.SetPositionAndRotation(transform.position, transform.rotation * Quaternion.Euler(0, 180, 0));
			deque.grabbable.rigidbody.velocity = Vector3.zero;
			deque.grabbable.rigidbody.angularVelocity = Vector3.zero;
			grabber.Grab(deque.grabbable);
		}

		public void DesummonDeque()
		{
			deque.gameObject.SetActive(false);
			deque.ForceClose();
		}
	}
}

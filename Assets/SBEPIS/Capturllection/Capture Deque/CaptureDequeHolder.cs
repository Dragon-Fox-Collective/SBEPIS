using UnityEngine;
using CallbackContext = UnityEngine.InputSystem.InputAction.CallbackContext;

namespace SBEPIS.Capturllection
{
	public class CaptureDequeHolder : MonoBehaviour
	{
		public CaptureDeque deque;

		private void Start()
		{
			deque.gameObject.SetActive(false);
		}

		public void OnToggleDeque(CallbackContext context)
		{
			if (!context.performed)
				return;

			deque.gameObject.SetActive(!deque.gameObject.activeSelf);
		}

		public void OnCapturllect(CallbackContext context)
		{
			if (!context.performed)
				return;

			print("capturllected");
		}
	}
}

using UnityEngine;
using CallbackContext = UnityEngine.InputSystem.InputAction.CallbackContext;

namespace SBEPIS.Thaumaturgy
{
	public class CaptureDequeHolder : MonoBehaviour
	{
		public Transform deque;

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

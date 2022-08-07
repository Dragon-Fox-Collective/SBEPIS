using SBEPIS.Controller;
using UnityEngine;
using CallbackContext = UnityEngine.InputSystem.InputAction.CallbackContext;

namespace SBEPIS.Capturllection
{
	[RequireComponent(typeof(Grabber))]
	public class CaptureDequeSummoner : MonoBehaviour
	{
		public CaptureDeque deque;

		private void Start()
		{
			deque.gameObject.SetActive(false);
		}

		public void OnToggleDeque(CallbackContext context)
		{
			if (!gameObject.activeInHierarchy || !enabled || !context.performed)
				return;

			deque.gameObject.SetActive(!deque.gameObject.activeSelf);
		}
	}
}

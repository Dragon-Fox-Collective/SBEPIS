using UnityEngine;
using CallbackContext = UnityEngine.InputSystem.InputAction.CallbackContext;

namespace SBEPIS.Capturllection
{
	public class DequeOwner : MonoBehaviour
	{
		public CaptureDeque deque;

		public void OnToggleDeque(CallbackContext context)
		{
			if (!isActiveAndEnabled || !context.performed)
				return;
			
			ToggleDeque();
		}
		
		public void ToggleDeque() => deque.ToggleDiajector();

		public void OpenDeque() => deque.SummonDiajector();
		
		public void CloseDeque() => deque.DesummonDiajector();
	}
}

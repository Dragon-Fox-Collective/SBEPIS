using KBCore.Refs;
using UnityEngine;
using UnityEngine.InputSystem;
using CallbackContext = UnityEngine.InputSystem.InputAction.CallbackContext;

namespace SBEPIS.Commands
{
	public class CommandStaff : ValidatedMonoBehaviour
	{
		[SerializeField, Anywhere]
		private PlayerInput input;
		
		public void OnOpenStaff(CallbackContext context)
		{
			if (!context.performed)
				return;

			gameObject.SetActive(true);
			input.SwitchCurrentActionMap("Command Staff");
		}
		
		public void OnCloseStaff(CallbackContext context)
		{
			if (!context.performed)
				return;

			gameObject.SetActive(false);
			input.SwitchCurrentActionMap("Gameplay");
		}
	}
}

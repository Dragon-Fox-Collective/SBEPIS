using KBCore.Refs;
using UnityEngine;
using UnityEngine.InputSystem;

namespace SBEPIS.Commands
{
	public class CommandStaff : ValidatedMonoBehaviour
	{
		[SerializeField, Anywhere]
		private PlayerInput input;
		
		public void OpenStaff()
		{
			gameObject.SetActive(true);
			input.SwitchCurrentActionMap("Command Staff");
		}
		
		public void CloseStaff()
		{
			gameObject.SetActive(false);
			input.SwitchCurrentActionMap("Gameplay");
		}
	}
}

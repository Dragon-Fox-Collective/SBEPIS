using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace SBEPIS.Interaction.Controller
{
	public class ControlSchemeEvents : MonoBehaviour
	{
		public UnityEvent onOpenXR = new(), onFlatscreen = new();

		public void OnControlsChanged(PlayerInput input)
		{
			switch (input.currentControlScheme)
			{
				case "OpenXR":
					print("Activating OpenXR input");
					onOpenXR.Invoke();
					break;

				case "Gamepad":
					print("Activating Gamepad input");
					onFlatscreen.Invoke();
					break;

				case "Keyboard":
					print("Activating Keyboard input");
					onFlatscreen.Invoke();
					break;

				default:
					throw new NotSupportedException($"Invalid control scheme {input.currentControlScheme}");
			}
		}
	}
}

using System;
using KBCore.Refs;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace SBEPIS.Controller
{
	public class ControlSchemeEvents : ValidatedMonoBehaviour
	{
		[SerializeField, Self]
		private PlayerInput input;
		
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
		
		private void OnEnable()
		{
			input.onControlsChanged += OnControlsChanged;
		}
		
		private void OnDisable()
		{
			input.onControlsChanged -= OnControlsChanged;
		}
	}
}

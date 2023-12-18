using System;
using System.Collections.Generic;
using System.Linq;
using KBCore.Refs;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;
using CallbackContext = UnityEngine.InputSystem.InputAction.CallbackContext;

namespace SBEPIS.Utils
{
	public class PlayerInputDataEvents : ValidatedMonoBehaviour
	{
		[SerializeField, Self]
		private PlayerInput input;
		public PlayerInput Input => input;
		
		[SerializeField]
		private List<PlayerInputEvent> triggerActions = new();
		public List<PlayerInputEvent> TriggerActions => triggerActions;
		[SerializeField]
		private List<PlayerInputEvent<bool>> boolActions = new();
		public List<PlayerInputEvent<bool>> BoolActions => boolActions;
		[SerializeField]
		private List<PlayerInputEvent<float>> floatActions = new();
		public List<PlayerInputEvent<float>> FloatActions => floatActions;
		[SerializeField]
		private List<PlayerInputEvent<Vector2>> vector2Actions = new();
		public List<PlayerInputEvent<Vector2>> Vector2Actions => vector2Actions;
		[SerializeField]
		private List<PlayerInputEvent<Vector3>> vector3Actions = new();
		public List<PlayerInputEvent<Vector3>> Vector3Actions => vector3Actions;
		[SerializeField]
		private List<PlayerInputEvent<PoseState>> poseActions = new();
		public List<PlayerInputEvent<PoseState>> PoseActions => poseActions;
		[SerializeField]
		private List<PlayerInputEvent<Quaternion>> quaternionActions = new();
		public List<PlayerInputEvent<Quaternion>> QuaternionActions => quaternionActions;
		
		private void OnActionTriggered(CallbackContext context)
		{
			string actionId = context.action.id.ToString();

			switch(context.action.expectedControlType)
			{
				case "Button":
					if (context.performed)
						triggerActions.First(inputEvent => inputEvent.ActionId == actionId).Invoke();
					if (context.performed)
						boolActions.First(inputEvent => inputEvent.ActionId == actionId).Invoke(true);
					if (context.canceled)
						boolActions.First(inputEvent => inputEvent.ActionId == actionId).Invoke(false);
					break;
				
				case "Axis":
					if (context.performed || context.canceled)
						floatActions.First(inputEvent => inputEvent.ActionId == actionId).Invoke(context.ReadValue<float>());
					break;
				
				case "Vector2":
					if (context.performed || context.canceled)
						vector2Actions.First(inputEvent => inputEvent.ActionId == actionId).Invoke(context.ReadValue<Vector2>());
					break;
				
				case "Vector3":
					if (context.performed || context.canceled)
						vector3Actions.First(inputEvent => inputEvent.ActionId == actionId).Invoke(context.ReadValue<Vector3>());
					break;
				
				case "Pose":
					if (context.performed || context.canceled)
						poseActions.First(inputEvent => inputEvent.ActionId == actionId).Invoke(context.ReadValue<PoseState>());
					break;
				
				case "Quaternion":
					if (context.performed || context.canceled)
						quaternionActions.First(inputEvent => inputEvent.ActionId == actionId).Invoke(context.ReadValue<Quaternion>());
					break;
			}
		}
		
		private void OnEnable()
		{
			input.onActionTriggered += OnActionTriggered;
		}
		
		private void OnDisable()
		{
			input.onActionTriggered -= OnActionTriggered;
		}
	}
	
	[Serializable]
	public class PlayerInputEvent : UnityEvent
	{
		[SerializeField]
		public string ActionId;
	}
	
	[Serializable]
	public class PlayerInputEvent<T> : UnityEvent<T>
	{
		[SerializeField]
		public string ActionId;
	}
}

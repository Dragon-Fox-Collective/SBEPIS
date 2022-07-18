using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.PlayerInput;
using CallbackContext = UnityEngine.InputSystem.InputAction.CallbackContext;

namespace SBEPIS.Interaction.XR
{
	[RequireComponent(typeof(PlayerInput))]
	public class XRBinder : MonoBehaviour
	{
		public PlayerInput bindTo;

		private PlayerInput bindFrom;
		private HashSet<InputDevice> devices = new HashSet<InputDevice>();

		private void Awake()
		{
			bindFrom = GetComponent<PlayerInput>();
		}

		private void Start()
		{
			foreach (ActionEvent e in bindFrom.actionEvents)
				e.AddListener(AttemptBind);
		}

		public void AttemptBind(CallbackContext context)
		{
			if (!context.performed || !bindTo.inputIsActive)
				return;

			foreach (ActionEvent e in bindFrom.actionEvents)
				if (e.actionId == context.action.id.ToString())
					e.RemoveListener(AttemptBind);

			devices.UnionWith(context.action.controls.Select(control => control.device));
			bindTo.SwitchCurrentControlScheme("OpenXR", devices.ToArray());
		}
	}
}

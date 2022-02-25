using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace SBEPIS.Interaction
{
	[RequireComponent(typeof(PlayerInput))]
	public class PlayerModeSwapper : MonoBehaviour
	{
		public new Transform camera;
		public PlayerModeEvent onSetPlayerMode;

		private Transform defaultCameraParent;
		private PlayerInput playerInput;

		private void Awake()
		{
			defaultCameraParent = camera.parent;
			playerInput = GetComponent<PlayerInput>();
		}

		public void SetPlayerMode(PlayerMode mode, Transform newParent)
		{
			switch (mode)
			{
				case PlayerMode.Gameplay:
					playerInput.SwitchCurrentActionMap("Gameplay");
					break;
				case PlayerMode.Typing:
					playerInput.SwitchCurrentActionMap("Typing");
					break;
			}
			camera.SetParent(newParent);
			onSetPlayerMode.Invoke(mode);
		}

		public void SetPlayerMode(PlayerMode mode) => SetPlayerMode(mode, defaultCameraParent);
	}

	public enum PlayerMode { Gameplay, Typing }

	[Serializable]
	public class PlayerModeEvent : UnityEvent<PlayerMode> { }
}
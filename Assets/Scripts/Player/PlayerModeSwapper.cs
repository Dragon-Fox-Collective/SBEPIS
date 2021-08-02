using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace WrightWay.SBEPIS.Player
{
	[RequireComponent(typeof(PlayerInput))]
	public class PlayerModeSwapper : MonoBehaviour
	{
		public new Transform camera;
		public PlayerModeEvent onSetPlayerMode;

		private Transform cameraParent;
		private PlayerInput playerInput;

		private void Awake()
		{
			cameraParent = camera.parent;
			playerInput = GetComponent<PlayerInput>();
		}

		public void SetPlayerMode(PlayerMode mode, Transform newParent)
		{
			switch (mode)
			{
				case PlayerMode.Normal:
					playerInput.SwitchCurrentActionMap("Normal");
					break;
				case PlayerMode.Keyboard:
					playerInput.SwitchCurrentActionMap("Keyboard");
					break;
			}
			camera.SetParent(newParent);
			onSetPlayerMode.Invoke(mode);
		}

		public void SetPlayerMode(PlayerMode mode) => SetPlayerMode(mode, cameraParent);
	}

	public enum PlayerMode { Normal, Keyboard, Sylladex }

	[Serializable]
	public class PlayerModeEvent : UnityEvent<PlayerMode> { }
}
using SBEPIS.Utils;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using CallbackContext = UnityEngine.InputSystem.InputAction.CallbackContext;

namespace SBEPIS.Interaction
{
	public class MenuActions : MonoBehaviour
	{
		public Transform playerPosition;
		public Transform yawRotation;
		public Transform pauseButtons;
		public Transform main;
		public Transform settings;

		public PhysicsSlider sensitivitySlider;

		public PlayerInput input;

		public MovementController controller;

		public float mouseSensitivityMin = 0.1f;
		public float mouseSensitivityMax = 1;

		private PlayerSettingsSaveData settingsData;

		private void Start()
		{
			settingsData = new PlayerSettingsSaveData { filename = "settings", sensitivity = controller.sensitivity };
			DataSaver.LoadData(ref settingsData);
			controller.sensitivity = settingsData.sensitivity;
		}

		public void StartNewGame()
		{
			StartCoroutine(LoadNewGameScene());
		}

		private IEnumerator LoadNewGameScene()
		{
			AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("InventoryDemo");
			while (!asyncLoad.isDone)
				yield return null;
		}

		public void OpenMainMenu()
		{
			StartCoroutine(LoadMainMenuScene());
		}

		private IEnumerator LoadMainMenuScene()
		{
			AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Main Menu");
			while (!asyncLoad.isDone)
				yield return null;
		}

		public void QuitGame()
		{
#if UNITY_EDITOR
			UnityEditor.EditorApplication.isPlaying = false;
#endif
			Application.Quit();
		}

		public void GoToMain()
		{
			main.gameObject.SetActive(true);
			settings.gameObject.SetActive(false);
		}

		public void GoToSettings()
		{
			main.gameObject.SetActive(false);
			settings.gameObject.SetActive(true);
			sensitivitySlider.ResetAnchor(controller.sensitivity.Map(mouseSensitivityMin, mouseSensitivityMax, 0, 1));
		}

		public void OnTogglePauseMenu(CallbackContext context)
		{
			if (!context.performed)
				return;

			pauseButtons.gameObject.SetActive(!pauseButtons.gameObject.activeSelf);

			if (pauseButtons.gameObject.activeSelf)
			{
				pauseButtons.position = playerPosition.position;
				pauseButtons.rotation = Quaternion.Euler(0, yawRotation.rotation.eulerAngles.y, 0);
				GoToMain();
			}
		}

		public void ChangeMouseSensitivity(float percent)
		{
			controller.sensitivity = percent.Map(0, 1, mouseSensitivityMin, mouseSensitivityMax);
			settingsData.sensitivity = controller.sensitivity;
			DataSaver.SaveData(settingsData);
		}
	}
}

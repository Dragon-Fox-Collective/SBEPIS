using SBEPIS.Controller;
using SBEPIS.Utils;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using CallbackContext = UnityEngine.InputSystem.InputAction.CallbackContext;

namespace SBEPIS.UI
{
	public class MenuActions : MonoBehaviour
	{
		public UnityEvent onPause = new(), onUnpause = new();

		[Header("Menu transforms")]
		public Transform pauseButtons;
		public Transform main;
		public Transform settings;
		
		[Header("Controls")]
		public PhysicsSlider sensitivitySlider;
		public float mouseSensitivityMin = 0.1f;
		public float mouseSensitivityMax = 1;

		[Header("Settings readers")]
		public MovementController movementController;
		public LookController lookController;

		private PlayerSettingsSaveData settingsData;

		public void StartNewGame()
		{
			StartCoroutine(LoadNewGameScene());
		}

		private IEnumerator LoadNewGameScene()
		{
			AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Demo");
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

			ResetMouseSensitivitySlider();
		}

		public void OnTogglePauseMenu(CallbackContext context)
		{
			if (!context.performed)
				return;

			if (pauseButtons.gameObject.activeSelf)
				Unpause();
			else
				Pause();
		}

		public void Pause()
		{
			pauseButtons.gameObject.SetActive(true);
			GoToMain();
			onPause.Invoke();
		}

		public void Unpause()
		{
			pauseButtons.gameObject.SetActive(false);
			onUnpause.Invoke();
		}


		private void Start()
		{
			settingsData = new PlayerSettingsSaveData { filename = "settings" };

			if (DataSaver.LoadData(ref settingsData))
			{
				LoadMouseSensitivity(ref settingsData);
				//LoadEyeHeight(ref settingsData);
			}
			else
			{
				SaveMouseSensitivity(ref settingsData);
				//SaveEyeHeight(ref settingsData);
			}
		}

		public void LoadMouseSensitivity(ref PlayerSettingsSaveData settingsData)
		{
			if (settingsData.version >= 0)
				lookController.sensitivity = settingsData.sensitivity;
			else
				SaveMouseSensitivity(ref settingsData);
		}

		public void SaveMouseSensitivity(ref PlayerSettingsSaveData settingsData)
		{
			settingsData.sensitivity = lookController.sensitivity;
		}

		public void ResetMouseSensitivitySlider()
		{
			sensitivitySlider.ResetAnchor(lookController.sensitivity.Map(mouseSensitivityMin, mouseSensitivityMax, 0, 1));
		}

		public void ChangeMouseSensitivity(float percent)
		{
			lookController.sensitivity = percent.Map(0, 1, mouseSensitivityMin, mouseSensitivityMax);
			SaveMouseSensitivity(ref settingsData);
			DataSaver.SaveData(settingsData);
		}
	}
}

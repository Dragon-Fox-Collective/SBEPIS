using SBEPIS.Controller;
using SBEPIS.Utils;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SBEPIS.Capturllection
{
	public class MenuActions : MonoBehaviour
	{
		[Header("Controls")]
		public SliderCardAttacher sensitivitySlider;
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
			sensitivitySlider.SliderValue = lookController.sensitivity.Map(mouseSensitivityMin, mouseSensitivityMax, 0, 1);
		}
		
		public void ChangeMouseSensitivity(float percent)
		{
			lookController.sensitivity = percent.Map(0, 1, mouseSensitivityMin, mouseSensitivityMax);
			SaveMouseSensitivity(ref settingsData);
			DataSaver.SaveData(settingsData);
		}
	}
}
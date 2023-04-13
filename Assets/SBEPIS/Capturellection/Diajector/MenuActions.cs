using SBEPIS.Controller;
using SBEPIS.Utils;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SBEPIS.Capturellection
{
	public class MenuActions : MonoBehaviour
	{
		public SliderCardAttacher sensitivitySlider;
		public float mouseSensitivityMin = 0.1f;
		public float mouseSensitivityMax = 1;

		private LookController lookController;
		
		private PlayerSettingsSaveData settingsData;
		
		public void StartNewGame()
		{
			StartCoroutine(LoadNewGameScene());
		}
		
		private static IEnumerator LoadNewGameScene()
		{
			AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Demo");
			while (!asyncLoad.isDone)
				yield return null;
		}
		
		public void OpenMainMenu()
		{
			StartCoroutine(LoadMainMenuScene());
		}
		
		private static IEnumerator LoadMainMenuScene()
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
		
		
		public void BindToPlayer(Grabber grabber, Grabbable grabbable)
		{
			if (!grabber.TryGetComponent(out PlayerReference player))
				return;
			lookController = player.GetComponent<LookController>();
			ReloadData();
		}
		
		private void ReloadData()
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
			
			ResetMouseSensitivitySlider();
		}
		
		private void LoadMouseSensitivity(ref PlayerSettingsSaveData settingsData)
		{
			if (settingsData.version >= 0)
				lookController.sensitivity = settingsData.sensitivity;
			else
				SaveMouseSensitivity(ref settingsData);
		}
		private void SaveMouseSensitivity(ref PlayerSettingsSaveData settingsData)
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
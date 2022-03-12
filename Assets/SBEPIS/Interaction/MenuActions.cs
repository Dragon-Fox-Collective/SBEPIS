using System.Collections;
using UnityEngine;
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

		public void OnTogglePauseMenu(CallbackContext context)
		{
			if (!context.performed)
				return;

			pauseButtons.gameObject.SetActive(!pauseButtons.gameObject.activeSelf);

			if (pauseButtons.gameObject.activeSelf)
			{
				pauseButtons.position = playerPosition.position;
				pauseButtons.rotation = Quaternion.Euler(0, yawRotation.rotation.eulerAngles.y, 0);
				main.gameObject.SetActive(true);
				settings.gameObject.SetActive(false);
			}
		}
	}
}

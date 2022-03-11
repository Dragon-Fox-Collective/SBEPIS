using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using CallbackContext = UnityEngine.InputSystem.InputAction.CallbackContext;

namespace SBEPIS.Interaction
{
	public class MenuActions : MonoBehaviour
	{
		public Transform player;
		public Transform pauseButtons;

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
				pauseButtons.position = player.position;
				pauseButtons.rotation = Quaternion.Euler(0, player.rotation.eulerAngles.y, 0);
			}
		}
	}
}

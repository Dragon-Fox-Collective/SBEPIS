//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
#if ARBOR_SUPPORT_UGUI
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Arbor.Examples
{
	[AddComponentMenu("Arbor/Examples/SceneSelector")]
	public sealed class SceneSelector : MonoBehaviour
	{
		public Button buttonPrefab;
		public ScrollRect scrollRect;
		public GameObject canvasInExamplePrefab;
		public string[] scenePaths;
	
		// Start is called before the first frame update
		void Start()
		{
			foreach (var scenePath in scenePaths)
			{
				CreateSceneButton(scenePath);
			}
		}

		void CreateSceneButton(string scenePath)
		{
			var button = Instantiate(buttonPrefab, scrollRect.content);
			button.onClick.AddListener(() =>
			{
				LoadScene(scenePath);
			});
			Text buttonText = button.GetComponentInChildren<Text>();
			
			string sceneName = System.IO.Path.GetFileNameWithoutExtension(scenePath);
			buttonText.text = sceneName;
		}

		void LoadScene(string scenePath)
		{
#if UNITY_EDITOR
			UnityEditor.SceneManagement.EditorSceneManager.LoadSceneInPlayMode(scenePath, new LoadSceneParameters(LoadSceneMode.Single));
#else
			string sceneName = System.IO.Path.GetFileNameWithoutExtension(scenePath);
			SceneManager.LoadScene(sceneName);
#endif

			Instantiate(canvasInExamplePrefab);
		}
	}
}
#endif
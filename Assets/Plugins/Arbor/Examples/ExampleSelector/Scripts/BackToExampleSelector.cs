//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
#if ARBOR_SUPPORT_UGUI
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

namespace Arbor.Examples
{
	[AddComponentMenu("Arbor/Examples/BackToExampleSelector")]
	public sealed class BackToExampleSelector : MonoBehaviour
	{
		private GameObject _EventSystem;

		// Start is called before the first frame update
		IEnumerator Start()
		{
			DontDestroyOnLoad(gameObject);
			yield return null;

			if (EventSystem.current == null)
			{
				_EventSystem = new GameObject("EventSystem", typeof(EventSystem), typeof(StandaloneInputModule));
				DontDestroyOnLoad(_EventSystem);
			}
		}

		public void OnClickBackButton()
		{
			Time.timeScale = 1f;
			SceneManager.LoadScene("ExampleSelector");

			foreach (var obj in ObjectUtility.FindObjectsOfType<GameObject>())
			{
				if (obj.transform.parent != null)
				{
					continue;
				}

				Scene scene = obj.scene;
				if (scene.name == "DontDestroyOnLoad" && scene.path == "DontDestroyOnLoad" && scene.buildIndex == -1)
				{
					Destroy(obj);
				}
			}
		}
	}
}
#endif
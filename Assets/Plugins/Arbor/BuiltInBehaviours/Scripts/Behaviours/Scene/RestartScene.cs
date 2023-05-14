//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.SceneManagement;
using System.Collections;

namespace Arbor.StateMachine.StateBehaviours
{
#if ARBOR_DOC_JA
	/// <summary>
	/// 現在のアクティブシーンをリスタートする。
	/// </summary>
#else
	/// <summary>
	/// Restart the current active scene.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Scene/RestartScene")]
	[BuiltInBehaviour]
	public sealed class RestartScene : StateBehaviour
	{
#if ARBOR_DOC_JA
		/// <summary>
		/// シーンの読み込みが完了したときに遷移する
		/// </summary>
#else
		/// <summary>
		/// Transition when the scene loading is completed
		/// </summary>
#endif
		[SerializeField]
		private StateLink _Done = new StateLink();

		// Use this for enter state
		public override void OnStateBegin()
		{
			var scene = SceneManager.GetActiveScene();

			StartCoroutine(WaitLoad(scene.name));
		}

		IEnumerator WaitLoad(string sceneName)
		{
			yield return SceneManager.LoadSceneAsync(sceneName);

			Transition(_Done);
		}
	}
}

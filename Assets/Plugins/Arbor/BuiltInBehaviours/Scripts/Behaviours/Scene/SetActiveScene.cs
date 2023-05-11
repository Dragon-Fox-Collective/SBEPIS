//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

namespace Arbor.StateMachine.StateBehaviours
{
#if ARBOR_DOC_JA
	/// <summary>
	/// シーンをアクティブにする。
	/// </summary>
#else
	/// <summary>
	/// Set the scene to be active.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Scene/SetActiveScene")]
	[BuiltInBehaviour]
	public sealed class SetActiveScene : StateBehaviour
	{
		#region Serialize fields

#if ARBOR_DOC_JA
		/// <summary>
		/// 読み込むシーンの名前。
		/// </summary>
#else
		/// <summary>
		/// The name of the load scene.
		/// </summary>
#endif
		[SerializeField]
		private FlexibleString _SceneName = new FlexibleString(string.Empty);

#if ARBOR_DOC_JA
		/// <summary>
		/// アクティブシーンの切り替え完了での遷移。<br />
		/// 遷移メソッド : OnStateBegin, Coroutine
		/// </summary>
#else
		/// <summary>
		/// Transition at done of of switching of active scene.<br />
		/// Transition Method : OnStateBegin, Coroutine
		/// </summary>
#endif
		[SerializeField]
		private StateLink _Done = new StateLink();

		#endregion // Serialize fields

		// Use this for enter state
		public override void OnStateBegin()
		{
			Scene scene = SceneManager.GetSceneByName(_SceneName.value);
			if (scene.IsValid())
			{
				StartCoroutine(WaitLoad(scene));
			}
		}

		IEnumerator WaitLoad(Scene scene)
		{
			while (!scene.isLoaded)
			{
				yield return null;
			}

			SceneManager.SetActiveScene(scene);

			Transition(_Done);
		}
	}
}

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
	/// 指定したシーンを読み込む。
	/// </summary>
#else
	/// <summary>
	/// Load the specified scene.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Scene/LoadScene")]
	[BuiltInBehaviour]
	public sealed class LoadScene : StateBehaviour, INodeBehaviourSerializationCallbackReceiver
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
		[FormerlySerializedAs("_LevelName")]
		private FlexibleString _SceneName = new FlexibleString(string.Empty);

#if ARBOR_DOC_JA
		/// <summary>
		/// シーンの読み込みモード
		/// </summary>
#else
		/// <summary>
		/// Load scene mode
		/// </summary>
#endif
		[SerializeField]
		private FlexibleLoadSceneMode _LoadSceneMode = new FlexibleLoadSceneMode(LoadSceneMode.Single);

#if ARBOR_DOC_JA
		/// <summary>
		/// ロードしたシーンをアクティブにする(LoadSceneMode.Additiveのみ)。
		/// </summary>
#else
		/// <summary>
		///Activate the loaded scene.(LoadSceneMode.Additive only)
		/// </summary>
#endif
		[SerializeField]
		private FlexibleBool _IsActiveScene = new FlexibleBool(false);

#if ARBOR_DOC_JA
		/// <summary>
		/// シーンのロード完了での遷移。<br />
		/// 遷移メソッド : Coroutine
		/// </summary>
#else
		/// <summary>
		/// Transition at done of scene loading<br />
		/// Transition Method : Coroutine
		/// </summary>
#endif
		[SerializeField]
		private StateLink _Done = new StateLink();

		[SerializeField]
		[HideInInspector]
		private int _SerializeVersion = 0;

		#region old

		[SerializeField]
		[FormerlySerializedAs("_LevelName")]
		[HideInInspector]
		private string _OldLevelName = string.Empty;

		[SerializeField]
		[HideInInspector]
		[FormerlySerializedAs("_Additive")]
		[FormerlySerializedAs("_LoadSceneMode")]
		private LoadSceneMode _OldLoadSceneMode = LoadSceneMode.Single;

		#endregion // old

		#endregion // Serialize fields

		private const int kCurrentSerializeVersion = 2;

		// Use this for enter state
		public override void OnStateBegin()
		{
			string sceneName = _SceneName.value;
			LoadSceneMode loadSceneMode = _LoadSceneMode.value;

			StartCoroutine(WaitLoad(sceneName, loadSceneMode));
		}

		IEnumerator WaitLoad(string sceneName, LoadSceneMode loadSceneMode)
		{
			yield return SceneManager.LoadSceneAsync(sceneName, loadSceneMode);

			Scene scene = SceneManager.GetSceneByName(sceneName);
			if (scene.IsValid())
			{
				if (loadSceneMode == LoadSceneMode.Additive && _IsActiveScene.value)
				{
					SceneManager.SetActiveScene(scene);
				}
			}
			else
			{
				Debug.LogWarning($"{sceneName} is invalid");
			}

			Transition(_Done);
		}

		void Reset()
		{
			_SerializeVersion = kCurrentSerializeVersion;
		}

		void SerializeVer1()
		{
			_SceneName = (FlexibleString)_OldLevelName;
		}

		void SerializeVer2()
		{
			_LoadSceneMode = (FlexibleLoadSceneMode)_OldLoadSceneMode;
		}

		void Serialize()
		{
			while (_SerializeVersion != kCurrentSerializeVersion)
			{
				switch (_SerializeVersion)
				{
					case 0:
						SerializeVer1();
						_SerializeVersion++;
						break;
					case 1:
						SerializeVer2();
						_SerializeVersion++;
						break;
					default:
						_SerializeVersion = kCurrentSerializeVersion;
						break;
				}
			}
		}

		void INodeBehaviourSerializationCallbackReceiver.OnAfterDeserialize()
		{
			Serialize();
		}

		void INodeBehaviourSerializationCallbackReceiver.OnBeforeSerialize()
		{
			Serialize();
		}
	}
}

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
	/// 指定したシーンを現在シーンからアンロードする。
	/// </summary>
#else
	/// <summary>
	/// Unload the specified scene from the current scene.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Scene/UnloadScene")]
	[BuiltInBehaviour]
	public sealed class UnloadScene : StateBehaviour, INodeBehaviourSerializationCallbackReceiver
	{
		#region Serialize fields

#if ARBOR_DOC_JA
		/// <summary>
		/// アンロードするシーンの名前。
		/// </summary>
#else
		/// <summary>
		/// The name of the scene to be unloaded.
		/// </summary>
#endif
		[SerializeField]
		[FormerlySerializedAs("_LevelName")]
		private FlexibleString _SceneName = new FlexibleString(string.Empty);

#if ARBOR_DOC_JA
		/// <summary>
		/// シーンのアンロードが完了したときに遷移する
		/// </summary>
#else
		/// <summary>
		/// Transition when scene unloading is complete
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

		#endregion // old

		#endregion // Serialize fields

		private const int kCurrentSerializeVersion = 1;

		// Use this for enter state
		public override void OnStateBegin()
		{
			string sceneName = _SceneName.value;
			StartCoroutine(WaitUnload(sceneName));
		}

		public override void OnStateEnd()
		{
			StopAllCoroutines();
		}

		IEnumerator WaitUnload(string sceneName)
		{
			yield return SceneManager.UnloadSceneAsync(sceneName);

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

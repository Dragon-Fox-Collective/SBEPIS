//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Arbor.StateMachine.StateBehaviours
{
#if ARBOR_DOC_JA
	/// <summary>
	/// ステートがアクティブになった際に、GameObjectを他のシーンに移動させる。
	/// </summary>
	/// <remarks>
	/// 詳しくは、<see cref="SceneManager.MoveGameObjectToScene(GameObject, Scene)"/>を参照してください。
	/// </remarks>
#else
	/// <summary>
	/// Move the GameObject to another scene when the state becomes active.
	/// </summary>
	/// <remarks>
	/// For more information, see <see cref="SceneManager.MoveGameObjectToScene(GameObject, Scene)"/>.
	/// </remarks>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("GameObject/MoveGameObjectToScene")]
	[BuiltInBehaviour]
	public sealed class MoveGameObjectToScene : StateBehaviour
	{
#if ARBOR_DOC_JA
		/// <summary>
		/// 移動させるGameObject。シーンのルートに配置しているGameObjectのみが有効。
		/// </summary>
#else
		/// <summary>
		/// GameObject to move. Only valid for GameObjects placed at the root of the scene.
		/// </summary>
#endif
		[SerializeField]
		private FlexibleGameObject _GameObject = new FlexibleGameObject(FlexibleHierarchyType.Self);

#if ARBOR_DOC_JA
		/// <summary>
		/// 移動先のシーン名
		/// </summary>
#else
		/// <summary>
		/// Scene name of the destination
		/// </summary>
#endif
		[SerializeField]
		private FlexibleString _SceneName = new FlexibleString();

		public override void OnStateBegin()
		{
			GameObject gameObject = _GameObject.value;
			if (gameObject == null)
			{
				return;
			}

			string sceneName = _SceneName.value;
			Scene scene = SceneManager.GetSceneByName(sceneName);
			if (!scene.IsValid())
			{
				return;
			}

			SceneManager.MoveGameObjectToScene(gameObject, scene);
		}
	}
}
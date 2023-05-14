//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arbor.StateMachine.StateBehaviours
{
#if ARBOR_DOC_JA
	/// <summary>
	/// ステートがアクティブになった際に指定したGameObjectをDontDestroyOnLoadに設定する。
	/// </summary>
#else
	/// <summary>
	/// Set the specified GameObject to DontDestroyOnLoad when the state becomes active.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("GameObject/SetGameObjectDontDestroyOnLoad")]
	[BuiltInBehaviour]
	public sealed class SetGameObjectDontDestroyOnLoad : StateBehaviour
	{
#if ARBOR_DOC_JA
		/// <summary>
		/// DontDestroyOnLoadに設定するGameObject
		/// </summary>
#else
		/// <summary>
		/// GameObject set to DontDestroyOnLoad
		/// </summary>
#endif
		[SerializeField]
		private FlexibleGameObject _GameObject = new FlexibleGameObject(FlexibleHierarchyType.Self);

		public override void OnStateBegin()
		{
			GameObject gameObject = _GameObject.value;
			if (gameObject == null)
			{
				return;
			}

			DontDestroyOnLoad(gameObject);
		}
	}
}
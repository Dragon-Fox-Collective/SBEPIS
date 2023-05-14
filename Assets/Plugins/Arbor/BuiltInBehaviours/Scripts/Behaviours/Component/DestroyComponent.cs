//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Arbor.StateMachine.StateBehaviours
{
#if ARBOR_DOC_JA
	/// <summary>
	/// コンポーネントを削除する
	/// </summary>
#else
	/// <summary>
	/// Destroy the component
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Component/DestroyComponent")]
	[BuiltInBehaviour]
	public class DestroyComponent : StateBehaviour
	{
#if ARBOR_DOC_JA
		/// <summary>
		/// 削除するコンポーネント
		/// </summary>
#else
		/// <summary>
		/// Components to destroy
		/// </summary>
#endif
		[SerializeField]
		private FlexibleComponent _Component = new FlexibleComponent();

		// Use this for enter state
		public override void OnStateBegin()
		{
			var component = _Component.value;
			if (component != null)
			{
				Destroy(component);
			}
		}
	}
}

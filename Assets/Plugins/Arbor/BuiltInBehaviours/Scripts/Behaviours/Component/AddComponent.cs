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
	/// コンポーネントを追加する。
	/// </summary>
#else
	/// <summary>
	/// Add a component.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Component/AddComponent")]
	[BuiltInBehaviour]
	public sealed class AddComponent : StateBehaviour
	{
#if ARBOR_DOC_JA
		/// <summary>
		/// コンポーネントを追加するGameObject
		/// </summary>
#else
		/// <summary>
		/// GameObject to add a component
		/// </summary>
#endif
		[SerializeField]
		private FlexibleGameObject _GameObject = new FlexibleGameObject(FlexibleHierarchyType.Self);

#if ARBOR_DOC_JA
		/// <summary>
		/// 追加したコンポーネント
		/// </summary>
#else
		/// <summary>
		/// Added component
		/// </summary>
#endif
		[SerializeField]
		private OutputSlotComponent _Output = new OutputSlotComponent();

		// Use this for enter state
		public override void OnStateBegin()
		{
			var gameObject = _GameObject.value;
			if (gameObject != null)
			{
				var type = _Output.dataType;

				_Output.SetValue(gameObject.AddComponent(type));
			}
		}
	}
}
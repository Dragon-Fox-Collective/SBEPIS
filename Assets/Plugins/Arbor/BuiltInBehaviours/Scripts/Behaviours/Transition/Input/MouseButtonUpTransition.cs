//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
#if ENABLE_LEGACY_INPUT_MANAGER
using UnityEngine;

namespace Arbor.StateMachine.StateBehaviours
{
#if ARBOR_DOC_JA
	/// <summary>
	/// マウスボタンが離されたときにステートを遷移する。
	/// </summary>
#else
	/// <summary>
	/// It will transition the state when the mouse button is released.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Transition/Input/MouseButtonUpTransition")]
	[BuiltInBehaviour]
	public sealed class MouseButtonUpTransition : MouseButtonBehaviourBase
	{
		protected override bool IsInput(int button)
		{
			return Input.GetMouseButtonUp(button);
		}
	}
}
#endif
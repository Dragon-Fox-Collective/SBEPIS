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
	/// マウスボタンが押されたときにステートを遷移する。
	/// </summary>
#else
	/// <summary>
	/// It will transition the state when the mouse button is pressed.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Transition/Input/MouseButtonDownTransition")]
	[BuiltInBehaviour]
	public sealed class MouseButtonDownTransition : MouseButtonBehaviourBase
	{
		protected override bool IsInput(int button)
		{
			return Input.GetMouseButtonDown(button);
		}
	}
}
#endif
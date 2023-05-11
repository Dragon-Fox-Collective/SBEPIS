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
	/// マウスボタンが押されているかでステートを遷移する。
	/// </summary>
#else
	/// <summary>
	/// It will transition the state on whether the mouse button is pressed.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Transition/Input/MouseButtonTransition")]
	[BuiltInBehaviour]
	public sealed class MouseButtonTransition : MouseButtonBehaviourBase
	{
		protected override bool IsInput(int button)
		{
			return Input.GetMouseButton(button);
		}
	}
}
#endif
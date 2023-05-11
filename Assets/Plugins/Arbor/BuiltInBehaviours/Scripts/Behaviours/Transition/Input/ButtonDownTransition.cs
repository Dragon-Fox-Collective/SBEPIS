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
	/// ボタンが押されたときにステートを遷移する。
	/// </summary>
#else
	/// <summary>
	/// It will transition the state when the button is pressed.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Transition/Input/ButtonDownTransition")]
	[BuiltInBehaviour]
	public sealed class ButtonDownTransition : ButtonBehaviourBase
	{
		#region Serialize fields

#if ARBOR_DOC_JA
		/// <summary>
		/// 遷移先ステート。<br />
		/// 遷移メソッド : Update
		/// </summary>
#else
		/// <summary>
		/// Transition destination state.<br />
		/// Transition Method : Update
		/// </summary>
#endif
		[SerializeField] private StateLink _NextState = new StateLink();

		#endregion // Serialize fields

		// Update is called once per frame
		protected override void OnUpdate()
		{
			if (Input.GetButtonDown(buttonName))
			{
				Transition(_NextState);
			}
		}
	}
}
#endif
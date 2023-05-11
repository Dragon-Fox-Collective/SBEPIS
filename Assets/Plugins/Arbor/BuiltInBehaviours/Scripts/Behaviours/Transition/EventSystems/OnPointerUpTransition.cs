//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
#if ARBOR_SUPPORT_UGUI
using UnityEngine;
using UnityEngine.EventSystems;

namespace Arbor.StateMachine.StateBehaviours
{
#if ARBOR_DOC_JA
	/// <summary>
	/// OnPointerUpが呼ばれたときにステートを遷移する。
	/// </summary>
#else
	/// <summary>
	/// It will transition the state when the OnPointerUp is called.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Transition/EventSystems/OnPointerUpTransition")]
	[BuiltInBehaviour]
	public sealed class OnPointerUpTransition : PointerBehaviourBase, IPointerUpHandler
	{
		#region Serialize fields

#if ARBOR_DOC_JA
		/// <summary>
		/// 遷移先ステート。<br />
		/// 遷移メソッド : OnPointerUp
		/// </summary>
#else
		/// <summary>
		/// Transition destination state.<br />
		/// Transition Method : OnPointerUp
		/// </summary>
#endif
		[SerializeField] private StateLink _NextState = new StateLink();

		#endregion // Serialize fields

		public void OnPointerUp(PointerEventData data)
		{
			if (CheckButton(data.button))
			{
				Transition(_NextState);
			}
		}
	}
}
#endif

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
	/// OnPointerDownが呼ばれたときにステートを遷移する。
	/// </summary>
#else
	/// <summary>
	/// It will transition the state when the OnPointerDown is called.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Transition/EventSystems/OnPointerDownTransition")]
	[BuiltInBehaviour]
	public sealed class OnPointerDownTransition : PointerBehaviourBase, IPointerDownHandler
	{
		#region Serialize fields

#if ARBOR_DOC_JA
		/// <summary>
		/// 遷移先ステート。<br />
		/// 遷移メソッド : OnPointerDown
		/// </summary>
#else
		/// <summary>
		/// Transition destination state.<br />
		/// Transition Method : OnPointerDown
		/// </summary>
#endif
		[SerializeField]
		private StateLink _NextState = new StateLink();

		#endregion // Serialize fields

		public void OnPointerDown(PointerEventData data)
		{
			if (CheckButton(data.button))
			{
				Transition(_NextState);
			}
		}
	}
}
#endif

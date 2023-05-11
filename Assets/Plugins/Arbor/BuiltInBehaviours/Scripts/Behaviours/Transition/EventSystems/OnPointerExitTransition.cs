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
	/// OnPointerExitが呼ばれたときにステートを遷移する。
	/// </summary>
#else
	/// <summary>
	/// It will transition the state when the OnPointerExit is called.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Transition/EventSystems/OnPointerExitTransition")]
	[BuiltInBehaviour]
	public sealed class OnPointerExitTransition : StateBehaviour, IPointerExitHandler
	{
		#region Serialize fields

#if ARBOR_DOC_JA
		/// <summary>
		/// 遷移先ステート。<br />
		/// 遷移メソッド : OnPointerExit
		/// </summary>
#else
		/// <summary>
		/// Transition destination state.<br />
		/// Transition Method : OnPointerExit
		/// </summary>
#endif
		[SerializeField] private StateLink _NextState = new StateLink();

		#endregion // Serialize fields

		public void OnPointerExit(PointerEventData data)
		{
			Transition(_NextState);
		}
	}
}
#endif

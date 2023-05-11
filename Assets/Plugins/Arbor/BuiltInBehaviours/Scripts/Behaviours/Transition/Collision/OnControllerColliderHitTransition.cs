//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.StateMachine.StateBehaviours
{
#if ARBOR_DOC_JA
	/// <summary>
	/// OnControllerColliderHitが呼ばれたときにステートを遷移する。
	/// </summary>
#else
	/// <summary>
	/// It will transition the state when the OnControllerColliderHit is called.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Transition/Collision/OnControllerColliderHitTransition")]
	[BuiltInBehaviour]
	public sealed class OnControllerColliderHitTransition : CheckTagBehaviourBase
	{
		#region Serialize fields

#if ARBOR_DOC_JA
		/// <summary>
		/// ControllerColliderHitの出力
		/// </summary>
#else
		/// <summary>
		/// Output ControllerColliderHit
		/// </summary>
#endif
		[SerializeField]
		[SlotType(typeof(ControllerColliderHit))]
		private OutputSlotAny _Hit = new OutputSlotAny();

#if ARBOR_DOC_JA
		/// <summary>
		/// 遷移先ステート。<br />
		/// 遷移メソッド : OnControllerColliderHit
		/// </summary>
#else
		/// <summary>
		/// Transition destination state.<br />
		/// Transition Method : OnControllerColliderHit
		/// </summary>
#endif
		[SerializeField]
		private StateLink _NextState = new StateLink();

		#endregion // Serialize fields

		void OnControllerColliderHit(ControllerColliderHit hit)
		{
			if (!enabled)
			{
				return;
			}

			if (CheckTag(hit.gameObject))
			{
				_Hit.SetValue(hit);
				Transition(_NextState);
			}
		}
	}
}

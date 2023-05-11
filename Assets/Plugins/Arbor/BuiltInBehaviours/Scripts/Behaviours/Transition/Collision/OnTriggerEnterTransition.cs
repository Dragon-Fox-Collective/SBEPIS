//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.StateMachine.StateBehaviours
{
#if ARBOR_DOC_JA
	/// <summary>
	/// OnTriggerEnterが呼ばれたときにステートを遷移する。
	/// </summary>
#else
	/// <summary>
	/// It will transition the state when the OnTriggerEnter is called.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Transition/Collision/OnTriggerEnterTransition")]
	[BuiltInBehaviour]
	public sealed class OnTriggerEnterTransition : CheckTagBehaviourBase
	{
		#region Serialize fields

#if ARBOR_DOC_JA
		/// <summary>
		/// Colliderの出力。
		/// </summary>
#else
		/// <summary>
		/// Collider output.
		/// </summary>
#endif
		[SerializeField] private OutputSlotCollider _Collider = new OutputSlotCollider();

#if ARBOR_DOC_JA
		/// <summary>
		/// 遷移先ステート。<br />
		/// 遷移メソッド : OnTriggerEnter
		/// </summary>
#else
		/// <summary>
		/// Transition destination state.<br />
		/// Transition Method : OnTriggerEnter
		/// </summary>
#endif
		[SerializeField] private StateLink _NextState = new StateLink();

		#endregion // Serialize fields

		void OnTriggerEnter(Collider collider)
		{
			if (!enabled)
			{
				return;
			}

			if (CheckTag(collider.gameObject))
			{
				_Collider.SetValue(collider);
				Transition(_NextState);
			}
		}
	}
}

﻿//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.StateMachine.StateBehaviours
{
#if ARBOR_DOC_JA
	/// <summary>
	/// OnTriggerStay2Dが呼ばれたときにステートを遷移する。
	/// </summary>
#else
	/// <summary>
	/// It will transition the state when the OnTriggerStay2D is called.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Transition/Collision2D/OnTriggerStay2DTransition")]
	[BuiltInBehaviour]
	public sealed class OnTriggerStay2DTransition : CheckTagBehaviourBase
	{
		#region Serialize fields

#if ARBOR_DOC_JA
		/// <summary>
		/// Collider2Dの出力。
		/// </summary>
#else
		/// <summary>
		/// Collider2D output.
		/// </summary>
#endif
		[SerializeField] private OutputSlotCollider2D _Collider2D = new OutputSlotCollider2D();

#if ARBOR_DOC_JA
		/// <summary>
		/// 遷移先ステート。<br />
		/// 遷移メソッド : OnTriggerStay2D
		/// </summary>
#else
		/// <summary>
		/// Transition destination state.<br />
		/// Transition Method : OnTriggerStay2D
		/// </summary>
#endif
		[SerializeField] private StateLink _NextState = new StateLink();

		#endregion // Serialize fields

		void OnTriggerStay2D(Collider2D collider)
		{
			if (!enabled)
			{
				return;
			}

			if (CheckTag(collider))
			{
				_Collider2D.SetValue(collider);
				Transition(_NextState);
			}
		}
	}
}

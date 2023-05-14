//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.StateMachine.StateBehaviours
{
	using Arbor.Utilities;

#if ARBOR_DOC_JA
	/// <summary>
	/// OnCollisionStay2Dが呼ばれたときにステートを遷移する。
	/// </summary>
	/// <remarks>Unity 2018.3.0以降 : Collision2Dを出力する場合は、Physics2D Settingsの「Reuse Collision Callbacks」を無効にする必要があります。</remarks>
#else
	/// <summary>
	/// It will transition the state when the OnCollisionStay2D is called.
	/// </summary>
	/// <remarks>Unity 2018.3.0 or later : If you want to output Collision2D, you need to disable "Reuse Collision Callbacks" in Physics2D Settings.</remarks>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Transition/Collision2D/OnCollisionStay2DTransition")]
	[BuiltInBehaviour]
	public sealed class OnCollisionStay2DTransition : CheckTagBehaviourBase
	{
		#region Serialize fields

#if ARBOR_DOC_JA
		/// <summary>
		/// Collision2Dの出力。
		/// </summary>
#else
		/// <summary>
		/// Collision2D output.
		/// </summary>
#endif
		[SerializeField] private OutputSlotCollision2D _Collision2D = new OutputSlotCollision2D();

#if ARBOR_DOC_JA
		/// <summary>
		/// 遷移先ステート。<br />
		/// 遷移メソッド : OnCollisionStay2D
		/// </summary>
#else
		/// <summary>
		/// Transition destination state.<br />
		/// Transition Method : OnCollisionStay2D
		/// </summary>
#endif
		[SerializeField] private StateLink _NextState = new StateLink();

		#endregion // Serialize fields

		void OnCollisionStay2D(Collision2D collision)
		{
			if (!enabled)
			{
				return;
			}

			if (CheckTag(collision.gameObject))
			{
				Physics2DUtility.CheckReuseCollision2D(_Collision2D);
				_Collision2D.SetValue(collision);
				Transition(_NextState);
			}
		}
	}
}

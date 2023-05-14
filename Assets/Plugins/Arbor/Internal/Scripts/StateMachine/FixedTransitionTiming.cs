//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using System;

namespace Arbor
{
#if ARBOR_DOC_JA
	/// <summary>
	/// StateLinkが遷移タイミングを固定した状態であることを設定。
	/// この指定とは別にTransitionメソッドのtransitionTiming引数も指定すること。
	/// </summary>
#else
	/// <summary>
	/// Setting the StateLink is in a state of fixing an immediate transition flags.
	/// This specified separately that it also specify the transitionTiming argument of Transition method with.
	/// </summary>
#endif
	[AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
	public sealed class FixedTransitionTiming : Attribute
	{
		private TransitionTiming _TransitionTiming;

#if ARBOR_DOC_JA
		/// <summary>
		/// 遷移タイミング
		/// </summary>
#else
		/// <summary>
		/// Transition timing
		/// </summary>
#endif
		public TransitionTiming transitionTiming
		{
			get
			{
				return _TransitionTiming;
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// FixedTransitionTimingのコンストラクタ
		/// </summary>
		/// <param name="transitionTiming">遷移タイミング</param>
#else
		/// <summary>
		/// FixedTransitionTiming constructor
		/// </summary>
		/// <param name="transitionTiming">Transition timing</param>
#endif
		public FixedTransitionTiming(TransitionTiming transitionTiming)
		{
			_TransitionTiming = transitionTiming;
		}
	}
}
//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using System;

namespace Arbor
{
#if ARBOR_DOC_JA
	/// <summary>
	/// StateLinkが即時遷移フラグを固定した状態であることを設定。
	/// この指定とは別にTransitionメソッドのimmediate引数も指定すること。
	/// </summary>
#else
	/// <summary>
	/// Setting the StateLink is in a state of fixing an immediate transition flags.
	/// This specified separately that it also specify the immediate argument of Transition method with.
	/// </summary>
#endif
	[AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
	public sealed class FixedImmediateTransition : Attribute
	{
		private bool _Immediate;

#if ARBOR_DOC_JA
		/// <summary>
		/// 即時フラグ。
		/// </summary>
#else
		/// <summary>
		/// immediate flag.
		/// </summary>
#endif
		public bool immediate
		{
			get
			{
				return _Immediate;
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// FixedImmediateTransitionコンストラクタ
		/// </summary>
		/// <param name="immediate">即時フラグ</param>
#else
		/// <summary>
		/// FixedImmediateTransition constructor
		/// </summary>
		/// <param name="immediate">Immediate flag</param>
#endif
		public FixedImmediateTransition(bool immediate)
		{
			_Immediate = immediate;
		}
	}
}

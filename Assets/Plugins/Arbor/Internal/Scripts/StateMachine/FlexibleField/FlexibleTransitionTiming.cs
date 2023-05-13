//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
namespace Arbor
{
#if ARBOR_DOC_JA
	/// <summary>
	/// 参照方法が複数ある柔軟なTransitionTiming型を扱うクラス。
	/// </summary>
#else
	/// <summary>
	/// Class to handle a flexible TransitionTiming type reference method there is more than one.
	/// </summary>
#endif
	[System.Serializable]
	public sealed class FlexibleTransitionTiming : FlexibleField<TransitionTiming>
	{
#if ARBOR_DOC_JA
		/// <summary>
		/// FlexibleTransitionTimingデフォルトコンストラクタ
		/// </summary>
#else
		/// <summary>
		/// FlexibleTransitionTiming default constructor
		/// </summary>
#endif
		public FlexibleTransitionTiming()
		{
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// FlexibleTransitionTimingコンストラクタ
		/// </summary>
		/// <param name="value">値</param>
#else
		/// <summary>
		/// FlexibleTransitionTiming constructor
		/// </summary>
		/// <param name="value">Value</param>
#endif
		public FlexibleTransitionTiming(TransitionTiming value) : base(value)
		{
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// FlexibleTransitionTimingコンストラクタ
		/// </summary>
		/// <param name="parameter">パラメータ</param>
#else
		/// <summary>
		/// FlexibleTransitionTiming constructor
		/// </summary>
		/// <param name="parameter">Parameter</param>
#endif
		public FlexibleTransitionTiming(AnyParameterReference parameter) : base(parameter)
		{
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// FlexibleTransitionTimingコンストラクタ
		/// </summary>
		/// <param name="slot">スロット</param>
#else
		/// <summary>
		/// FlexibleTransitionTiming constructor
		/// </summary>
		/// <param name="slot">Slot</param>
#endif
		public FlexibleTransitionTiming(InputSlotAny slot) : base(slot)
		{
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// FlexibleTransitionTimingをTransitionTimingにキャスト。
		/// </summary>
		/// <param name="flexible">FlexibleTransitionTiming</param>
		/// <returns>TransitionTimingにキャストした結果を返す。</returns>
#else
		/// <summary>
		/// Cast FlexibleTransitionTiming to TransitionTiming.
		/// </summary>
		/// <param name="flexible">FlexibleTransitionTiming</param>
		/// <returns>Returns the result of casting to TransitionTiming.</returns>
#endif
		public static explicit operator TransitionTiming(FlexibleTransitionTiming flexible)
		{
			return flexible.value;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// TransitionTimingをFlexibleTransitionTimingにキャスト。
		/// </summary>
		/// <param name="value">TransitionTiming</param>
		/// <returns>FlexibleTransitionTimingにキャストした結果を返す。</returns>
#else
		/// <summary>
		/// Cast TransitionTiming to FlexibleTransitionTiming.
		/// </summary>
		/// <param name="value">TransitionTiming</param>
		/// <returns>Returns the result of casting to FlexibleTransitionTiming.</returns>
#endif
		public static explicit operator FlexibleTransitionTiming(TransitionTiming value)
		{
			return new FlexibleTransitionTiming(value);
		}
	}
}
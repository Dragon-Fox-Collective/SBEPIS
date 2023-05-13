//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
namespace Arbor
{
	using System;

#if ARBOR_DOC_JA
	/// <summary>
	/// 参照方法が複数ある柔軟なStringComparison型を扱うクラス。
	/// </summary>
#else
	/// <summary>
	/// Class to handle a flexible StringComparison type reference method there is more than one.
	/// </summary>
#endif
	[Serializable]
	public sealed class FlexibleStringComparison : FlexibleField<StringComparison>
	{
#if ARBOR_DOC_JA
		/// <summary>
		/// FlexibleStringComparisonデフォルトコンストラクタ
		/// </summary>
#else
		/// <summary>
		/// FlexibleStringComparison default constructor
		/// </summary>
#endif
		public FlexibleStringComparison()
		{
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// FlexibleStringComparisonコンストラクタ
		/// </summary>
		/// <param name="value">値</param>
#else
		/// <summary>
		/// FlexibleStringComparison constructor
		/// </summary>
		/// <param name="value">Value</param>
#endif
		public FlexibleStringComparison(StringComparison value) : base(value)
		{
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// FlexibleStringComparisonコンストラクタ
		/// </summary>
		/// <param name="parameter">パラメータ</param>
#else
		/// <summary>
		/// FlexibleStringComparison constructor
		/// </summary>
		/// <param name="parameter">Parameter</param>
#endif
		public FlexibleStringComparison(AnyParameterReference parameter) : base(parameter)
		{
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// FlexibleStringComparisonコンストラクタ
		/// </summary>
		/// <param name="slot">スロット</param>
#else
		/// <summary>
		/// FlexibleStringComparison constructor
		/// </summary>
		/// <param name="slot">Slot</param>
#endif
		public FlexibleStringComparison(InputSlotAny slot) : base(slot)
		{
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// FlexibleStringComparisonをStringComparisonにキャスト。
		/// </summary>
		/// <param name="flexible">FlexibleStringComparison</param>
		/// <returns>StringComparisonにキャストした結果を返す。</returns>
#else
		/// <summary>
		/// Cast FlexibleStringComparison to StringComparison.
		/// </summary>
		/// <param name="flexible">FlexibleStringComparison</param>
		/// <returns>Returns the result of casting to StringComparison.</returns>
#endif
		public static explicit operator StringComparison(FlexibleStringComparison flexible)
		{
			return flexible.value;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// StringComparisonをFlexibleStringComparisonにキャスト。
		/// </summary>
		/// <param name="value">StringComparison</param>
		/// <returns>FlexibleStringComparisonにキャストした結果を返す。</returns>
#else
		/// <summary>
		/// Cast StringComparison to FlexibleStringComparison.
		/// </summary>
		/// <param name="value">StringComparison</param>
		/// <returns>Returns the result of casting to FlexibleStringComparison.</returns>
#endif
		public static explicit operator FlexibleStringComparison(StringComparison value)
		{
			return new FlexibleStringComparison(value);
		}
	}
}
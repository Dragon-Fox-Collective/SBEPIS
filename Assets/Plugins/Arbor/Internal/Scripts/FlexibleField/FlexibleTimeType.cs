//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
namespace Arbor
{
#if ARBOR_DOC_JA
	/// <summary>
	/// 参照方法が複数ある柔軟なTimeType型を扱うクラス。
	/// </summary>
#else
	/// <summary>
	/// Class to handle a flexible TimeType type reference method there is more than one.
	/// </summary>
#endif
	[System.Serializable]
	public sealed class FlexibleTimeType : FlexibleField<TimeType>
	{
#if ARBOR_DOC_JA
		/// <summary>
		/// FlexibleTimeTypeデフォルトコンストラクタ
		/// </summary>
#else
		/// <summary>
		/// FlexibleTimeType default constructor
		/// </summary>
#endif
		public FlexibleTimeType()
		{
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// FlexibleTimeTypeコンストラクタ
		/// </summary>
		/// <param name="value">値</param>
#else
		/// <summary>
		/// FlexibleTimeType constructor
		/// </summary>
		/// <param name="value">Value</param>
#endif
		public FlexibleTimeType(TimeType value) : base(value)
		{
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// FlexibleTimeTypeコンストラクタ
		/// </summary>
		/// <param name="parameter">パラメータ</param>
#else
		/// <summary>
		/// FlexibleTimeType constructor
		/// </summary>
		/// <param name="parameter">Parameter</param>
#endif
		public FlexibleTimeType(AnyParameterReference parameter) : base(parameter)
		{
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// FlexibleTimeTypeコンストラクタ
		/// </summary>
		/// <param name="slot">スロット</param>
#else
		/// <summary>
		/// FlexibleTimeType constructor
		/// </summary>
		/// <param name="slot">Slot</param>
#endif
		public FlexibleTimeType(InputSlotAny slot) : base(slot)
		{
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// FlexibleTimeTypeをTimeTypeにキャスト。
		/// </summary>
		/// <param name="flexible">FlexibleTimeType</param>
		/// <returns>TimeTypeにキャストした結果を返す。</returns>
#else
		/// <summary>
		/// Cast FlexibleTimeType to TimeType.
		/// </summary>
		/// <param name="flexible">FlexibleTimeType</param>
		/// <returns>Returns the result of casting to TimeType.</returns>
#endif
		public static explicit operator TimeType(FlexibleTimeType flexible)
		{
			return flexible.value;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// TimeTypeをFlexibleTimeTypeにキャスト。
		/// </summary>
		/// <param name="value">TimeType</param>
		/// <returns>FlexibleTimeTypeにキャストした結果を返す。</returns>
#else
		/// <summary>
		/// Cast TimeType to FlexibleTimeType.
		/// </summary>
		/// <param name="value">TimeType</param>
		/// <returns>Returns the result of casting to FlexibleTimeType.</returns>
#endif
		public static explicit operator FlexibleTimeType(TimeType value)
		{
			return new FlexibleTimeType(value);
		}
	}
}
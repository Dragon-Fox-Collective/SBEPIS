//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
namespace Arbor
{
#if ARBOR_DOC_JA
	/// <summary>
	/// 参照方法が複数ある柔軟なSendTriggerFlags型を扱うクラス。
	/// </summary>
#else
	/// <summary>
	/// Class to handle a flexible SendTriggerFlags type reference method there is more than one.
	/// </summary>
#endif
	[System.Serializable]
	public sealed class FlexibleSendTriggerFlags : FlexibleField<SendTriggerFlags>
	{
#if ARBOR_DOC_JA
		/// <summary>
		/// FlexibleSendTriggerFlagsデフォルトコンストラクタ
		/// </summary>
#else
		/// <summary>
		/// FlexibleSendTriggerFlags default constructor
		/// </summary>
#endif
		public FlexibleSendTriggerFlags()
		{
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// FlexibleSendTriggerFlagsコンストラクタ
		/// </summary>
		/// <param name="value">値</param>
#else
		/// <summary>
		/// FlexibleSendTriggerFlags constructor
		/// </summary>
		/// <param name="value">Value</param>
#endif
		public FlexibleSendTriggerFlags(SendTriggerFlags value) : base(value)
		{
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// FlexibleSendTriggerFlagsコンストラクタ
		/// </summary>
		/// <param name="parameter">パラメータ</param>
#else
		/// <summary>
		/// FlexibleSendTriggerFlags constructor
		/// </summary>
		/// <param name="parameter">Parameter</param>
#endif
		public FlexibleSendTriggerFlags(AnyParameterReference parameter) : base(parameter)
		{
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// FlexibleSendTriggerFlagsコンストラクタ
		/// </summary>
		/// <param name="slot">スロット</param>
#else
		/// <summary>
		/// FlexibleSendTriggerFlags constructor
		/// </summary>
		/// <param name="slot">Slot</param>
#endif
		public FlexibleSendTriggerFlags(InputSlotAny slot) : base(slot)
		{
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// FlexibleSendTriggerFlagsをSendTriggerFlagsにキャスト。
		/// </summary>
		/// <param name="flexible">FlexibleSendTriggerFlags</param>
		/// <returns>SendTriggerFlagsにキャストした結果を返す。</returns>
#else
		/// <summary>
		/// Cast FlexibleSendTriggerFlags to SendTriggerFlags.
		/// </summary>
		/// <param name="flexible">FlexibleSendTriggerFlags</param>
		/// <returns>Returns the result of casting to SendTriggerFlags.</returns>
#endif
		public static explicit operator SendTriggerFlags(FlexibleSendTriggerFlags flexible)
		{
			return flexible.value;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// SendTriggerFlagsをFlexibleSendTriggerFlagsにキャスト。
		/// </summary>
		/// <param name="value">SendTriggerFlags</param>
		/// <returns>FlexibleSendTriggerFlagsにキャストした結果を返す。</returns>
#else
		/// <summary>
		/// Cast SendTriggerFlags to FlexibleSendTriggerFlags.
		/// </summary>
		/// <param name="value">SendTriggerFlags</param>
		/// <returns>Returns the result of casting to FlexibleSendTriggerFlags.</returns>
#endif
		public static explicit operator FlexibleSendTriggerFlags(SendTriggerFlags value)
		{
			return new FlexibleSendTriggerFlags(value);
		}
	}
}
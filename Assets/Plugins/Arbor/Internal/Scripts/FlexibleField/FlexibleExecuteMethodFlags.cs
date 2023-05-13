//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
namespace Arbor
{
#if ARBOR_DOC_JA
	/// <summary>
	/// 参照方法が複数ある柔軟なExecuteMethodFlags型を扱うクラス。
	/// </summary>
#else
	/// <summary>
	/// Class to handle a flexible ExecuteMethodFlags type reference method there is more than one.
	/// </summary>
#endif
	[System.Serializable]
	public sealed class FlexibleExecuteMethodFlags : FlexibleField<ExecuteMethodFlags>
	{
#if ARBOR_DOC_JA
		/// <summary>
		/// FlexibleExecuteMethodFlagsデフォルトコンストラクタ
		/// </summary>
#else
		/// <summary>
		/// FlexibleExecuteMethodFlags default constructor
		/// </summary>
#endif
		public FlexibleExecuteMethodFlags()
		{
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// FlexibleExecuteMethodFlagsコンストラクタ
		/// </summary>
		/// <param name="value">値</param>
#else
		/// <summary>
		/// FlexibleExecuteMethodFlags constructor
		/// </summary>
		/// <param name="value">Value</param>
#endif
		public FlexibleExecuteMethodFlags(ExecuteMethodFlags value) : base(value)
		{
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// FlexibleExecuteMethodFlagsコンストラクタ
		/// </summary>
		/// <param name="parameter">パラメータ</param>
#else
		/// <summary>
		/// FlexibleExecuteMethodFlags constructor
		/// </summary>
		/// <param name="parameter">Parameter</param>
#endif
		public FlexibleExecuteMethodFlags(AnyParameterReference parameter) : base(parameter)
		{
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// FlexibleExecuteMethodFlagsコンストラクタ
		/// </summary>
		/// <param name="slot">スロット</param>
#else
		/// <summary>
		/// FlexibleExecuteMethodFlags constructor
		/// </summary>
		/// <param name="slot">Slot</param>
#endif
		public FlexibleExecuteMethodFlags(InputSlotAny slot) : base(slot)
		{
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// FlexibleExecuteMethodFlagsをExecuteMethodFlagsにキャスト。
		/// </summary>
		/// <param name="flexible">FlexibleExecuteMethodFlags</param>
		/// <returns>ExecuteMethodFlagsにキャストした結果を返す。</returns>
#else
		/// <summary>
		/// Cast FlexibleExecuteMethodFlags to ExecuteMethodFlags.
		/// </summary>
		/// <param name="flexible">FlexibleExecuteMethodFlags</param>
		/// <returns>Returns the result of casting to ExecuteMethodFlags.</returns>
#endif
		public static explicit operator ExecuteMethodFlags(FlexibleExecuteMethodFlags flexible)
		{
			return flexible.value;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// ExecuteMethodFlagsをFlexibleExecuteMethodFlagsにキャスト。
		/// </summary>
		/// <param name="value">ExecuteMethodFlags</param>
		/// <returns>FlexibleExecuteMethodFlagsにキャストした結果を返す。</returns>
#else
		/// <summary>
		/// Cast ExecuteMethodFlags to FlexibleExecuteMethodFlags.
		/// </summary>
		/// <param name="value">ExecuteMethodFlags</param>
		/// <returns>Returns the result of casting to FlexibleExecuteMethodFlags.</returns>
#endif
		public static explicit operator FlexibleExecuteMethodFlags(ExecuteMethodFlags value)
		{
			return new FlexibleExecuteMethodFlags(value);
		}
	}
}
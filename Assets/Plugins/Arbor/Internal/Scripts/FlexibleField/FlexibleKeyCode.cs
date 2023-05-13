//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor
{
#if ARBOR_DOC_JA
	/// <summary>
	/// 参照方法が複数ある柔軟なKeyCode型を扱うクラス。
	/// </summary>
#else
	/// <summary>
	/// Class to handle a flexible KeyCode type reference method there is more than one.
	/// </summary>
#endif
	[System.Serializable]
	public sealed class FlexibleKeyCode : FlexibleField<KeyCode>
	{
#if ARBOR_DOC_JA
		/// <summary>
		/// FlexibleKeyCodeデフォルトコンストラクタ
		/// </summary>
#else
		/// <summary>
		/// FlexibleKeyCode default constructor
		/// </summary>
#endif
		public FlexibleKeyCode()
		{
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// FlexibleKeyCodeコンストラクタ
		/// </summary>
		/// <param name="value">値</param>
#else
		/// <summary>
		/// FlexibleKeyCode constructor
		/// </summary>
		/// <param name="value">Value</param>
#endif
		public FlexibleKeyCode(KeyCode value) : base(value)
		{
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// FlexibleKeyCodeコンストラクタ
		/// </summary>
		/// <param name="parameter">パラメータ</param>
#else
		/// <summary>
		/// FlexibleKeyCode constructor
		/// </summary>
		/// <param name="parameter">Parameter</param>
#endif
		public FlexibleKeyCode(AnyParameterReference parameter) : base(parameter)
		{
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// FlexibleKeyCodeコンストラクタ
		/// </summary>
		/// <param name="slot">スロット</param>
#else
		/// <summary>
		/// FlexibleKeyCode constructor
		/// </summary>
		/// <param name="slot">Slot</param>
#endif
		public FlexibleKeyCode(InputSlotAny slot) : base(slot)
		{
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// FlexibleKeyCodeをKeyCodeにキャスト。
		/// </summary>
		/// <param name="flexible">FlexibleKeyCode</param>
		/// <returns>KeyCodeにキャストした結果を返す。</returns>
#else
		/// <summary>
		/// Cast FlexibleKeyCode to KeyCode.
		/// </summary>
		/// <param name="flexible">FlexibleKeyCode</param>
		/// <returns>Returns the result of casting to KeyCode.</returns>
#endif
		public static explicit operator KeyCode(FlexibleKeyCode flexible)
		{
			return flexible.value;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// KeyCodeをFlexibleKeyCodeにキャスト。
		/// </summary>
		/// <param name="value">KeyCode</param>
		/// <returns>FlexibleKeyCodeにキャストした結果を返す。</returns>
#else
		/// <summary>
		/// Cast KeyCode to FlexibleKeyCode.
		/// </summary>
		/// <param name="value">KeyCode</param>
		/// <returns>Returns the result of casting to FlexibleKeyCode.</returns>
#endif
		public static explicit operator FlexibleKeyCode(KeyCode value)
		{
			return new FlexibleKeyCode(value);
		}
	}
}
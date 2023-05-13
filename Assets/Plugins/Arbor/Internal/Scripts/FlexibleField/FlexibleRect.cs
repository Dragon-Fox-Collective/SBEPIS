//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor
{
#if ARBOR_DOC_JA
	/// <summary>
	/// 参照方法が複数ある柔軟なRect型を扱うクラス。
	/// </summary>
#else
	/// <summary>
	/// Class to handle a flexible Rect type reference method there is more than one.
	/// </summary>
#endif
	[System.Serializable]
	public sealed class FlexibleRect : FlexibleField<Rect>
	{
#if ARBOR_DOC_JA
		/// <summary>
		/// FlexibleRectデフォルトコンストラクタ
		/// </summary>
#else
		/// <summary>
		/// FlexibleRect default constructor
		/// </summary>
#endif
		public FlexibleRect()
		{
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// FlexibleRectコンストラクタ
		/// </summary>
		/// <param name="value">値</param>
#else
		/// <summary>
		/// FlexibleRect constructor
		/// </summary>
		/// <param name="value">Value</param>
#endif
		public FlexibleRect(Rect value) : base(value)
		{
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// FlexibleRectコンストラクタ
		/// </summary>
		/// <param name="parameter">パラメータ</param>
#else
		/// <summary>
		/// FlexibleRect constructor
		/// </summary>
		/// <param name="parameter">Parameter</param>
#endif
		public FlexibleRect(RectParameterReference parameter) : base(new AnyParameterReference(parameter))
		{
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// FlexibleRectコンストラクタ
		/// </summary>
		/// <param name="slot">スロット</param>
#else
		/// <summary>
		/// FlexibleRect constructor
		/// </summary>
		/// <param name="slot">Slot</param>
#endif
		public FlexibleRect(InputSlotRect slot) : base(new InputSlotAny(slot))
		{
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// FlexibleRectをRectにキャスト。
		/// </summary>
		/// <param name="flexible">FlexibleRect</param>
		/// <returns>Rectにキャストした結果を返す。</returns>
#else
		/// <summary>
		/// Cast FlexibleRect to Rect.
		/// </summary>
		/// <param name="flexible">FlexibleRect</param>
		/// <returns>Returns the result of casting to Rect.</returns>
#endif
		public static explicit operator Rect(FlexibleRect flexible)
		{
			return flexible.value;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// RectをFlexibleRectにキャスト。
		/// </summary>
		/// <param name="value">Rect</param>
		/// <returns>FlexibleRectにキャストした結果を返す。</returns>
#else
		/// <summary>
		/// Cast Rect to FlexibleRect.
		/// </summary>
		/// <param name="value">Rect</param>
		/// <returns>Returns the result of casting to FlexibleRect.</returns>
#endif
		public static explicit operator FlexibleRect(Rect value)
		{
			return new FlexibleRect(value);
		}
	}
}

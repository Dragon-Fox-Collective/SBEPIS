//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor
{
#if ARBOR_DOC_JA
	/// <summary>
	/// 参照方法が複数ある柔軟なRectInt型を扱うクラス。
	/// </summary>
#else
	/// <summary>
	/// Class to handle a flexible RectInt type reference method there is more than one.
	/// </summary>
#endif
	[System.Serializable]
	public sealed class FlexibleRectInt : FlexibleField<RectInt>
	{
#if ARBOR_DOC_JA
		/// <summary>
		/// FlexibleRectIntデフォルトコンストラクタ
		/// </summary>
#else
		/// <summary>
		/// FlexibleRectInt default constructor
		/// </summary>
#endif
		public FlexibleRectInt()
		{
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// FlexibleRectIntコンストラクタ
		/// </summary>
		/// <param name="value">値</param>
#else
		/// <summary>
		/// FlexibleRectInt constructor
		/// </summary>
		/// <param name="value">Value</param>
#endif
		public FlexibleRectInt(RectInt value) : base(value)
		{
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// FlexibleRectIntコンストラクタ
		/// </summary>
		/// <param name="parameter">パラメータ</param>
#else
		/// <summary>
		/// FlexibleVector2 constructor
		/// </summary>
		/// <param name="parameter">Parameter</param>
#endif
		public FlexibleRectInt(RectIntParameterReference parameter) : base(new AnyParameterReference(parameter))
		{
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// FlexibleRectIntコンストラクタ
		/// </summary>
		/// <param name="slot">スロット</param>
#else
		/// <summary>
		/// FlexibleRectInt constructor
		/// </summary>
		/// <param name="slot">Slot</param>
#endif
		public FlexibleRectInt(InputSlotRectInt slot) : base(new InputSlotAny(slot))
		{
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// FlexibleRectIntをRectIntにキャスト。
		/// </summary>
		/// <param name="flexible">FlexibleRectInt</param>
		/// <returns>RectIntにキャストした結果を返す。</returns>
#else
		/// <summary>
		/// Cast FlexibleRectInt to RectInt.
		/// </summary>
		/// <param name="flexible">FlexibleRectInt</param>
		/// <returns>Returns the result of casting to RectInt.</returns>
#endif
		public static explicit operator RectInt(FlexibleRectInt flexible)
		{
			return flexible.value;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// RectIntをFlexibleRectIntにキャスト。
		/// </summary>
		/// <param name="value">RectInt</param>
		/// <returns>FlexibleRectIntにキャストした結果を返す。</returns>
#else
		/// <summary>
		/// Cast RectInt to FlexibleRectInt.
		/// </summary>
		/// <param name="value">RectInt</param>
		/// <returns>Returns the result of casting to FlexibleRectInt.</returns>
#endif
		public static explicit operator FlexibleRectInt(RectInt value)
		{
			return new FlexibleRectInt(value);
		}
	}
}
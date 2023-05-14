//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor
{
#if ARBOR_DOC_JA
	/// <summary>
	/// 参照方法が複数ある柔軟なColor型を扱うクラス。
	/// </summary>
#else
	/// <summary>
	/// Class to handle a flexible Color type reference method there is more than one.
	/// </summary>
#endif
	[System.Serializable]
	public sealed class FlexibleColor : FlexibleField<Color>
	{
#if ARBOR_DOC_JA
		/// <summary>
		/// FlexibleColorデフォルトコンストラクタ
		/// </summary>
#else
		/// <summary>
		/// FlexibleColor default constructor
		/// </summary>
#endif
		public FlexibleColor() : base()
		{
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// FlexibleColorコンストラクタ
		/// </summary>
		/// <param name="value">値</param>
#else
		/// <summary>
		/// FlexibleColor constructor
		/// </summary>
		/// <param name="value">Value</param>
#endif
		public FlexibleColor(Color value) : base(value)
		{
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// FlexibleColorコンストラクタ
		/// </summary>
		/// <param name="parameter">パラメータ</param>
#else
		/// <summary>
		/// FlexibleColor constructor
		/// </summary>
		/// <param name="parameter">Parameter</param>
#endif
		public FlexibleColor(ColorParameterReference parameter) : base(new AnyParameterReference(parameter))
		{
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// FlexibleColorコンストラクタ
		/// </summary>
		/// <param name="slot">スロット</param>
#else
		/// <summary>
		/// FlexibleColor constructor
		/// </summary>
		/// <param name="slot">Slot</param>
#endif
		public FlexibleColor(InputSlotColor slot) : base(new InputSlotAny(slot))
		{
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// FlexibleColorをColorにキャスト。
		/// </summary>
		/// <param name="flexible">FlexibleColor</param>
		/// <returns>Colorにキャストした結果を返す。</returns>
#else
		/// <summary>
		/// Cast FlexibleColor to Color.
		/// </summary>
		/// <param name="flexible">FlexibleColor</param>
		/// <returns>Returns the result of casting to Color.</returns>
#endif
		public static explicit operator Color(FlexibleColor flexible)
		{
			return flexible.value;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// ColorをFlexibleColorにキャスト。
		/// </summary>
		/// <param name="value">Color</param>
		/// <returns>FlexibleColorにキャストした結果を返す。</returns>
#else
		/// <summary>
		/// Cast Color to FlexibleColor.
		/// </summary>
		/// <param name="value">Color</param>
		/// <returns>Returns the result of casting to FlexibleColor.</returns>
#endif
		public static explicit operator FlexibleColor(Color value)
		{
			return new FlexibleColor(value);
		}
	}
}

//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
#if ARBOR_SUPPORT_UGUI
using UnityEngine.EventSystems;

namespace Arbor
{
#if ARBOR_DOC_JA
	/// <summary>
	/// 参照方法が複数ある柔軟なInputButton型を扱うクラス。
	/// </summary>
#else
	/// <summary>
	/// Class to handle a flexible InputButton type reference method there is more than one.
	/// </summary>
#endif
	[System.Serializable]
	public sealed class FlexibleInputButton : FlexibleField<PointerEventData.InputButton>
	{
#if ARBOR_DOC_JA
		/// <summary>
		/// FlexibleInputButtonデフォルトコンストラクタ
		/// </summary>
#else
		/// <summary>
		/// FlexibleInputButton default constructor
		/// </summary>
#endif
		public FlexibleInputButton()
		{
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// FlexibleInputButtonコンストラクタ
		/// </summary>
		/// <param name="value">値</param>
#else
		/// <summary>
		/// FlexibleInputButton constructor
		/// </summary>
		/// <param name="value">Value</param>
#endif
		public FlexibleInputButton(PointerEventData.InputButton value) : base(value)
		{
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// FlexibleInputButtonコンストラクタ
		/// </summary>
		/// <param name="parameter">パラメータ</param>
#else
		/// <summary>
		/// FlexibleInputButton constructor
		/// </summary>
		/// <param name="parameter">Parameter</param>
#endif
		public FlexibleInputButton(AnyParameterReference parameter) : base(parameter)
		{
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// FlexibleInputButtonコンストラクタ
		/// </summary>
		/// <param name="slot">スロット</param>
#else
		/// <summary>
		/// FlexibleInputButton constructor
		/// </summary>
		/// <param name="slot">Slot</param>
#endif
		public FlexibleInputButton(InputSlotAny slot) : base(slot)
		{
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// FlexibleInputButtonをInputButtonにキャスト。
		/// </summary>
		/// <param name="flexible">FlexibleInputButton</param>
#else
		/// <summary>
		/// Cast FlexibleInputButton to InputButton.
		/// </summary>
		/// <param name="flexible">FlexibleInputButton</param>
#endif
		public static explicit operator PointerEventData.InputButton(FlexibleInputButton flexible)
		{
			return flexible.value;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// InputButtonをFlexibleInputButtonにキャスト。
		/// </summary>
		/// <param name="value">InputButton</param>
#else
		/// <summary>
		/// Cast InputButton to FlexibleInputButton.
		/// </summary>
		/// <param name="value">InputButton</param>
#endif
		public static explicit operator FlexibleInputButton(PointerEventData.InputButton value)
		{
			return new FlexibleInputButton(value);
		}
	}
}
#endif

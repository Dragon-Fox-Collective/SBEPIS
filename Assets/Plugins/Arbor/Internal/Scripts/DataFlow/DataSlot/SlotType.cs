//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------

namespace Arbor
{
#if ARBOR_DOC_JA
	/// <summary>
	/// DataSlotの種類
	/// </summary>
#else
	/// <summary>
	/// Types of DataSlot
	/// </summary>
#endif
	public enum SlotType
	{
#if ARBOR_DOC_JA
		/// <summary>
		/// 入力スロット
		/// </summary>
#else
		/// <summary>
		/// Input slot
		/// </summary>
#endif
		Input,

#if ARBOR_DOC_JA
		/// <summary>
		/// 出力スロット
		/// </summary>
#else
		/// <summary>
		/// Output slot
		/// </summary>
#endif
		Output,

#if ARBOR_DOC_JA
		/// <summary>
		/// リルートスロット
		/// </summary>
#else
		/// <summary>
		/// Reroute slot
		/// </summary>
#endif
		Reroute,
	}
}

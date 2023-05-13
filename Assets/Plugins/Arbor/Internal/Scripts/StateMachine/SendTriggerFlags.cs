//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------

namespace Arbor
{
#if ARBOR_DOC_JA
	/// <summary>
	/// SendTriggerで送信するトリガーのフラグ。
	/// </summary>
#else
	/// <summary>
	/// Trigger flag to send with SendTrigger.
	/// </summary>
#endif
	[System.Flags]
	[Internal.Documentable]
	public enum SendTriggerFlags
	{
#if ARBOR_DOC_JA
		/// <summary>
		/// 現在ステートへ送る。
		/// </summary>
#else
		/// <summary>
		/// Send to current state.
		/// </summary>
#endif
		CurrentState = 0x01,

#if ARBOR_DOC_JA
		/// <summary>
		/// 常駐ステートへ送る。
		/// </summary>
#else
		/// <summary>
		/// Send to resident states.
		/// </summary>
#endif
		ResidentStates = 0x02,
	}
}
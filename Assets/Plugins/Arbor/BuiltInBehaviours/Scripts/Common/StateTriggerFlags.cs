//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------

namespace Arbor
{
#if ARBOR_DOC_JA
	/// <summary>
	/// ステートのイベントトリガーのフラグ
	/// </summary>
#else
	/// <summary>
	/// State event trigger flags
	/// </summary>
#endif
	[System.Flags]
	[Internal.Documentable]
	public enum StateTriggerFlags
	{
#if ARBOR_DOC_JA
		/// <summary>
		/// OnStateAwakeの時にイベントを呼ぶ
		/// </summary>
#else
		/// <summary>
		/// Call an event at OnStateAwake
		/// </summary>
#endif
		OnStateAwake = 0x01,

#if ARBOR_DOC_JA
		/// <summary>
		/// OnStateBeginの時にイベントを呼ぶ
		/// </summary>
#else
		/// <summary>
		/// Call an event at OnStateBegin
		/// </summary>
#endif
		OnStateBegin = 0x02,

#if ARBOR_DOC_JA
		/// <summary>
		/// OnStateUpdateの時にイベントを呼ぶ
		/// </summary>
#else
		/// <summary>
		/// Call an event at OnStateUpdate
		/// </summary>
#endif
		OnStateUpdate = 0x04,

#if ARBOR_DOC_JA
		/// <summary>
		/// OnStateLateUpdateの時にイベントを呼ぶ
		/// </summary>
#else
		/// <summary>
		/// Call an event at OnStateLateUpdate
		/// </summary>
#endif
		OnStateLateUpdate = 0x08,

#if ARBOR_DOC_JA
		/// <summary>
		/// OnStateEndの時にイベントを呼ぶ
		/// </summary>
#else
		/// <summary>
		/// Call an event at OnStateLateUpdate
		/// </summary>
#endif
		OnStateEnd = 0x10,
	}
}
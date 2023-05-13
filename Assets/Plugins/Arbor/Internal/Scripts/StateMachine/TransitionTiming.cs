//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------

namespace Arbor
{
#if ARBOR_DOC_JA
	/// <summary>
	/// 遷移するタイミング。
	/// </summary>
#else
	/// <summary>
	/// Transition timing.
	/// </summary>
#endif
	[Internal.Documentable]
	public enum TransitionTiming
	{
#if ARBOR_DOC_JA
		/// <summary>
		/// 遷移をLateUpdate時に行うように予約する。すでに予約済みの場合は上書きする。
		/// </summary>
#else
		/// <summary>
		/// Reserve to make transition at LateUpdate. Overwrite if already reserved.
		/// </summary>
#endif
		LateUpdateOverwrite = 0,

#if ARBOR_DOC_JA
		/// <summary>
		/// 即時に遷移する。遷移メソッドの中で処理が完遂する。
		/// </summary>
#else
		/// <summary>
		/// Transit immediately. Processing is completed in the transition method.
		/// </summary>
#endif
		Immediate = 1,

#if ARBOR_DOC_JA
		/// <summary>
		/// 遷移をLateUpdate時に行うように予約する。すでに予約済みの場合は上書きしない。
		/// </summary>
#else
		/// <summary>
		/// Reserve to make transition at LateUpdate. If it is already reserved, do not overwrite it.
		/// </summary>
#endif
		LateUpdateDontOverwrite = 2,

#if ARBOR_DOC_JA
		/// <summary>
		/// 遷移を次のUpdate時に行うように予約する。すでに予約済みの場合は上書きする。
		/// </summary>
#else
		/// <summary>
		/// Reserve to make transition at next Update. Overwrite if already reserved.
		/// </summary>
#endif
		NextUpdateOverwrite = 3,

#if ARBOR_DOC_JA
		/// <summary>
		/// 遷移を次のUpdate時に行うように予約する。すでに予約済みの場合は上書きしない。
		/// </summary>
#else
		/// <summary>
		/// Reserve to make transition at next Update. If it is already reserved, do not overwrite it.
		/// </summary>
#endif
		NextUpdateDontOverwrite = 4,
	}
}
﻿//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------

namespace Arbor
{
#if ARBOR_DOC_JA
	/// <summary>
	/// 時間タイプ
	/// </summary>
#else
	/// <summary>
	/// Time type
	/// </summary>
#endif
	[Internal.Documentable]
	public enum TimeType
	{
#if ARBOR_DOC_JA
		/// <summary>
		/// timeScaleを考慮した現在フレームの時間を使用。
		/// </summary>
#else
		/// <summary>
		/// Use time of current frame considering timeScale.
		/// </summary>
#endif
		Normal,

#if ARBOR_DOC_JA
		/// <summary>
		///timeScaleを考慮しない現在フレームの時間を使用。
		/// </summary>
#else
		/// <summary>
		/// Use time of current frame without considering timeScale.
		/// </summary>
#endif
		Unscaled,

#if ARBOR_DOC_JA
		/// <summary>
		/// リアルタイムを使用。
		/// </summary>
#else
		/// <summary>
		/// Use realtime.
		/// </summary>
#endif
		Realtime,

#if ARBOR_DOC_JA
		/// <summary>
		/// timeScaleを考慮した最新のFixedUpdate開始時間を使用。
		/// </summary>
#else
		/// <summary>
		/// Use the latest FixedUpdate start time considering the timeScale.
		/// </summary>
#endif
		FixedTime,

#if ARBOR_DOC_JA
		/// <summary>
		///timeScaleを考慮しない最新のFixedUpdate開始時間を使用。
		/// </summary>
#else
		/// <summary>
		/// Use the latest FixedUpdate start time without considering timeScale.
		/// </summary>
#endif
		FixedUnscaledTime,
	}
}
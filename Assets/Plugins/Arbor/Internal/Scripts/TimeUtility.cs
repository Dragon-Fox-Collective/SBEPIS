//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor
{
#if ARBOR_DOC_JA
	/// <summary>
	/// Timeのユーティリティクラス
	/// </summary>
#else
	/// <summary>
	/// Time utility class
	/// </summary>
#endif
	public static class TimeUtility
	{
#if ARBOR_DOC_JA
		/// <summary>
		/// 現在時間を返す。
		/// </summary>
		/// <param name="type">時間タイプ</param>
		/// <returns>現在時間</returns>
#else
		/// <summary>
		/// Return current time.
		/// </summary>
		/// <param name="type">Time type</param>
		/// <returns>Current time</returns>
#endif
		public static float CurrentTime(TimeType type)
		{
			switch (type)
			{
				case TimeType.Normal:
					return Time.time;
				case TimeType.Unscaled:
					return Time.unscaledTime;
				case TimeType.Realtime:
					return Time.realtimeSinceStartup;
				case TimeType.FixedTime:
					return Time.fixedTime;
				case TimeType.FixedUnscaledTime:
					return Time.fixedUnscaledTime;
			}
			return Time.time;
		}
	}
}

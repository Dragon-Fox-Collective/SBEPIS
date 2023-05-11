//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor
{
	public static class WaypointTimeUtility
	{
		public static TimeType ToTimeType(WaypointTimeType timeType)
		{
			switch (timeType)
			{
				case WaypointTimeType.Normal:
					return TimeType.Normal;
				case WaypointTimeType.Unscaled:
					return TimeType.Unscaled;
				case WaypointTimeType.Realtime:
					return TimeType.Realtime;
				case WaypointTimeType.FixedTime:
					return TimeType.FixedTime;
				case WaypointTimeType.FixedUnscaledTime:
					return TimeType.FixedUnscaledTime;
			}

			return TimeType.Normal;
		}

	}
}
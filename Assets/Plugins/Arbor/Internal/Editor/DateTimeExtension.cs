//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------

namespace ArborEditor
{
	internal static class DateTimeExtension
	{
		private static readonly System.DateTime UNIX_EPOCH;

		static DateTimeExtension()
		{
			UNIX_EPOCH = new System.DateTime(1970, 1, 1, 0, 0, 0, 0);
		}

		public static long ToUnixTime(this System.DateTime targetTime)
		{
			return (long)(targetTime.ToUniversalTime() - UNIX_EPOCH).TotalSeconds;
		}
	}
}
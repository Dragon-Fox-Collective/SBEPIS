namespace WrightWay.SBEPIS.Util
{
	public static class ExtensionMethods
	{
		public static float Map(this float value, float inputFrom, float inputTo, float outputFrom, float outputTo)
		{
			return (value - inputFrom) / (inputTo - inputFrom) * (outputTo - outputFrom) + outputFrom;
		}
	}
}

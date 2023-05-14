//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using System;
using System.Collections.Generic;

namespace Arbor
{
	public static class CompareUtility
	{
		public static bool IsCompare(int compare, CompareType compareType)
		{
			switch (compareType)
			{
				case CompareType.Equals:
					return compare == 0;
				case CompareType.NotEquals:
					return compare != 0;
				case CompareType.Greater:
					return compare > 0;
				case CompareType.GreaterOrEquals:
					return compare >= 0;
				case CompareType.Less:
					return compare < 0;
				case CompareType.LessOrEquals:
					return compare <= 0;
			}

			return false;
		}

		public static bool Compare<T>(T value1, T value2, CompareType compareType) where T : IComparable<T>
		{
			int compare = value1.CompareTo(value2);
			return IsCompare(compare, compareType);
		}

		public static bool Compare<T>(T value1, T value2, CompareType compareType, IComparer<T> comparer)
		{
			int compare = comparer.Compare(value1, value2);
			return IsCompare(compare, compareType);
		}

		public static bool Compare(string value1, string value2, CompareType compareType, StringComparison comparison)
		{
			int compare = string.Compare(value1, value2, comparison);
			return IsCompare(compare, compareType);
		}
	}
}
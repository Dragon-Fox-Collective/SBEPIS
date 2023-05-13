using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arbor
{
	internal static class FlexiblePrimitiveUtility
	{
		private static List<IFlexibleField> s_FindFlexibleFields = null;

		public static bool HasRandomFlexiblePrimitive(object obj)
		{
			if (s_FindFlexibleFields == null)
			{
				s_FindFlexibleFields = new List<IFlexibleField>();
			}
			else
			{
				s_FindFlexibleFields.Clear();
			}

			EachField<IFlexibleField>.Find(obj, obj.GetType(), s_FindFlexibleFields);

			for (int i = 0; i < s_FindFlexibleFields.Count; i++)
			{
				var f = s_FindFlexibleFields[i];
				FlexiblePrimitiveBase flexiblePrimitive = f as FlexiblePrimitiveBase;
				if (flexiblePrimitive != null && flexiblePrimitive.type == FlexiblePrimitiveType.Random)
				{
					return true;
				}
			}

			return false;
		}
	}
}
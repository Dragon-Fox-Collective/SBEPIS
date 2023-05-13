//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace ArborEditor
{
	public static class RectUtility
	{
		public static Rect ScaleSizeBy(Rect rect, float scale)
		{
			return ScaleSizeBy(rect, scale, rect.center);
		}

		public static Rect ScaleSizeBy(Rect rect, float scale, Vector2 pivotPoint)
		{
			Rect result = rect;

			result.x -= pivotPoint.x;
			result.y -= pivotPoint.y;

			result.xMin *= scale;
			result.yMin *= scale;
			result.xMax *= scale;
			result.yMax *= scale;

			result.x += pivotPoint.x;
			result.y += pivotPoint.y;

			return result;
		}

		public static Rect FromToRect(Vector2 start, Vector2 end)
		{
			Rect rect = new Rect(start.x, start.y, end.x - start.x, end.y - start.y);
			if (rect.width < 0.0f)
			{
				rect.x += rect.width;
				rect.width = -rect.width;
			}
			if (rect.height < 0.0f)
			{
				rect.y += rect.height;
				rect.height = -rect.height;
			}
			return rect;
		}

		public static bool IsNaN(Rect rect)
		{
			return float.IsNaN(rect.x) || float.IsNaN(rect.y) || float.IsNaN(rect.width) || float.IsNaN(rect.height);
		}
	}

}

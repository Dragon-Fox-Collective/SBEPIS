//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using System.Collections;

namespace Arbor
{
	internal static class ParameterAccessor
	{
		private static readonly ParameterAccessorInt s_Int = new ParameterAccessorInt();
		private static readonly ParameterAccessorLong s_Long = new ParameterAccessorLong();
		private static readonly ParameterAccessorFloat s_Float = new ParameterAccessorFloat();
		private static readonly ParameterAccessorBool s_Bool = new ParameterAccessorBool();
		private static readonly ParameterAccessorEnum s_Enum = new ParameterAccessorEnum();
		private static readonly ParameterAccessorVector2 s_Vector2 = new ParameterAccessorVector2();
		private static readonly ParameterAccessorVector3 s_Vector3 = new ParameterAccessorVector3();
		private static readonly ParameterAccessorVector4 s_Vector4 = new ParameterAccessorVector4();
		private static readonly ParameterAccessorQuaternion s_Quaternion = new ParameterAccessorQuaternion();
		private static readonly ParameterAccessorRect s_Rect = new ParameterAccessorRect();
		private static readonly ParameterAccessorBounds s_Bounds = new ParameterAccessorBounds();
		private static readonly ParameterAccessorColor s_Color = new ParameterAccessorColor();
		private static readonly ParameterAccessorVector2Int s_Vector2Int = new ParameterAccessorVector2Int();
		private static readonly ParameterAccessorVector3Int s_Vector3Int = new ParameterAccessorVector3Int();
		private static readonly ParameterAccessorRectInt s_RectInt = new ParameterAccessorRectInt();
		private static readonly ParameterAccessorBoundsInt s_BoundsInt = new ParameterAccessorBoundsInt();

		public static IParameterAccessor GetAccessor(Parameter.Type type)
		{
			switch (type)
			{
				case Parameter.Type.Int:
					return s_Int;
				case Parameter.Type.Long:
					return s_Long;
				case Parameter.Type.Float:
					return s_Float;
				case Parameter.Type.Bool:
					return s_Bool;
				case Parameter.Type.Enum:
					return s_Enum;
				case Parameter.Type.Vector2:
					return s_Vector2;
				case Parameter.Type.Vector3:
					return s_Vector3;
				case Parameter.Type.Vector4:
					return s_Vector4;
				case Parameter.Type.Quaternion:
					return s_Quaternion;
				case Parameter.Type.Rect:
					return s_Rect;
				case Parameter.Type.Bounds:
					return s_Bounds;
				case Parameter.Type.Color:
					return s_Color;
				case Parameter.Type.Vector2Int:
					return s_Vector2Int;
				case Parameter.Type.Vector3Int:
					return s_Vector3Int;
				case Parameter.Type.RectInt:
					return s_RectInt;
				case Parameter.Type.BoundsInt:
					return s_BoundsInt;
			}

			return null;
		}
	}
}
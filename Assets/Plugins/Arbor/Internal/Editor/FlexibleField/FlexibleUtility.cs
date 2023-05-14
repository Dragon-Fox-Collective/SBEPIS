//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEditor;

using Arbor;

namespace ArborEditor
{
	public static class FlexibleUtility
	{
		public static FlexiblePropertyType GetPropertyType(Parameter.Type parameterType)
		{
			switch (parameterType)
			{
				case Parameter.Type.Int:
				case Parameter.Type.Long:
				case Parameter.Type.Float:
				case Parameter.Type.Bool:
					return FlexiblePropertyType.Primitive;
				case Parameter.Type.String:
				case Parameter.Type.Enum:
				case Parameter.Type.Vector2:
				case Parameter.Type.Vector3:
				case Parameter.Type.Quaternion:
				case Parameter.Type.Rect:
				case Parameter.Type.Bounds:
				case Parameter.Type.Color:
				case Parameter.Type.Vector4:
				case Parameter.Type.Vector2Int:
				case Parameter.Type.Vector3Int:
				case Parameter.Type.RectInt:
				case Parameter.Type.BoundsInt:
				case Parameter.Type.AssetObject:
					return FlexiblePropertyType.Field;
				case Parameter.Type.GameObject:
				case Parameter.Type.Transform:
				case Parameter.Type.RectTransform:
				case Parameter.Type.Rigidbody:
				case Parameter.Type.Rigidbody2D:
				case Parameter.Type.Component:
					return FlexiblePropertyType.SceneObject;
			}

			return FlexiblePropertyType.Unknown;
		}
	}
}
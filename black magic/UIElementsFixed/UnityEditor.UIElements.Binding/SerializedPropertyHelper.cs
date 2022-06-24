using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEditor.UIElements.Bindings;

internal static class SerializedPropertyHelper
{
	public static bool IsPropertyValid(SerializedProperty prop)
	{
		return prop?.isValid ?? false;
	}

	public static bool IsPropertyValidFaster(HashSet<int> knownPropertyPaths, SerializedProperty p)
	{
		if (p == null)
		{
			return false;
		}
		if (knownPropertyPaths.Contains(p.hashCodeForPropertyPath))
		{
			return true;
		}
		if (IsPropertyValid(p))
		{
			knownPropertyPaths.Add(p.hashCodeForPropertyPath);
			return true;
		}
		return false;
	}

	public static int GetIntPropertyValue(SerializedProperty p)
	{
		return p.intValue;
	}

	public static object GetManagedReferenceValue(SerializedProperty p)
	{
		return p.managedReferenceValue;
	}

	public static long GetLongPropertyValue(SerializedProperty p)
	{
		return p.longValue;
	}

	public static bool GetBoolPropertyValue(SerializedProperty p)
	{
		return p.boolValue;
	}

	public static float GetFloatPropertyValue(SerializedProperty p)
	{
		return p.floatValue;
	}

	public static double GetDoublePropertyValue(SerializedProperty p)
	{
		return p.doubleValue;
	}

	public static string GetStringPropertyValue(SerializedProperty p)
	{
		return p.stringValue;
	}

	public static Color GetColorPropertyValue(SerializedProperty p)
	{
		return p.colorValue;
	}

	public static UnityEngine.Object GetObjectRefPropertyValue(SerializedProperty p)
	{
		return p.objectReferenceValue;
	}

	public static int GetLayerMaskPropertyValue(SerializedProperty p)
	{
		return p.intValue;
	}

	public static Vector2 GetVector2PropertyValue(SerializedProperty p)
	{
		return p.vector2Value;
	}

	public static Vector3 GetVector3PropertyValue(SerializedProperty p)
	{
		return p.vector3Value;
	}

	public static Vector4 GetVector4PropertyValue(SerializedProperty p)
	{
		return p.vector4Value;
	}

	public static Vector2Int GetVector2IntPropertyValue(SerializedProperty p)
	{
		return p.vector2IntValue;
	}

	public static Vector3Int GetVector3IntPropertyValue(SerializedProperty p)
	{
		return p.vector3IntValue;
	}

	public static Rect GetRectPropertyValue(SerializedProperty p)
	{
		return p.rectValue;
	}

	public static RectInt GetRectIntPropertyValue(SerializedProperty p)
	{
		return p.rectIntValue;
	}

	public static AnimationCurve GetAnimationCurvePropertyValue(SerializedProperty p)
	{
		return p.animationCurveValue;
	}

	public static Bounds GetBoundsPropertyValue(SerializedProperty p)
	{
		return p.boundsValue;
	}

	public static BoundsInt GetBoundsIntPropertyValue(SerializedProperty p)
	{
		return p.boundsIntValue;
	}

	public static Gradient GetGradientPropertyValue(SerializedProperty p)
	{
		return p.gradientValue;
	}

	public static Quaternion GetQuaternionPropertyValue(SerializedProperty p)
	{
		return p.quaternionValue;
	}

	public static char GetCharacterPropertyValue(SerializedProperty p)
	{
		return (char)p.intValue;
	}

	public static float GetDoublePropertyValueAsFloat(SerializedProperty p)
	{
		return (float)p.doubleValue;
	}

	public static double GetFloatPropertyValueAsDouble(SerializedProperty p)
	{
		return p.floatValue;
	}

	public static string GetCharacterPropertyValueAsString(SerializedProperty p)
	{
		return new string((char)p.intValue, 1);
	}

	public static float GetIntPropertyValueAsFloat(SerializedProperty p)
	{
		return p.intValue;
	}

	public static float GetLongPropertyValueAsFloat(SerializedProperty p)
	{
		return p.longValue;
	}

	public static string GetEnumPropertyValueAsString(SerializedProperty p)
	{
		return p.enumDisplayNames[p.enumValueIndex];
	}

	public static void SetIntPropertyValue(SerializedProperty p, int v)
	{
		p.intValue = v;
	}

	public static void SetLongPropertyValue(SerializedProperty p, long v)
	{
		p.longValue = v;
	}

	public static void SetBoolPropertyValue(SerializedProperty p, bool v)
	{
		p.boolValue = v;
	}

	public static void SetFloatPropertyValue(SerializedProperty p, float v)
	{
		p.floatValue = v;
	}

	public static void SetDoublePropertyValue(SerializedProperty p, double v)
	{
		p.doubleValue = v;
	}

	public static void SetStringPropertyValue(SerializedProperty p, string v)
	{
		p.stringValue = v;
	}

	public static void SetColorPropertyValue(SerializedProperty p, Color v)
	{
		p.colorValue = v;
	}

	public static void SetObjectRefPropertyValue(SerializedProperty p, UnityEngine.Object v)
	{
		p.objectReferenceValue = v;
	}

	public static void SetLayerMaskPropertyValue(SerializedProperty p, int v)
	{
		p.intValue = v;
	}

	public static void SetVector2PropertyValue(SerializedProperty p, Vector2 v)
	{
		p.vector2Value = v;
	}

	public static void SetVector3PropertyValue(SerializedProperty p, Vector3 v)
	{
		p.vector3Value = v;
	}

	public static void SetVector4PropertyValue(SerializedProperty p, Vector4 v)
	{
		p.vector4Value = v;
	}

	public static void SetVector2IntPropertyValue(SerializedProperty p, Vector2Int v)
	{
		p.vector2IntValue = v;
	}

	public static void SetVector3IntPropertyValue(SerializedProperty p, Vector3Int v)
	{
		p.vector3IntValue = v;
	}

	public static void SetRectPropertyValue(SerializedProperty p, Rect v)
	{
		p.rectValue = v;
	}

	public static void SetRectIntPropertyValue(SerializedProperty p, RectInt v)
	{
		p.rectIntValue = v;
	}

	public static void SetAnimationCurvePropertyValue(SerializedProperty p, AnimationCurve v)
	{
		p.animationCurveValue = v;
	}

	public static void SetBoundsPropertyValue(SerializedProperty p, Bounds v)
	{
		p.boundsValue = v;
	}

	public static void SetBoundsIntPropertyValue(SerializedProperty p, BoundsInt v)
	{
		p.boundsIntValue = v;
	}

	public static void SetGradientPropertyValue(SerializedProperty p, Gradient v)
	{
		p.gradientValue = v;
	}

	public static void SetQuaternionPropertyValue(SerializedProperty p, Quaternion v)
	{
		p.quaternionValue = v;
	}

	public static void SetCharacterPropertyValue(SerializedProperty p, char v)
	{
		p.intValue = v;
	}

	public static void SetDoublePropertyValueFromFloat(SerializedProperty p, float v)
	{
		p.doubleValue = v;
	}

	public static void SetFloatPropertyValueFromDouble(SerializedProperty p, double v)
	{
		p.floatValue = (float)v;
	}

	public static void SetCharacterPropertyValueFromString(SerializedProperty p, string v)
	{
		if (v.Length > 0)
		{
			p.intValue = v[0];
		}
	}

	public static void SetEnumPropertyValueFromString(SerializedProperty p, string v)
	{
		p.enumValueIndex = FindStringIndex(p.enumDisplayNames, v);
	}

	public static int FindStringIndex(string[] values, string v)
	{
		for (int i = 0; i < values.Length; i++)
		{
			if (values[i] == v)
			{
				return i;
			}
		}
		return -1;
	}

	public static bool ValueEquals<TValue>(TValue value, SerializedProperty p, Func<SerializedProperty, TValue> propertyReadFunc)
	{
		TValue y = propertyReadFunc(p);
		return EqualityComparer<TValue>.Default.Equals(value, y);
	}

	public static bool ValueEquals(string value, SerializedProperty p, Func<SerializedProperty, string> propertyReadFunc)
	{
		if (p.propertyType == SerializedPropertyType.Enum)
		{
			return p.enumDisplayNames[p.enumValueIndex] == value;
		}
		return p.ValueEquals(value);
	}

	public static bool ValueEquals(AnimationCurve value, SerializedProperty p, Func<SerializedProperty, AnimationCurve> propertyReadFunc)
	{
		return p.ValueEquals(value);
	}

	public static bool ValueEquals(Gradient value, SerializedProperty p, Func<SerializedProperty, Gradient> propertyReadFunc)
	{
		return p.ValueEquals(value);
	}

	public static bool SlowEnumValueEquals(string value, SerializedProperty p, Func<SerializedProperty, string> propertyReadFunc)
	{
		string y = propertyReadFunc(p);
		return EqualityComparer<string>.Default.Equals(value, y);
	}

	public static void ForceSync(SerializedProperty p)
	{
		bool unsafeMode = p.unsafeMode;
		p.unsafeMode = false;
		p.Verify();
		p.unsafeMode = unsafeMode;
	}
}

//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using UnityEditor;

namespace ArborEditor
{
	public static class SerializedPropertyExtensions
	{
		public static bool clearBool = false;
		public static Color clearColor = Color.white;

		public static void SetStateData<T>(this SerializedProperty property, T data)
		{
			SerializedStateData<T>.SetData(property, data);
		}

		public static T GetStateData<T>(this SerializedProperty property)
		{
			return SerializedStateData<T>.GetData(property);
		}

		public static bool TryGetStateData<T>(this SerializedProperty property, out T data)
		{
			return SerializedStateData<T>.TryGetData(property, out data);
		}

		public static bool HasStateData<T>(this SerializedProperty property)
		{
			return SerializedStateData<T>.HasData(property);
		}

		public static bool RemoveStateData<T>(this SerializedProperty property)
		{
			return SerializedStateData<T>.Remove(property);
		}

		public static void Clear(this SerializedProperty property, bool includeChildren = false)
		{
			switch (property.propertyType)
			{
				case SerializedPropertyType.Integer:
					property.longValue = 0L;
					break;
				case SerializedPropertyType.Float:
					if (property.type == "float")
					{
						property.floatValue = 0f;
					}
					else
					{
						property.doubleValue = 0.0;
					}
					break;
				case SerializedPropertyType.Boolean:
					property.boolValue = clearBool;
					break;
				case SerializedPropertyType.String:
					property.stringValue = "";
					break;
				case SerializedPropertyType.ObjectReference:
					property.objectReferenceValue = null;
					break;
				case SerializedPropertyType.Vector2:
					property.vector2Value = Vector2.zero;
					break;
				case SerializedPropertyType.Vector3:
					property.vector3Value = Vector3.zero;
					break;
				case SerializedPropertyType.Vector4:
					property.vector4Value = Vector4.zero;
					break;
				case SerializedPropertyType.Quaternion:
					property.quaternionValue = Quaternion.identity;
					break;
				case SerializedPropertyType.Rect:
					property.rectValue = new Rect();
					break;
				case SerializedPropertyType.Bounds:
					property.boundsValue = new Bounds();
					break;
				case SerializedPropertyType.AnimationCurve:
					property.animationCurveValue = new AnimationCurve();
					break;
				case SerializedPropertyType.ArraySize:
					property.ClearArray();
					break;
				case SerializedPropertyType.Character:
					property.intValue = default(char);
					break;
				case SerializedPropertyType.Color:
					property.colorValue = clearColor;
					break;
				case SerializedPropertyType.Enum:
					property.enumValueIndex = 0;
					break;
				case SerializedPropertyType.LayerMask:
					property.intValue = 0;
					break;
				default:
					if (includeChildren)
					{
						property = property.Copy();

						bool enterChildren = property.hasVisibleChildren;
						if (enterChildren)
						{
							SerializedProperty endProperty = property.GetEndProperty();
							while (property.NextVisible(enterChildren) && !SerializedProperty.EqualContents(property, endProperty))
							{
								property.Clear(true);
								enterChildren = false;
							}
						}
					}
					else
					{
#if ARBOR_DEBUG
						Debug.LogFormat("{0}({1}) : Clear not support", property.propertyType, property.type);
#endif
					}
					break;
			}


		}

		public static System.Type GetTypeFromManagedReferenceFullTypeName(this SerializedProperty property)
		{
			string managedReferenceFullTypename = property.managedReferenceFullTypename;

			System.Type managedReferenceInstanceType = null;

			var parts = managedReferenceFullTypename.Split(' ');
			if (parts.Length == 2)
			{
				var assemblyPart = parts[0];
				var nsClassnamePart = parts[1];
				managedReferenceInstanceType = System.Type.GetType($"{nsClassnamePart}, {assemblyPart}");
			}

			return managedReferenceInstanceType;
		}

		public static bool IsInvalidManagedReference(this SerializedProperty property)
		{
			return property.propertyType == SerializedPropertyType.ManagedReference && property.GetTypeFromManagedReferenceFullTypeName() == null;
		}
	}
}
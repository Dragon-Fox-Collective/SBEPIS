//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using UnityEngine;
using UnityEditor;

namespace ArborEditor
{
	using Arbor.DynamicReflection;
	using Arbor.Serialization;

	public static class SerializedPropertyUtility
	{
		public static bool EqualContents(SerializedProperty x, SerializedProperty y)
		{
			try
			{
				return SerializedProperty.EqualContents(x, y);
			}
			catch
			{
				return false;
			}
		}

		private static System.Type GetScriptTypeFromProperty(SerializedProperty property)
		{
			SerializedProperty property1 = property.serializedObject.FindProperty("m_Script");
			if (property1 == null)
			{
				return null;
			}
			MonoScript monoScript = property1.objectReferenceValue as MonoScript;
			if (monoScript == null)
			{
				return null;
			}
			return monoScript.GetClass();
		}

		private sealed class FieldList
		{
			public System.Type type
			{
				get;
				private set;
			}

			internal sealed class Field
			{
				public System.Type fieldType
				{
					get;
					private set;
				}
				public FieldInfo fieldInfo
				{
					get;
					private set;
				}
				public DynamicField dynamicField
				{
					get;
					private set;
				}

				public Field(System.Type type, FieldInfo fieldInfo)
				{
					this.fieldType = type;
					this.fieldInfo = fieldInfo;
					this.dynamicField = DynamicField.GetField(fieldInfo);
				}
			}

			private Dictionary<string, Field> _Fields = new Dictionary<string, Field>();

			public FieldList(System.Type type)
			{
				this.type = type;
			}

			private static readonly System.Text.RegularExpressions.Regex s_ToFieldPathRegex = new System.Text.RegularExpressions.Regex(@"Array.data\[[0-9]*\]");
			private const string kArrayName = "[Array]";

			public static string ToFieldPath(string propertyPath)
			{
				return s_ToFieldPathRegex.Replace(propertyPath, kArrayName);
			}

			private static Field GetFieldInfoFromPropertyPath(System.Type typeFromProperty, string path)
			{
				FieldInfo fieldInfo1 = null;
				System.Type type = typeFromProperty;
				string[] strArray = path.Split('.');
				int length = strArray.Length;
				for (int index2 = 0; index2 < length; ++index2)
				{
					string name = strArray[index2];
					if (name == kArrayName)
					{
						type = SerializationUtility.ElementType(type);
					}
					else
					{
						FieldInfo fieldInfo2 = null;
						for (System.Type type1 = type; fieldInfo2 == null && type1 != null; type1 = type1.BaseType)
						{
							fieldInfo2 = type1.GetField(name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
						}
						if (fieldInfo2 == null)
						{
							type = null;
							return null;
						}
						fieldInfo1 = fieldInfo2;
						type = fieldInfo1.FieldType;
					}
				}

				if (fieldInfo1 == null)
				{
					return null;
				}

				return new Field(type, fieldInfo1);
			}

			public Field GetField(string path)
			{
				Field field = null;
				if (!_Fields.TryGetValue(path, out field))
				{
					field = GetFieldInfoFromPropertyPath(type, path);
					if (field != null)
					{
						_Fields.Add(path, field);
					}
				}

				return field;
			}

			public Field GetField(SerializedProperty property)
			{
				string path = ToFieldPath(property.propertyPath);
				return GetField(path);
			}
		}

		private static Dictionary<System.Type, FieldList> _FieldLists = new Dictionary<System.Type, FieldList>();

		private static FieldList GetFieldList(System.Type typeFromProperty)
		{
			FieldList fieldList = null;
			if (!_FieldLists.TryGetValue(typeFromProperty, out fieldList))
			{
				fieldList = new FieldList(typeFromProperty);
				_FieldLists.Add(typeFromProperty, fieldList);
			}
			return fieldList;
		}

		public static FieldInfo GetFieldInfoFromProperty(SerializedProperty property, out System.Type fieldType)
		{
			System.Type typeFromProperty = GetScriptTypeFromProperty(property);
			if (typeFromProperty != null)
			{
				FieldList fieldList = GetFieldList(typeFromProperty);

				FieldList.Field field = fieldList.GetField(property);
				if (field != null)
				{
					fieldType = field.fieldType;
					return field.fieldInfo;
				}
			}
			fieldType = null;
			return null;
		}

		static System.Text.StringBuilder s_ObjectPathBuilder = new System.Text.StringBuilder(1000);
		const string kPropertyArrayPrefix = ".Array.data[";

		static string GetPropertyPath(SerializedProperty property)
		{
			string path = property.propertyPath;
			if (path.Contains(kPropertyArrayPrefix))
			{
				s_ObjectPathBuilder.Length = 0;
				s_ObjectPathBuilder.Append(path);

				s_ObjectPathBuilder.Replace(kPropertyArrayPrefix, "[");
				path = s_ObjectPathBuilder.ToString();
			}
			return path;
		}

		private static object GetValue_Imp(object source, string name)
		{
			if (source == null)
			{
				return null;
			}

			System.Type type = source.GetType();

			FieldList fieldList = GetFieldList(type);

			FieldList.Field field = fieldList.GetField(name);
			if (field != null)
			{
				return field.dynamicField.GetValue(source);
			}

			return null;
		}

		private static object GetValue_Imp(object source, string name, int index)
		{
			IList list = GetValue_Imp(source, name) as IList;
			if (list == null || index < 0 || index >= list.Count)
			{
				return null;
			}

			return list[index];
		}

		public static object GetPropertyObject(SerializedProperty property)
		{
			object obj = property.serializedObject.targetObject;

			string path = GetPropertyPath(property);

			string[] elements = path.Split('.');

			int length = elements.Length;
			for (int i = 0; i < length; i++)
			{
				string element = elements[i];

				if (element.Contains('['))
				{
					var array = element.Split('[', ']');
					var elementName = array[0];
					var index = System.Convert.ToInt32(array[1]);
					obj = GetValue_Imp(obj, elementName, index);
				}
				else
				{
					obj = GetValue_Imp(obj, element);
				}
			}
			return obj;
		}

		public static T GetPropertyObject<T>(SerializedProperty property)
		{
			return (T)GetPropertyObject(property);
		}

		public static IEnumerable<object> GetPropertyObjects(SerializedProperty property)
		{
			string path = GetPropertyPath(property);

			string[] elements = path.Split('.');

			int length = elements.Length;

			var targetObjects = property.serializedObject.targetObjects;
			for (int targetObjectIndex = 0; targetObjectIndex < targetObjects.Length; targetObjectIndex++)
			{
				Object targetObj = targetObjects[targetObjectIndex];
				object obj = targetObj;

				for (int elementIndex = 0; elementIndex < length; elementIndex++)
				{
					string element = elements[elementIndex];

					if (element.Contains('['))
					{
						var array = element.Split('[', ']');
						var elementName = array[0];
						var index = System.Convert.ToInt32(array[1]);
						obj = GetValue_Imp(obj, elementName, index);
					}
					else
					{
						obj = GetValue_Imp(obj, element);
					}
				}
				yield return obj;
			}
		}
	}
}
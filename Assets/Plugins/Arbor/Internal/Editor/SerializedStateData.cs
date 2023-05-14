//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEditor;
using System.Collections.Generic;

namespace ArborEditor
{
	internal static class SerializedStateData<T>
	{
		private static Dictionary<SerializedPropertyKey, T> s_StateData = new Dictionary<SerializedPropertyKey, T>();

		public static void SetData(SerializedProperty property, T data)
		{
			SerializedPropertyKey key = new SerializedPropertyKey(property);
			s_StateData[key] = data;
		}

		public static T GetData(SerializedProperty property)
		{
			SerializedPropertyKey key = new SerializedPropertyKey(property);

			T data;
			if (s_StateData.TryGetValue(key, out data))
			{
				return data;
			}

			return default(T);
		}

		public static bool TryGetData(SerializedProperty property, out T data)
		{
			SerializedPropertyKey key = new SerializedPropertyKey(property);

			return s_StateData.TryGetValue(key, out data);
		}

		public static bool HasData(SerializedProperty property)
		{
			SerializedPropertyKey key = new SerializedPropertyKey(property);

			return s_StateData.ContainsKey(key);
		}

		public static bool Remove(SerializedProperty property)
		{
			SerializedPropertyKey key = new SerializedPropertyKey(property);

			return s_StateData.Remove(key);
		}

		public static void Clear()
		{
			s_StateData.Clear();
		}
	}
}

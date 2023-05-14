//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace ArborEditor
{
	public sealed class PropertyHeightCache
	{
		private Dictionary<SerializedPropertyKey, float> _Cache = new Dictionary<SerializedPropertyKey, float>();

		public bool TryGetHeight(SerializedProperty property, out float height)
		{
			SerializedPropertyKey key = new SerializedPropertyKey(property);
			return _Cache.TryGetValue(key, out height);
		}

		public void AddHeight(SerializedProperty property, float height)
		{
			SerializedPropertyKey key = new SerializedPropertyKey(property);
			_Cache.Add(key, height);
		}

		public float GetPropertyHeight(SerializedProperty property, GUIContent label, bool includeChildren)
		{
			float height;
			if (!TryGetHeight(property, out height))
			{
				height = EditorGUI.GetPropertyHeight(property, label, includeChildren);
				AddHeight(property, height);
			}
			return height;
		}

		public float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			float height;
			if (!TryGetHeight(property, out height))
			{
				height = EditorGUI.GetPropertyHeight(property, label);
				AddHeight(property, height);
			}
			return height;
		}

		public float GetPropertyHeight(SerializedProperty property)
		{
			float height;
			if (!TryGetHeight(property, out height))
			{
				height = EditorGUI.GetPropertyHeight(property);
				AddHeight(property, height);
			}
			return height;
		}

		public void Clear()
		{
			_Cache.Clear();
		}
	}
}
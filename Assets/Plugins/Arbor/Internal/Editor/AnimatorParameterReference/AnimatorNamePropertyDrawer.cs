//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace ArborEditor
{
	using Arbor;

	[CustomPropertyDrawer(typeof(AnimatorName))]
	public class AnimatorNamePropertyDrawer : PropertyDrawer
	{
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			SerializedProperty nameProperty = property.FindPropertyRelative("_Name");

			EditorGUI.PropertyField(position, nameProperty, label);
		}

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			SerializedProperty nameProperty = property.FindPropertyRelative("_Name");

			return EditorGUI.GetPropertyHeight(nameProperty, label);
		}
	}
}
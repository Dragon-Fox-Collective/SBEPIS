//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using UnityEditor;

namespace ArborEditor
{
	using Arbor;

	[CustomPropertyDrawer(typeof(TagChecker))]
	internal sealed class TagCheckerPropertyDrawer : PropertyDrawer
	{
		void LabelField(Rect position, SerializedProperty property, GUIContent label)
		{
			label = EditorGUI.BeginProperty(position, label, property);

			EditorGUI.LabelField(position, label);

			EditorGUI.EndProperty();
		}

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			position.height = EditorGUIUtility.singleLineHeight;

			bool showLabel = label != GUIContent.none;

			if (showLabel)
			{
				LabelField(position, property, label);

				EditorGUI.indentLevel++;

				position.y += position.height + EditorGUIUtility.standardVerticalSpacing;
			}

			EditorGUI.PropertyField(position, property.FindPropertyRelative("_IsCheckTag"));

			position.y += position.height + EditorGUIUtility.standardVerticalSpacing;

			EditorGUI.PropertyField(position, property.FindPropertyRelative("_Tag"));

			if (showLabel)
			{
				EditorGUI.indentLevel--;
			}
		}

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			float height = EditorGUIUtility.singleLineHeight * 2 + EditorGUIUtility.standardVerticalSpacing;

			bool showLabel = label != GUIContent.none;
			if (showLabel)
			{
				height += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
			}

			return height;
		}
	}
}
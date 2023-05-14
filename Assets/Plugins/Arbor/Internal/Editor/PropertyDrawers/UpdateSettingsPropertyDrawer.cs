//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using UnityEditor;

namespace ArborEditor
{
	using Arbor;
	[CustomPropertyDrawer(typeof(UpdateSettings))]
	internal sealed class UpdateSettingsPropertyDrawer : PropertyDrawer
	{
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			if (property.IsInvalidManagedReference())
			{
				EditorGUI.PropertyField(position, property, label);
				return;
			}

			EditorGUI.BeginProperty(position, label, property);

			EditorGUI.PrefixLabel(position, label);

			position.height = EditorGUIUtility.singleLineHeight;

			EditorGUI.indentLevel++;
			position.y += position.height;
			SerializedProperty typeProperty = property.FindPropertyRelative("type");
			EditorGUI.PropertyField(position, typeProperty);

			UpdateType updateType = EnumUtility.GetValueFromIndex<UpdateType>(typeProperty.enumValueIndex);
			if (updateType == UpdateType.SpecifySeconds)
			{
				position.y += position.height;
				EditorGUI.PropertyField(position, property.FindPropertyRelative("timeType"));
				position.y += position.height;
				EditorGUI.PropertyField(position, property.FindPropertyRelative("seconds"));
			}
			EditorGUI.indentLevel--;

			EditorGUI.EndProperty();
		}

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			if (property.IsInvalidManagedReference())
			{
				return EditorGUI.GetPropertyHeight(property, label);
			}

			float height = EditorGUIUtility.singleLineHeight * 2;
			SerializedProperty typeProperty = property.FindPropertyRelative("type");
			UpdateType updateType = EnumUtility.GetValueFromIndex<UpdateType>(typeProperty.enumValueIndex);
			if (updateType == UpdateType.SpecifySeconds)
			{
				height += EditorGUIUtility.singleLineHeight * 2;
			}
			return height;
		}
	}
}
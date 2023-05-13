//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using UnityEditor;

namespace ArborEditor.BehaviourTree
{
	using Arbor.BehaviourTree;

	[CustomPropertyDrawer(typeof(ExecutionSettings))]
	internal sealed class ExecutionSettingsPropertyDrawer : PropertyDrawer
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

			ExecutionType executionType = EnumUtility.GetValueFromIndex<ExecutionType>(typeProperty.enumValueIndex);
			if (executionType == ExecutionType.Count)
			{
				position.y += position.height;
				EditorGUI.PropertyField(position, property.FindPropertyRelative("maxCount"));
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
			ExecutionType executionType = EnumUtility.GetValueFromIndex<ExecutionType>(typeProperty.enumValueIndex);
			if (executionType == ExecutionType.Count)
			{
				height += EditorGUIUtility.singleLineHeight;
			}
			return height;
		}
	}
}
//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor;
using UnityEditor.UIElements;

namespace ArborEditor
{
	using Arbor;
	using UnityEngine.UIElements;

	[CustomPropertyDrawer(typeof(LogicalCondition))]
	internal sealed class LogicalConditionDrawer : PropertyDrawer
	{
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			label = EditorGUI.BeginProperty(position, label, property);

			position = EditorGUI.PrefixLabel(position, 0, label);

			var logicalOpProperty = property.FindPropertyRelative("logicalOperation");
			var logicalOpRect = new Rect(position);
			logicalOpRect.width = position.width * 0.5f - 2f;

			EditorGUI.PropertyField(logicalOpRect, logicalOpProperty, GUIContent.none);

			var notProperty = property.FindPropertyRelative("notOp");
			var notToggleRect = new Rect(position);
			notToggleRect.xMin += logicalOpRect.width + 2f;

			notProperty.boolValue = GUI.Toggle(notToggleRect, notProperty.boolValue, notProperty.displayName, GUI.skin.button);

			EditorGUI.EndProperty();
		}

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			return EditorGUIUtility.singleLineHeight;
		}
	}
}
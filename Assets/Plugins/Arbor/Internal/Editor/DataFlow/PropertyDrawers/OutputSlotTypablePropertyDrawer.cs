//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using UnityEditor;

namespace ArborEditor
{
	using Arbor;

	[CustomPropertyDrawer(typeof(OutputSlotTypable))]
	internal sealed class OutputSlotTypablePropertyDrawer : DataSlotPropertyDrawer
	{
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			if (ArborEditorWindow.isInArborEditor && !AttributeHelper.HasAttribute<HideSlotFields>(fieldInfo))
			{
				SerializedProperty typeProperty = property.FindPropertyRelative("_Type");
				Rect typePosition = new Rect(position);
				typePosition.width = EditorGUIUtility.labelWidth - 2f;
				typePosition.height = EditorGUIUtility.singleLineHeight;

				EditorGUI.PropertyField(typePosition, typeProperty, GUIContent.none, true);

				Rect slotPosition = new Rect(position);

				slotPosition.xMin += EditorGUIUtility.labelWidth;
				slotPosition.height = EditorGUIUtility.singleLineHeight;

				int indentLevel = EditorGUI.indentLevel;
				EditorGUI.indentLevel = 0;
				base.OnGUI(slotPosition, property, label);
				EditorGUI.indentLevel = indentLevel;
			}
			else
			{
				base.OnGUI(position, property, label);
			}
		}

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			float height = 0.0f;
			if (ArborEditorWindow.isInArborEditor && !AttributeHelper.HasAttribute<HideSlotFields>(fieldInfo))
			{
				height += EditorGUIUtility.singleLineHeight;
			}
			else
			{
				height += base.GetPropertyHeight(property, label);
			}
			return height;
		}
	}
}
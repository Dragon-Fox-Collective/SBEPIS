//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using UnityEditor;

namespace ArborEditor
{
	using Arbor;

	[CustomPropertyDrawer(typeof(SendTriggerFlags))]
	internal sealed class SendTriggerFlagsPropertyDrawer : PropertyDrawer
	{
		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			return EditorGUIUtility.singleLineHeight;
		}

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			label = EditorGUI.BeginProperty(position, label, property);

			EditorGUI.BeginChangeCheck();
			SendTriggerFlags flags = (SendTriggerFlags)EditorGUI.EnumFlagsField(position, label, (SendTriggerFlags)property.intValue);
			if (EditorGUI.EndChangeCheck())
			{
				property.intValue = (int)flags;
			}

			EditorGUI.EndProperty();
		}
	}
}
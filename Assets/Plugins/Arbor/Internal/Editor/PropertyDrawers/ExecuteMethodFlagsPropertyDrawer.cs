//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using UnityEditor;

namespace ArborEditor
{
	using Arbor;

	[CustomPropertyDrawer(typeof(ExecuteMethodFlags))]
	internal sealed class ExecuteMethodFlagsPropertyDrawer : PropertyDrawer
	{
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			label = EditorGUI.BeginProperty(position, label, property);

			EditorGUI.BeginChangeCheck();
			ExecuteMethodFlags executeMethodFlags = (ExecuteMethodFlags)EditorGUI.EnumFlagsField(position, label, (ExecuteMethodFlags)property.intValue);
			if (EditorGUI.EndChangeCheck())
			{
				property.intValue = (int)executeMethodFlags;
			}

			EditorGUI.EndProperty();
		}

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			return base.GetPropertyHeight(property, label);
		}
	}
}
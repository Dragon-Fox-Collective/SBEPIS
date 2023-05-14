//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using UnityEditor;

namespace ArborEditor
{
	using Arbor;

#pragma warning disable 0618
	[CustomPropertyDrawer(typeof(TransformUpdateTiming))]
	internal sealed class TransformUpdateTimingPropertyDrawer : PropertyDrawer
	{
		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			return EditorGUIUtility.singleLineHeight;
		}

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			label = EditorGUI.BeginProperty(position, label, property);

			EditorGUI.BeginChangeCheck();
			TransformUpdateTiming updateTiming = (TransformUpdateTiming)EditorGUI.EnumFlagsField(position, label, (TransformUpdateTiming)property.intValue);
			if (EditorGUI.EndChangeCheck())
			{
				property.intValue = (int)updateTiming;
			}

			EditorGUI.EndProperty();
		}
	}
#pragma warning restore 0618
}
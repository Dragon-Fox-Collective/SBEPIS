//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using UnityEditor;

using Arbor;

namespace ArborEditor
{
	[CustomPropertyDrawer(typeof(StateLink))]
	internal sealed class StateLinkPropertyDrawer : PropertyDrawer
	{
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
		}

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			return -EditorGUIUtility.standardVerticalSpacing;
		}
	}
}
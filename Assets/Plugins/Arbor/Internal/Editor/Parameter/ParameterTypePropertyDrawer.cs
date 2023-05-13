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

	[CustomPropertyDrawer(typeof(Parameter.Type))]
	internal sealed class ParameterTypePropertyDrawer : PropertyDrawer
	{
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			var parameterType = EnumUtility.GetValueFromIndex<Parameter.Type>(property.enumValueIndex);

			EditorGUI.BeginChangeCheck();
			parameterType = ParameterTypeMenuItem.Popup(position, label, parameterType);
			if (EditorGUI.EndChangeCheck())
			{
				property.enumValueIndex = EnumUtility.GetIndexFromValue<Parameter.Type>(parameterType);
			}
		}
	}
}
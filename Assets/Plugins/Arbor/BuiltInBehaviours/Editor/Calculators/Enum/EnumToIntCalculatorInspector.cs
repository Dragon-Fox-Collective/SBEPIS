//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace ArborEditor.Calculators
{
	using Arbor;
	using Arbor.Calculators;
	using ArborEditor.Inspectors;

	[CustomEditor(typeof(EnumToIntCalculator))]
	internal sealed class EnumToIntCalculatorInspector : InspectorBase
	{
		private ClassTypeReferenceProperty _TypeProperty;
		private FlexibleFieldProperty _ValueProperty;

		protected override void OnRegisterElements()
		{
			_TypeProperty = new ClassTypeReferenceProperty(serializedObject.FindProperty("_Type"));
			_ValueProperty = new FlexibleFieldProperty(serializedObject.FindProperty("_Value"));

			RegisterIMGUI(OnTypeGUI);

			RegisterProperty(_ValueProperty.property);
			RegisterProperty("_Output");
		}

		void OnTypeGUI()
		{
			System.Type enumType = _TypeProperty.type;

			EditorGUI.BeginChangeCheck();
			EditorGUILayout.PropertyField(_TypeProperty.property, true);
			if (EditorGUI.EndChangeCheck() && enumType != _TypeProperty.type)
			{
				_ValueProperty.Disconnect();

				serializedObject.ApplyModifiedProperties();

				GUIUtility.ExitGUI();
			}
		}

	}
}
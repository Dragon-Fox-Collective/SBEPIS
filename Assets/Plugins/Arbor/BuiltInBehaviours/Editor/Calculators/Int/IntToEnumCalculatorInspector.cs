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

	[CustomEditor(typeof(IntToEnumCalculator))]
	public class IntToEnumCalculatorInspector : InspectorBase
	{
		private OutputSlotTypableProperty _OutputProperty;

		protected override void OnRegisterElements()
		{
			_OutputProperty = new OutputSlotTypableProperty(serializedObject.FindProperty("_Output"));

			RegisterIMGUI(OnTypeGUI);

			RegisterProperty("_Value");
			RegisterProperty(_OutputProperty.property);
		}

		void OnTypeGUI()
		{
			ClassTypeReferenceProperty typeProperty = _OutputProperty.typeProperty;

			System.Type enumType = typeProperty.type;

			typeProperty.SetConstraint(ClassTypeConstraintEditorUtility.enumField);
			EditorGUI.BeginChangeCheck();
			EditorGUILayout.PropertyField(typeProperty.property, true);
			if (EditorGUI.EndChangeCheck() && enumType != typeProperty.type)
			{
				_OutputProperty.Disconnect();

				serializedObject.ApplyModifiedProperties();

				GUIUtility.ExitGUI(); // throw ExitGUIException
			}
		}
	}
}
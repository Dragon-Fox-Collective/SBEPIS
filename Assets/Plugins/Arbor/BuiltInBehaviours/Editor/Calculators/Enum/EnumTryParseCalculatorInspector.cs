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

	[CustomEditor(typeof(EnumTryParseCalculator))]
	internal sealed class EnumTryParseCalculatorInspector : InspectorBase
	{
		private OutputSlotTypableProperty _ResultProperty;

		protected override void OnRegisterElements()
		{
			_ResultProperty = new OutputSlotTypableProperty(serializedObject.FindProperty("_Result"));

			RegisterIMGUI(OnTypeGUI);

			RegisterProperty("_String");
			RegisterProperty("_IgnoreCase");
			RegisterProperty("_Success");
			RegisterProperty(_ResultProperty.property, true);
		}

		void OnTypeGUI()
		{
			ClassTypeReferenceProperty typeProperty = _ResultProperty.typeProperty;

			System.Type enumType = typeProperty.type;

			typeProperty.SetConstraint(ClassTypeConstraintEditorUtility.enumField);
			EditorGUI.BeginChangeCheck();
			EditorGUILayout.PropertyField(typeProperty.property, true);
			if (EditorGUI.EndChangeCheck() && enumType != typeProperty.type)
			{
				_ResultProperty.Disconnect();

				serializedObject.ApplyModifiedProperties();

				GUIUtility.ExitGUI(); // throw ExitGUIException
			}
		}
	}
}
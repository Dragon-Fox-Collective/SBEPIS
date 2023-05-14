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

	[CustomEditor(typeof(EnumEqualsCalculator))]
	internal sealed class EnumEqualsCalculatorInspector : InspectorBase
	{
		private ClassTypeReferenceProperty _TypeProperty;
		private FlexibleFieldProperty _Value1Property;
		private FlexibleFieldProperty _Value2Property;

		protected override void OnRegisterElements()
		{
			_TypeProperty = new ClassTypeReferenceProperty(serializedObject.FindProperty("_Type"));
			_Value1Property = new FlexibleFieldProperty(serializedObject.FindProperty("_Value1"));
			_Value2Property = new FlexibleFieldProperty(serializedObject.FindProperty("_Value2"));
			
			RegisterIMGUI(OnTypeGUI);

			RegisterProperty(_Value1Property.property);
			RegisterProperty(_Value2Property.property);
			RegisterProperty("_Result");
		}

		void OnTypeGUI()
		{
			System.Type enumType = _TypeProperty.type;

			EditorGUI.BeginChangeCheck();
			EditorGUILayout.PropertyField(_TypeProperty.property, true);
			if (EditorGUI.EndChangeCheck() && enumType != _TypeProperty.type)
			{
				_Value1Property.Disconnect();
				_Value2Property.Disconnect();

				serializedObject.ApplyModifiedProperties();

				GUIUtility.ExitGUI();
			}
		}
	}
}
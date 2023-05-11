//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using UnityEditor;

namespace ArborEditor.Calculators
{
	using Arbor;
	using Arbor.Calculators;
	using ArborEditor.Inspectors;

	[CustomEditor(typeof(EnumFlagsAddCalculator))]
	internal sealed class EnumFlagsAddCalculatorInspector : InspectorBase
	{
		private FlexibleFieldProperty _Value1Property;
		private FlexibleFieldProperty _Value2Property;
		private OutputSlotTypableProperty _ResultProperty;

		protected override void OnRegisterElements()
		{
			_Value1Property = new FlexibleFieldProperty(serializedObject.FindProperty("_Value1"));
			_Value2Property = new FlexibleFieldProperty(serializedObject.FindProperty("_Value2"));
			_ResultProperty = new OutputSlotTypableProperty(serializedObject.FindProperty("_Result"));

			RegisterIMGUI(OnTypeGUI);

			RegisterProperty(_Value1Property.property);
			RegisterProperty(_Value2Property.property);
			RegisterProperty(_ResultProperty.property);
		}

		void OnTypeGUI()
		{
			ClassTypeReferenceProperty typeProperty = _ResultProperty.typeProperty;

			System.Type enumType = typeProperty.type;

			typeProperty.SetConstraint(ClassTypeConstraintEditorUtility.enumFlags);
			EditorGUI.BeginChangeCheck();
			EditorGUILayout.PropertyField(typeProperty.property, true);
			if (EditorGUI.EndChangeCheck() && enumType != typeProperty.type)
			{
				_Value1Property.Disconnect();
				_Value2Property.Disconnect();
				_ResultProperty.Disconnect();

				serializedObject.ApplyModifiedProperties();

				GUIUtility.ExitGUI(); // throw ExitGUIException
			}
		}
	}
}
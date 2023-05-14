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
	using Arbor.Events;
	using ArborEditor.Events;

	[CustomEditor(typeof(ListLastIndexOfCalculator))]
	internal sealed class ListLastIndexOfCalculatorInspector : ListElementCalculatorBaseInspector
	{
		private const string kOutputPath = "_Output";

		private OutputSlotBaseProperty _OutputProperty;

		protected override void OnEnable()
		{
			base.OnEnable();

			_OutputProperty = new OutputSlotBaseProperty(serializedObject.FindProperty(kOutputPath));
		}

		protected override void OnOutputGUI()
		{
			EditorGUILayout.PropertyField(_OutputProperty.property, GUIContentCaches.Get(_OutputProperty.property.displayName), true, GUILayout.Width(70f));
		}
	}
}
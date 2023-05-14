//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using UnityEditor;

namespace ArborEditor.Calculators
{
	using Arbor.Calculators;
	using ArborEditor.Inspectors;

	[CustomEditor(typeof(GetValueCalculator))]
	internal sealed class GetValueCalculatorInspector : InspectorBase
	{
		protected override void OnRegisterElements()
		{
			RegisterProperty("_Persistent", "");
		}
	}
}
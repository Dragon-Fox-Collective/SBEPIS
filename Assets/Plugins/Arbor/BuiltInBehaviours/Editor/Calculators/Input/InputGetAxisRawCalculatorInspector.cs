//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
#if ENABLE_LEGACY_INPUT_MANAGER
using UnityEditor;

namespace ArborEditor.Calculators
{
	using Arbor.Calculators;
	using ArborEditor.Inspectors;

	[CustomEditor(typeof(InputGetAxisRawCalculator))]
	internal sealed class InputGetAxisRawCalculatorInspector : InspectorBase
	{
		protected override void OnRegisterElements()
		{
			RegisterProperty("_AxisName");
			RegisterProperty("_Output");
		}
	}
}
#endif
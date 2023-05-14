//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEditor;

namespace ArborEditor.StateMachine.StateBehaviours
{
	using Arbor.StateMachine.StateBehaviours;
	using ArborEditor.Inspectors;

	[CustomEditor(typeof(CalculatorTransition))]
	internal sealed class CalculatorTransitionInspector : InspectorBase
	{
		protected override void OnRegisterElements()
		{
			RegisterProperty("_TriggerFlags");
			RegisterProperty("_ConditionList");
		}
	}
}

//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEditor;

namespace ArborEditor.BehaviourTree.Decorators
{
	using Arbor.BehaviourTree.Decorators;
	using ArborEditor.Inspectors;

	[CustomEditor(typeof(ParameterConditionalLoop))]
	internal sealed class ParameterConditionalLoopInspector : InspectorBase
	{
		protected override void OnRegisterElements()
		{
			RegisterProperty("_ConditionList");
		}
	}
}
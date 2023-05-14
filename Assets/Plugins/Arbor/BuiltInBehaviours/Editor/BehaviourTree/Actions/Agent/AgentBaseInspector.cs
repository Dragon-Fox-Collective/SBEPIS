//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEditor;

namespace ArborEditor.BehaviourTree.Actions
{
	using ArborEditor.Inspectors;

	public class AgentBaseInspector : InspectorBase
	{
		protected override void OnRegisterElements()
		{
			RegisterProperty("_AgentController");
		}
	}
}
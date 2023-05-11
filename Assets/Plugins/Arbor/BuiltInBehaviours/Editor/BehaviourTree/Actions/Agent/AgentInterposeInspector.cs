//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEditor;

namespace ArborEditor.BehaviourTree.Actions
{
	using Arbor.BehaviourTree.Actions;

	[CustomEditor(typeof(AgentInterpose))]
	internal sealed class AgentInterposeInspector : AgentMoveBaseInspector
	{
		protected override void OnRegisterElements()
		{
			base.OnRegisterElements();

			RegisterProperty("_TargetA");
			RegisterProperty("_TargetB");
			RegisterProperty("_StoppingDistance");
		}
	}
}

//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEditor;

namespace ArborEditor.BehaviourTree.Actions
{
	using Arbor.BehaviourTree.Actions;

	[CustomEditor(typeof(AgentMoveToTransform))]
	internal sealed class AgentMoveToTransformInspector : AgentMoveBaseInspector
	{
		protected override void OnRegisterElements()
		{
			base.OnRegisterElements();

			RegisterProperty("_TargetTransform");
			RegisterProperty("_StoppingDistance");
		}
	}
}

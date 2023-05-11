//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEditor;

namespace ArborEditor.BehaviourTree.Actions
{
	using Arbor.BehaviourTree.Actions;

	[CustomEditor(typeof(AgentWarpToPosition))]
	internal sealed class AgentWarpToPositionInspector : AgentBaseInspector
	{
		protected override void OnRegisterElements()
		{
			base.OnRegisterElements();

			RegisterSpace();
			RegisterProperty("_TargetPosition");
		}
	}
}
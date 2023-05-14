//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEditor;

namespace ArborEditor.BehaviourTree.Actions
{
	using Arbor.BehaviourTree.Actions;

	[CustomEditor(typeof(AgentHideFromTransform))]
	internal sealed class AgentHideFromTransformInspector : AgentMoveBaseInspector
	{
		protected override void OnRegisterElements()
		{
			base.OnRegisterElements();

			RegisterProperty("_ObstacleTargetFlags");
			RegisterProperty("_ObstacleSearchFlags");
			RegisterProperty("_ObstacleLayer");
			RegisterProperty("_MinDistanceToTarget");
			RegisterProperty("_TargetTransform");
		}
	}
}

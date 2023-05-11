//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEditor;

using Arbor.StateMachine.StateBehaviours;

namespace ArborEditor.StateMachine.StateBehaviours
{
	[CustomEditor(typeof(AgentMoveOnWaypoint))]
	internal sealed class AgentMoveOnWaypointInspector : AgentUpdateBaseInspector
	{
		protected override void OnRegisterElements()
		{
			base.OnRegisterElements();

			RegisterSpace();

			RegisterProperty("_Speed");
			RegisterProperty("_Waypoint");
			RegisterProperty("_Radius");
			RegisterProperty("_ClearDestPoint");
			RegisterProperty("_Type");
			RegisterProperty("_StoppingDistance");
		}
	}
}
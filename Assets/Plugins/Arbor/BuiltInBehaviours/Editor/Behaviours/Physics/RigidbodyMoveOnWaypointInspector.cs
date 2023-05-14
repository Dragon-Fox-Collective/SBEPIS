//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEditor;

using Arbor.StateMachine.StateBehaviours;

namespace ArborEditor.StateMachine.StateBehaviours
{
	using ArborEditor.Inspectors;

	[CustomEditor(typeof(RigidbodyMoveOnWaypoint))]

	internal sealed class RigidbodyMoveOnWaypointInspector : InspectorBase
	{
		protected override void OnRegisterElements()
		{
			RegisterProperty("_Target");
			RegisterProperty("_Speed");
			RegisterProperty("_Waypoint");
			RegisterProperty("_Offset");
			RegisterProperty("_ClearDestPoint");
			RegisterProperty("_Type");
		}
	}
}
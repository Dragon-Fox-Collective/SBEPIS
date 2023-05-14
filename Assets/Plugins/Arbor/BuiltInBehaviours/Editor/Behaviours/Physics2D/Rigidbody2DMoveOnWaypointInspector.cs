//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using UnityEditor;

namespace ArborEditor.StateMachine.StateBehaviours
{
	using Arbor;
	using Arbor.StateMachine.StateBehaviours;
	using ArborEditor.Inspectors;

	[CustomEditor(typeof(Rigidbody2DMoveOnWaypoint))]
	public class Rigidbody2DMoveOnWaypointInspector : InspectorBase
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
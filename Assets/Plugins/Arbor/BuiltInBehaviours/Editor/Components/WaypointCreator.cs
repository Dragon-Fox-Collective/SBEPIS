//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using UnityEditor;

using Arbor;

namespace ArborEditor
{
	internal static class WaypointCreator
	{
		[MenuItem("GameObject/Arbor/Waypoint", false, 21)]
		static void CreateWaypoint(MenuCommand menuCommand)
		{
			GameObject gameObject = new GameObject("Waypoint", typeof(Waypoint));
			GameObjectUtility.SetParentAndAlign(gameObject, menuCommand.context as GameObject);
			Undo.RegisterCreatedObjectUndo(gameObject, "Create Waypoint");
			Selection.activeGameObject = gameObject;
		}
	}
}

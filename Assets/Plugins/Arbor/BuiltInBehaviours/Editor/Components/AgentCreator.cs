//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using UnityEngine.AI;
using UnityEditor;

using Arbor;

namespace ArborEditor
{
	internal static class AgentCreator
	{
		[MenuItem("GameObject/Arbor/AgentController", false, 20)]
		static void CreateAgent(MenuCommand menuCommand)
		{
			GameObject gameObject = new GameObject("Agent", typeof(NavMeshAgent), typeof(AgentController));
			GameObjectUtility.SetParentAndAlign(gameObject, menuCommand.context as GameObject);
			Undo.RegisterCreatedObjectUndo(gameObject, "Create Agent");
			Selection.activeGameObject = gameObject;
		}
	}
}

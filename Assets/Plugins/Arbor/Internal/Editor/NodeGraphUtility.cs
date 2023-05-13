//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using UnityEditor;

using Arbor;

namespace ArborEditor
{
	public static class NodeGraphUtility
	{
		public static NodeGraph CreateGraphObject(System.Type graphType, string graphName, GameObject parent)
		{
			GameObject gameObject = new GameObject(graphName);

			ComponentUtility.useEditorProcessor = false;
			NodeGraph nodeGraph = NodeGraph.Create(gameObject, graphType);
			ComponentUtility.useEditorProcessor = true;

			GameObjectUtility.SetParentAndAlign(gameObject, parent);

			Undo.RegisterCreatedObjectUndo(gameObject, "Create " + graphType.Name);
			Selection.activeGameObject = gameObject;

			return nodeGraph;
		}
	}
}
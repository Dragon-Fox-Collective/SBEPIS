//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using UnityEditor;

using Arbor;
namespace ArborEditor
{
	internal static class NodeGraphMenu
	{
		[MenuItem("CONTEXT/NodeGraph/Copy Component")]
		static void CopyComponent(MenuCommand command)
		{
			NodeGraph nodeGraph = command.context as NodeGraph;
			if (nodeGraph != null)
			{
				Clipboard.CopyNodeGraph(nodeGraph);
				return;
			}

			Component component = command.context as Component;
			if (component != null)
			{
				UnityEditorInternal.ComponentUtility.CopyComponent(component);
				return;
			}

			Debug.LogError("NodeGraph : Can't Copy Component");
		}

		[MenuItem("CONTEXT/NodeGraph/Paste NodeGraph Values", true)]
		static bool ValidatePasteNodeGraphValues(MenuCommand command)
		{
			NodeGraph nodeGraph = command.context as NodeGraph;
			if (nodeGraph != null)
			{
				return nodeGraph.GetType() == Clipboard.copiedComponentType;
			}

			return false;
		}

		[MenuItem("CONTEXT/NodeGraph/Paste NodeGraph Values")]
		static void PasteNodeGraphValues(MenuCommand command)
		{
			NodeGraph nodeGraph = command.context as NodeGraph;
			if (nodeGraph != null)
			{
				Clipboard.PasteNodeGraphValues(nodeGraph);
			}
		}

		[MenuItem("CONTEXT/Component/Paste NodeGraph As New", true)]
		static bool ValidatePasteNodeGraphAsNew(MenuCommand command)
		{
			System.Type copiedComponentType = Clipboard.copiedComponentType;
			if (copiedComponentType == null)
			{
				return false;
			}

			return typeof(NodeGraph).IsAssignableFrom(copiedComponentType);
		}

		[MenuItem("CONTEXT/Component/Paste NodeGraph As New")]
		static void PasteNodeGraphAsNew(MenuCommand command)
		{
			Component component = command.context as Component;
			if (component == null)
			{
				return;
			}

			GameObject gameObject = component.gameObject;
			if (gameObject == null)
			{
				return;
			}

			Clipboard.PasteNodeGraphAsNew(gameObject);
		}
	}
}
//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using UnityEditor;

namespace ArborEditor.BehaviourTree
{
	using Arbor;
	using Arbor.BehaviourTree;

	[System.Serializable]
	internal sealed class ServiceEditorList : BehaviourEditorList<ServiceEditorGUI, Service>
	{
		public TreeBehaviourNodeEditor treeNodeEditor
		{
			get
			{
				return nodeEditor as TreeBehaviourNodeEditor;
			}
		}

		public TreeBehaviourNode treeNode
		{
			get
			{
				return node as TreeBehaviourNode;
			}
		}

		private static readonly Color kBackGroundColor = new Color(0.9f, 1.0f, 0.9f);

		public override Color backgroundColor
		{
			get
			{
				return kBackGroundColor;
			}
		}

		public override string backgroundClassName
		{
			get
			{
				return "service-list-background";
			}
		}

		public override System.Type targetType
		{
			get
			{
				return typeof(Service);
			}
		}

		public override GUIContent GetAddBehaviourContent()
		{
			return EditorContents.addService;
		}

		public override GUIContent GetInsertButtonContent()
		{
			return EditorContents.insertService;
		}

		public override GUIContent GetPasteBehaviourContent()
		{
			return EditorContents.pasteService;
		}

		public override Object GetObject(int behaviourIndex)
		{
			return treeNode.serviceList[behaviourIndex];
		}

		public override int GetCount()
		{
			return treeNode.serviceList.count;
		}

		public override void InsertBehaviour(int index, System.Type classType)
		{
			treeNodeEditor.InsertService(index, classType);
		}

		public override void MoveBehaviour(Node fromNode, int fromIndex, Node toNode, int toIndex, bool isCopy)
		{
			TreeBehaviourNode fromTreeNode = fromNode as TreeBehaviourNode;
			TreeBehaviourNode toTreeNode = toNode as TreeBehaviourNode;

			NodeGraph nodeGraph = fromTreeNode.nodeGraph;

			Undo.IncrementCurrentGroup();

			Undo.RecordObject(nodeGraph, isCopy ? "Paste Behaviour" : "Move Behaviour");

			Service fromService = fromTreeNode.serviceList[fromIndex];
			Service toService = null;
			if (isCopy)
			{
				toService = TreeBehaviourNodeEditor.PasteServiceAsNew(toTreeNode, toIndex, fromService);
			}
			else
			{
				var fromTreeEditor = graphEditor.GetNodeEditor(fromTreeNode) as TreeBehaviourNodeEditor;
				if (fromTreeEditor != null)
				{
					fromTreeEditor.RemoveServiceEditor(fromIndex);
				}

				fromTreeNode.MoveService(fromIndex, toTreeNode, toIndex);

				toService = fromService;
			}

			if (toService != null)
			{
				var toTreeEditor = graphEditor.GetNodeEditor(toTreeNode) as TreeBehaviourNodeEditor;
				if (toTreeEditor != null)
				{
					toTreeEditor.InsertServiceEditor(toIndex, toService);
				}
			}

			Undo.CollapseUndoOperations(Undo.GetCurrentGroup());

			EditorUtility.SetDirty(nodeGraph);

			graphEditor.RaiseOnChangedNodes();
		}

		public override void PasteBehaviour(int index)
		{
			treeNodeEditor.PasteService(index);
		}

		public override void OpenBehaviourMenu(Rect buttonRect, int index)
		{
			ServiceMenuWindow.instance.Init(buttonRect, index, treeNodeEditor.CreateServiceByType);
		}
	}
}
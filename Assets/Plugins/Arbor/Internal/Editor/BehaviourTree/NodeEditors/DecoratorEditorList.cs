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
	internal sealed class DecoratorEditorList : BehaviourEditorList<DecoratorEditorGUI, Decorator>
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

		private static readonly Color kBackGroundColor = new Color(0.9f, 0.9f, 1.0f);

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
				return "decorator-list-background";
			}
		}

		public override System.Type targetType
		{
			get
			{
				return typeof(Decorator);
			}
		}

		public override GUIContent GetAddBehaviourContent()
		{
			return EditorContents.addDecorator;
		}

		public override GUIContent GetInsertButtonContent()
		{
			return EditorContents.insertDecorator;
		}

		public override GUIContent GetPasteBehaviourContent()
		{
			return EditorContents.pasteDecorator;
		}

		public override Object GetObject(int behaviourIndex)
		{
			return treeNode.decoratorList[behaviourIndex];
		}

		public override int GetCount()
		{
			return treeNode.decoratorList.count;
		}

		public override void InsertBehaviour(int index, System.Type classType)
		{
			treeNodeEditor.InsertDecorator(index, classType);
		}

		public override void MoveBehaviour(Node fromNode, int fromIndex, Node toNode, int toIndex, bool isCopy)
		{
			TreeBehaviourNode fromTreeNode = fromNode as TreeBehaviourNode;
			TreeBehaviourNode toTreeNode = toNode as TreeBehaviourNode;

			NodeGraph nodeGraph = fromTreeNode.nodeGraph;

			Undo.IncrementCurrentGroup();

			Undo.RecordObject(nodeGraph, isCopy ? "Paste Behaviour" : "Move Behaviour");

			Decorator fromDecorator = fromTreeNode.decoratorList[fromIndex];
			Decorator toDecorator = null;
			if (isCopy)
			{
				toDecorator = TreeBehaviourNodeEditor.PasteDecoratorAsNew(toTreeNode, toIndex, fromDecorator);
			}
			else
			{
				var fromTreeEditor = graphEditor.GetNodeEditor(fromTreeNode) as TreeBehaviourNodeEditor;
				if (fromTreeEditor != null)
				{
					fromTreeEditor.RemoveDecoratorEditor(fromIndex);
				}

				fromTreeNode.MoveDecorator(fromIndex, toTreeNode, toIndex);

				toDecorator = fromDecorator;
			}

			if (toDecorator != null)
			{
				var toTreeEditor = graphEditor.GetNodeEditor(toTreeNode) as TreeBehaviourNodeEditor;
				if (toTreeEditor != null)
				{
					toTreeEditor.InsertDecoratorEditor(toIndex, toDecorator);
				}
			}

			Undo.CollapseUndoOperations(Undo.GetCurrentGroup());

			EditorUtility.SetDirty(nodeGraph);

			graphEditor.RaiseOnChangedNodes();
		}

		public override void PasteBehaviour(int index)
		{
			treeNodeEditor.PasteDecorator(index);
		}

		public override void OpenBehaviourMenu(Rect buttonRect, int index)
		{
			DecoratorMenuWindow.instance.Init(buttonRect, index, treeNodeEditor.CreateDecoratorByType);
		}
	}
}
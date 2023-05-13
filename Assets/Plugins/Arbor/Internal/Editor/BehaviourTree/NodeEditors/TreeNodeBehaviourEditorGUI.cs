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
	public class TreeNodeBehaviourEditorGUI : BehaviourEditorGUI
	{
		public TreeBehaviourNodeEditor treeBehaviourEditor
		{
			get
			{
				return nodeEditor as TreeBehaviourNodeEditor;
			}
		}

		public TreeBehaviourNode treeBehaviourNode
		{
			get
			{
				return (nodeEditor != null) ? nodeEditor.node as TreeBehaviourNode : null;
			}
		}

		public TreeNodeBehaviour treeNodeBehaviour
		{
			get
			{
				return behaviourObj as TreeNodeBehaviour;
			}
		}
	}
}
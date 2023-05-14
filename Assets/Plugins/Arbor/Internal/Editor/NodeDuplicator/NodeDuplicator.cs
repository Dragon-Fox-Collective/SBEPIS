//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using System.Collections.Generic;

using Arbor;

namespace ArborEditor
{
	public abstract class NodeDuplicator : ScriptableObject
	{
		public NodeGraph targetGraph
		{
			get;
			private set;
		}

		public bool isClip
		{
			get;
			private set;
		}

		public Node sourceNode
		{
			get;
			private set;
		}

		public Node destNode
		{
			get;
			private set;
		}

		internal static NodeDuplicator CreateDuplicator(NodeGraph targetGraph, Node sourceNode, bool clip)
		{
			if (sourceNode == null)
			{
				return null;
			}

			System.Type classType = sourceNode.GetType();
			var editorInfo = CustomAttributes<CustomNodeDuplicator, NodeDuplicator>.FindEditorInfo(classType);

			if (editorInfo == null || editorInfo.editorType == null || !editorInfo.attribute.graphType.IsAssignableFrom(targetGraph.GetType()))
			{
				return null;
			}

			NodeDuplicator nodeDuplicator = CreateInstance(editorInfo.editorType) as NodeDuplicator;
			nodeDuplicator.hideFlags = HideFlags.HideAndDontSave;
			nodeDuplicator.Initialize(targetGraph, sourceNode, clip);

			return nodeDuplicator;
		}

		private Dictionary<NodeBehaviour, NodeBehaviour> _DuplicateBehaviours = new Dictionary<NodeBehaviour, NodeBehaviour>();

		private void Initialize(NodeGraph targetGraph, Node sourceNode, bool clip)
		{
			this.targetGraph = targetGraph;
			this.sourceNode = sourceNode;
			this.isClip = clip;
		}

		protected abstract Node OnDuplicate();

		internal Node Duplicate(Vector2 position)
		{
			destNode = OnDuplicate();

			if (destNode != null)
			{
				destNode.position = sourceNode.position;
				destNode.position.position += position;

				destNode.position.position = EditorGUITools.SnapToGrid(destNode.position.position);

				destNode.name = sourceNode.name;
				destNode.showComment = sourceNode.showComment;
				destNode.nodeComment = sourceNode.nodeComment;
			}

			return destNode;
		}

		protected void RegisterBehaviour(NodeBehaviour source, NodeBehaviour dest)
		{
			_DuplicateBehaviours.Add(source, dest);
		}

		internal NodeBehaviour GetDestBehaviour(NodeBehaviour sourceBehaviour)
		{
			NodeBehaviour destBehaviour = null;
			if (_DuplicateBehaviours.TryGetValue(sourceBehaviour, out destBehaviour))
			{
				return destBehaviour;
			}
			return null;
		}

		protected virtual void OnAfterDuplicate(List<NodeDuplicator> duplicators)
		{
		}

		internal void DoAfterDuplicate(List<NodeDuplicator> duplicators)
		{
			OnAfterDuplicate(duplicators);
		}
	}
}
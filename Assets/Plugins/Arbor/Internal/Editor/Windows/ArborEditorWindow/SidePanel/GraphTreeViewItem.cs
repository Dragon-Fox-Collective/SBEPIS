//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using System.Collections;

namespace ArborEditor
{
	using Arbor;
	using ArborEditor.IMGUI.Controls;

	public class GraphTreeViewItem : TreeViewItem
	{
		public NodeGraph nodeGraph
		{
			get;
			private set;
		}

		public virtual bool isExternal
		{
			get
			{
				return false;
			}
		}

		public override bool renamable
		{
			get
			{
				return (nodeGraph.hideFlags & HideFlags.NotEditable) != HideFlags.NotEditable && !isExternal;
			}
		}

		public GraphTreeViewItem(int id, NodeGraph nodeGraph) : base(id, nodeGraph.graphName, null)
		{
			this.nodeGraph = nodeGraph;
			nodeGraph.onChangedGraphName += OnChangedGraphName;
		}

		public GraphTreeViewItem(NodeGraph nodeGraph) : this(nodeGraph.GetInstanceID(), nodeGraph)
		{
		}

		public override void Dispose()
		{
			base.Dispose();

			if (nodeGraph is object)
			{
				nodeGraph.onChangedGraphName -= OnChangedGraphName;
			}
		}

		void OnChangedGraphName()
		{
			displayName = nodeGraph.graphName;
		}
	}
}
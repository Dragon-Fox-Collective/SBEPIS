//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace ArborEditor
{
	using Arbor;

	[CustomClipboardProcessor(typeof(StateBehaviour))]
	public class StateBehaviourClipboardProcessor : ClipboardProcessor
	{
		internal static void CopyBehaviour(StateBehaviour source, StateBehaviour dest, bool checkLink, List<NodeDuplicator> duplicators)
		{
			if (dest == null)
			{
				return;
			}

			Clipboard.DoCopyNodeBehaviour(source, dest, checkLink);

			if (checkLink)
			{
				Undo.RecordObject(dest, "Copy Behaviour");

				bool isSameNodeGraph = Clipboard.IsSameNodeGraph(source, dest);
				NodeGraph nodeGraph = dest.nodeGraph;

				for (int i = 0, count = dest.stateLinkCount; i < count; ++i)
				{
					StateLink s = dest.GetStateLink(i);
					CheckStateLink(s, isSameNodeGraph, nodeGraph, duplicators);
				}

				EditorUtility.SetDirty(dest);
			}
		}

		internal static void CheckStateLink(StateLink stateLink, bool isSameNodeGraph, NodeGraph nodeGraph, List<NodeDuplicator> duplicators)
		{
			if (duplicators != null)
			{
				for (int duplicatorIndex = 0; duplicatorIndex < duplicators.Count; duplicatorIndex++)
				{
					NodeDuplicator duplicator = duplicators[duplicatorIndex];
					if (duplicator.sourceNode.nodeID == stateLink.stateID)
					{
						stateLink.stateID = duplicator.destNode.nodeID;
						return;
					}
				}
			}

			if (!isSameNodeGraph || nodeGraph.GetNodeFromID(stateLink.stateID) == null)
			{
				stateLink.stateID = 0;
			}
		}

		public override void CopyNodeBehaviour(NodeBehaviour source, NodeBehaviour dest, bool checkLink)
		{
			StateBehaviour sourceBehaviour = source as StateBehaviour;
			StateBehaviour destBehaviour = dest as StateBehaviour;
			if (sourceBehaviour != null && destBehaviour != null)
			{
				CopyBehaviour(sourceBehaviour, destBehaviour, checkLink, null);
				return;
			}
		}

		public override void MoveBehaviour(Node node, NodeBehaviour sourceBehaviour)
		{
			State state = node as State;
			StateBehaviour sourceStateBehaviour = sourceBehaviour as StateBehaviour;
			if (state == null || sourceStateBehaviour == null)
			{
				return;
			}

			bool cachedEnabled = ComponentUtility.useEditorProcessor;
			ComponentUtility.useEditorProcessor = false;

			StateBehaviour destBehaviour = NodeBehaviour.CreateNodeBehaviour(state, sourceBehaviour.GetType(), true) as StateBehaviour;

			if (destBehaviour != null)
			{
				state.AddBehaviour(destBehaviour);
				CopyBehaviour(sourceStateBehaviour, destBehaviour, false, null);
			}

			ComponentUtility.useEditorProcessor = cachedEnabled;
		}
	}
}

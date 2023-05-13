//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArborEditor.BehaviourTree
{
	using Arbor;
	using Arbor.BehaviourTree;

	[CustomClipboardProcessor(typeof(ActionBehaviour))]
	public class ActionBehaviourClipboardProcessor : ClipboardProcessor
	{
		public override void MoveBehaviour(Node node, NodeBehaviour sourceBehaviour)
		{
			ActionNode actionNode = node as ActionNode;
			ActionBehaviour sourceActionBehaviour = sourceBehaviour as ActionBehaviour;
			if( actionNode == null || sourceActionBehaviour == null)
			{
				return;
			}

			bool cachedEnabled = ComponentUtility.useEditorProcessor;
			ComponentUtility.useEditorProcessor = false;

			ActionBehaviour destActionBehaviour = actionNode.CreateActionBehaviour(sourceActionBehaviour.GetType());

			if (destActionBehaviour != null)
			{
				Clipboard.DoCopyNodeBehaviour(sourceActionBehaviour, destActionBehaviour, false);
			}

			ComponentUtility.useEditorProcessor = cachedEnabled;
		}
	}
}
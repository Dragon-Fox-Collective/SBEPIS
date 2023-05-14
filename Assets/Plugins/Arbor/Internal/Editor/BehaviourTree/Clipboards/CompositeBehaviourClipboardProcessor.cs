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

	[CustomClipboardProcessor(typeof(CompositeBehaviour))]
	public class CompositeBehaviourClipboardProcessor : ClipboardProcessor
	{
		public override void MoveBehaviour(Node node, NodeBehaviour sourceBehaviour)
		{
			CompositeNode compositeNode = node as CompositeNode;
			CompositeBehaviour sourceCompositeBehaviour = sourceBehaviour as CompositeBehaviour;
			if (compositeNode == null || sourceCompositeBehaviour == null)
			{
				return;
			}

			bool cachedEnabled = ComponentUtility.useEditorProcessor;
			ComponentUtility.useEditorProcessor = false;

			CompositeBehaviour destCompositeBehaviour = compositeNode.CreateCompositeBehaviour(sourceCompositeBehaviour.GetType());

			if (destCompositeBehaviour != null)
			{
				Clipboard.DoCopyNodeBehaviour(sourceCompositeBehaviour, destCompositeBehaviour, false);
			}

			ComponentUtility.useEditorProcessor = cachedEnabled;
		}
	}
}
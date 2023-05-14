//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using Arbor;
using UnityEngine;

namespace ArborEditor.BehaviourTree
{
	using Arbor.BehaviourTree;

	[CustomClipboardProcessor(typeof(Decorator))]
	internal sealed class DecoratorClipboardProcessor : ClipboardProcessor
	{
		public override void MoveBehaviour(Node node, NodeBehaviour sourceBehaviour)
		{
			TreeBehaviourNode behaviourNode = node as TreeBehaviourNode;
			Decorator sourceDecorator = sourceBehaviour as Decorator;
			if (behaviourNode == null || sourceDecorator == null)
			{
				return;
			}

			bool cachedEnabled = ComponentUtility.useEditorProcessor;
			ComponentUtility.useEditorProcessor = false;

			Decorator destDecorator = NodeBehaviour.CreateNodeBehaviour(behaviourNode, sourceDecorator.GetType(), true) as Decorator;

			if (destDecorator != null)
			{
				behaviourNode.decoratorList.Add(destDecorator);
				Clipboard.DoCopyNodeBehaviour(sourceDecorator, destDecorator, false);
			}

			ComponentUtility.useEditorProcessor = cachedEnabled;
		}
	}
}
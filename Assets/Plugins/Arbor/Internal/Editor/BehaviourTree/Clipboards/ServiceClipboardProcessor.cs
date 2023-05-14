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

	[CustomClipboardProcessor(typeof(Service))]
	public class ServiceClipboardProcessor : ClipboardProcessor
	{
		public override void MoveBehaviour(Node node, NodeBehaviour sourceBehaviour)
		{
			TreeBehaviourNode behaviourNode = node as TreeBehaviourNode;
			Service sourceService = sourceBehaviour as Service;
			if (behaviourNode == null || sourceService == null)
			{
				return;
			}

			bool cachedEnabled = ComponentUtility.useEditorProcessor;
			ComponentUtility.useEditorProcessor = false;

			Service destService = NodeBehaviour.CreateNodeBehaviour(behaviourNode, sourceService.GetType(), true) as Service;

			if (destService != null)
			{
				behaviourNode.serviceList.Add(destService);
				Clipboard.DoCopyNodeBehaviour(sourceService, destService, false);
			}

			ComponentUtility.useEditorProcessor = cachedEnabled;
		}
	}
}
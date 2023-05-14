//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using System.Collections.Generic;

namespace ArborEditor.BehaviourTree
{
	using Arbor;
	using Arbor.BehaviourTree;

	[CustomNodeDuplicator(typeof(TreeBehaviourNode), typeof(BehaviourTreeInternal))]
	internal abstract class TreeBehaviourNodeDuplicator : TreeNodeDuplicator
	{
		protected override void OnAfterDuplicate(List<NodeDuplicator> duplicators)
		{
			TreeBehaviourNode sourceBehaviourNode = sourceNode as TreeBehaviourNode;
			TreeBehaviourNode destBehaviourNode = destNode as TreeBehaviourNode;

			Clipboard.CopyNodeBehaviour(sourceBehaviourNode.behaviour, destBehaviourNode.behaviour, true);
			RegisterBehaviour(sourceBehaviourNode.behaviour, destBehaviourNode.behaviour);

			DecoratorList decoratorList = sourceBehaviourNode.decoratorList;
			int decoratorCount = decoratorList.count;
			for (int decoratorIndex = 0; decoratorIndex < decoratorCount; ++decoratorIndex)
			{
				Decorator sourceDecorator = decoratorList[decoratorIndex];
				if (sourceDecorator != null)
				{
					Decorator decorator = NodeBehaviour.CreateNodeBehaviour(destBehaviourNode, sourceDecorator.GetType(), true) as Decorator;

					if (decorator != null)
					{
						destBehaviourNode.decoratorList.Add(decorator);

						Clipboard.CopyNodeBehaviour(sourceDecorator, decorator, true);
						RegisterBehaviour(sourceDecorator, decorator);
					}
				}
			}

			ServiceList serviceList = sourceBehaviourNode.serviceList;
			int serviceCount = serviceList.count;
			for (int serviceIndex = 0; serviceIndex < serviceCount; ++serviceIndex)
			{
				Service sourceService = serviceList[serviceIndex];
				if (sourceService != null)
				{
					Service service = NodeBehaviour.CreateNodeBehaviour(destBehaviourNode, sourceService.GetType(), true) as Service;

					if (service != null)
					{
						destBehaviourNode.serviceList.Add(service);

						Clipboard.CopyNodeBehaviour(sourceService, service, true);
						RegisterBehaviour(sourceService, service);
					}
				}
			}

			ReconnectBranch(duplicators);
		}
	}
}
//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace ArborEditor.BehaviourTree
{
	using Arbor;
	using Arbor.BehaviourTree;

	[CustomNodeDuplicator(typeof(CompositeNode),typeof(BehaviourTreeInternal))]
	internal sealed class CompositeNodeDuplicator : TreeBehaviourNodeDuplicator
	{
		protected override Node OnDuplicate()
		{
			CompositeNode sourceCompositeNode = sourceNode as CompositeNode;

			TreeNodeBehaviour mainBehaviour = sourceCompositeNode.behaviour;
			if (mainBehaviour == null)
			{
				return null;
			}

			CompositeNode destCompositeNode = null;
			if (isClip)
			{
				destCompositeNode = targetBehaviourTree.CreateComposite(Vector2.zero, sourceCompositeNode.nodeID, mainBehaviour.GetType());
			}
			else
			{
				destCompositeNode = targetBehaviourTree.CreateComposite(Vector2.zero, mainBehaviour.GetType());
			}

			return destCompositeNode;
		}
	}
}
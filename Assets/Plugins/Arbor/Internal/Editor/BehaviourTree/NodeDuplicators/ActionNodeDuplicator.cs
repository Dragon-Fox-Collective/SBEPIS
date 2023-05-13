//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace ArborEditor.BehaviourTree
{
	using Arbor;
	using Arbor.BehaviourTree;

	[CustomNodeDuplicator(typeof(ActionNode),typeof(BehaviourTreeInternal))]
	internal sealed class ActionNodeDuplicator : TreeBehaviourNodeDuplicator
	{
		protected override Node OnDuplicate()
		{
			ActionNode sourceActionNode = sourceNode as ActionNode;

			TreeNodeBehaviour mainBehaviour = sourceActionNode.behaviour;
			if (mainBehaviour == null)
			{
				return null;
			}

			ActionNode destActionNode = null;
			if (isClip)
			{
				destActionNode = targetBehaviourTree.CreateAction(Vector2.zero, sourceActionNode.nodeID, mainBehaviour.GetType());
			}
			else
			{
				destActionNode = targetBehaviourTree.CreateAction(Vector2.zero, mainBehaviour.GetType());
			}

			return destActionNode;
		}
	}
}
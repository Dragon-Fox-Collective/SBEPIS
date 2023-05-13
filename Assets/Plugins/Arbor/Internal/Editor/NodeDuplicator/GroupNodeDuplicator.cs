//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using Arbor;
namespace ArborEditor
{
	[CustomNodeDuplicator(typeof(GroupNode))]
	internal sealed class GroupNodeDuplicator : NodeDuplicator
	{
		protected override Node OnDuplicate()
		{
			GroupNode sourceGroup = sourceNode as GroupNode;

			GroupNode group = null;
			if (isClip)
			{
				group = targetGraph.CreateGroup(sourceGroup.nodeID);
			}
			else
			{
				group = targetGraph.CreateGroup();
			}

			if (group != null)
			{
				group.color = sourceGroup.color;
				group.autoAlignment = sourceGroup.autoAlignment;
			}

			return group;
		}
	}
}
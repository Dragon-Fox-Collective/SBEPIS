//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace ArborEditor
{
	using Arbor;
	[CustomNodeDuplicator(typeof(DataBranchRerouteNode))]
	internal sealed class DataBranchRerouteNodeDuplicator : NodeDuplicator
	{
		protected override Node OnDuplicate()
		{
			DataBranchRerouteNode sourceRerouteNode = sourceNode as DataBranchRerouteNode;
			DataBranchRerouteNode rerouteNode = null;
			if (isClip)
			{
				rerouteNode = targetGraph.CreateDataBranchRerouteNode(Vector2.zero, sourceRerouteNode.link.dataType, sourceRerouteNode.nodeID);
			}
			else
			{
				rerouteNode = targetGraph.CreateDataBranchRerouteNode(Vector2.zero, sourceRerouteNode.link.dataType);
			}

			if (rerouteNode != null)
			{
				rerouteNode.link.Copy(sourceRerouteNode.link);
				rerouteNode.direction = sourceRerouteNode.direction;
			}

			return rerouteNode;
		}
	}
}
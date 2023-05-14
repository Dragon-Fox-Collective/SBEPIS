//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using Arbor;

namespace ArborEditor
{
	[CustomNodeDuplicator(typeof(CommentNode))]
	internal sealed class CommentNodeDuplicator : NodeDuplicator
	{
		protected override Node OnDuplicate()
		{
			CommentNode sourceCommentNode = sourceNode as CommentNode;
			CommentNode comment = null;

			if (isClip)
			{
				comment = targetGraph.CreateComment(sourceCommentNode.nodeID);
			}
			else
			{
				comment = targetGraph.CreateComment();
			}

			if (comment != null)
			{
				comment.comment = sourceCommentNode.comment;
			}

			return comment;
		}
	}
}
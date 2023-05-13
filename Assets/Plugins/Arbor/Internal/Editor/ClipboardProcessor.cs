//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArborEditor
{
	using Arbor;

	public abstract class ClipboardProcessor
	{
		public virtual void CopyNodeBehaviour(NodeBehaviour source, NodeBehaviour dest, bool checkLink)
		{
			Clipboard.DoCopyNodeBehaviour(source, dest, checkLink);
		}

		public abstract void MoveBehaviour(Node node, NodeBehaviour sourceBehaviour);
	}
}
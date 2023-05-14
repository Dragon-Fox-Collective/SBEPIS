//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.BehaviourTree.Composites
{
#if ARBOR_DOC_JA
	/// <summary>
	/// 子ノードのうち一つをランダムに実行し、実行結果をそのまま返す。
	/// </summary>
#else
	/// <summary>
	/// Execute one of the child nodes randomly and return the execution result as it is.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("RandomExecutor")]
	[BuiltInBehaviour]
	public sealed class RandomExecutor : CompositeBehaviour
	{
		public override int GetBeginIndex()
		{
			var childrenLinkSlot = compositeNode.GetChildrenLinkSlot();
			return Random.Range(0, childrenLinkSlot.branchIDs.Count);
		}

		public override bool CanExecute(NodeStatus childStatus)
		{
			return childStatus == NodeStatus.Running;
		}
	}
}

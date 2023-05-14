//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.BehaviourTree.Composites
{
#if ARBOR_DOC_JA
	/// <summary>
	/// 子ノードを左から順に実行し、成功した子ノードが見つかったら成功を返し終了する。
	/// 子ノードがすべて失敗した場合は失敗を返す。
	/// </summary>
#else
	/// <summary>
	/// Execute child nodes in order from the left, and return success if successful child nodes are found and end.
	/// If all the child nodes fail, it returns failure.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Selector")]
	[BuiltInBehaviour]
	public sealed class Selector : CompositeBehaviour
	{
		public override bool CanExecute(NodeStatus childStatus)
		{
			return childStatus != NodeStatus.Success;
		}
	}
}
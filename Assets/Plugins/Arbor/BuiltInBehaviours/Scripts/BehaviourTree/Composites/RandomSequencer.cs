//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.BehaviourTree.Composites
{
#if ARBOR_DOC_JA
	/// <summary>
	/// 子ノードをランダムな順に実行し、失敗した子ノードが見つかったら失敗を返し終了する。
	/// 子ノードがすべて成功した場合は成功を返す。
	/// </summary>
#else
	/// <summary>
	/// Execute child nodes in random order, return failed and finish when a failed child node is found.
	/// If all the child nodes succeed, it returns success.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("RandomSequencer")]
	[BuiltInBehaviour]
	public sealed class RandomSequencer : RandomOrderBase
	{
		public override bool CanExecute(NodeStatus childStatus)
		{
			return childStatus != NodeStatus.Failure;
		}
	}
}

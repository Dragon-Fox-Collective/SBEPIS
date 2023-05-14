//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.BehaviourTree.Composites
{
#if ARBOR_DOC_JA
	/// <summary>
	/// 子ノードをランダムな順に実行し、成功した子ノードが見つかったら成功を返し終了する。
	/// 子ノードがすべて失敗した場合は失敗を返す。
	/// </summary>
#else
	/// <summary>
	/// Execute the child nodes in random order, return success if successful child nodes are found and end.
	/// If all the child nodes fail, it returns failure.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("RandomSelector")]
	[BuiltInBehaviour]
	public sealed class RandomSelector : RandomOrderBase
	{
		public override bool CanExecute(NodeStatus childStatus)
		{
			return childStatus != NodeStatus.Success;
		}
	}
}

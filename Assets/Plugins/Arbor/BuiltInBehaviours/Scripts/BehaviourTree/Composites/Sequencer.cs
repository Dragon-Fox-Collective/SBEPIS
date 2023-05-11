//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.BehaviourTree.Composites
{
#if ARBOR_DOC_JA
	/// <summary>
	/// 子ノードを左から順に実行し、失敗した子ノードが見つかったら失敗を返し終了する。
	/// 子ノードがすべて成功したら成功を返す。
	/// </summary>
#else
	/// <summary>
	/// Execute the child nodes in order from the left, fail if the failed child nodes are found, and exit.
	/// Return success if all child nodes succeed.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Sequencer")]
	[BuiltInBehaviour]
	public sealed class Sequencer : CompositeBehaviour
	{
		public override bool CanExecute(NodeStatus childStatus)
		{
			return childStatus != NodeStatus.Failure;
		}
	}
}
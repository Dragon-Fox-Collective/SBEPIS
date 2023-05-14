//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.BehaviourTree.Decorators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// ノードの実行結果を反転する。
	/// </summary>
#else
	/// <summary>
	/// Invert the execution result of the node.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("InvertResult")]
	[BuiltInBehaviour]
	public sealed class InvertResult : Decorator
	{
		public override bool HasConditionCheck()
		{
			return false;
		}

		protected override bool OnFinishExecute(bool result)
		{
			return !result;
		}
	}
}
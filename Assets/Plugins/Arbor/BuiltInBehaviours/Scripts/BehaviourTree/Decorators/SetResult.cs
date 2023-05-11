//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.BehaviourTree.Decorators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// ノードの実行結果を強制的に設定する。
	/// </summary>
#else
	/// <summary>
	/// Forcibly set the execution result of the node.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("SetResult")]
	[BuiltInBehaviour]
	public sealed class SetResult : Decorator
	{
#if ARBOR_DOC_JA
		/// <summary>
		/// 設定する値
		/// </summary>
#else
		/// <summary>
		/// Value to set
		/// </summary>
#endif
		[SerializeField]
		private FlexibleBool _Result = new FlexibleBool();

		public override bool HasConditionCheck()
		{
			return false;
		}

		protected override bool OnFinishExecute(bool result)
		{
			return _Result.value;
		}
	}
}
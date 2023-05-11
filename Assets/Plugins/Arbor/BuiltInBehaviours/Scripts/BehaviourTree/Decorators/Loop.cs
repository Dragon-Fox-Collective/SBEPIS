//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.BehaviourTree.Decorators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// ノードを繰り返し実行する。
	/// </summary>
#else
	/// <summary>
	/// Repeat the node.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Loop")]
	[BuiltInBehaviour]
	public sealed class Loop : Decorator
	{
#if ARBOR_DOC_JA
		/// <summary>
		/// 無限ループ。
		/// </summary>
#else
		/// <summary>
		/// Infinite loop.
		/// </summary>
#endif
		[SerializeField]
		private FlexibleBool _InfiniteLoop = new FlexibleBool(true);

#if ARBOR_DOC_JA
		/// <summary>
		/// 繰り返す回数(Infinite Loopがfalseの場合のみ)
		/// </summary>
#else
		/// <summary>
		/// Number of times to repeat (only when Infinite Loop is false)
		/// </summary>
#endif
		[SerializeField]
		private FlexibleInt _LoopCount = new FlexibleInt();

		private int _Count;

		protected override void OnStart()
		{
			_Count = 0;
		}

		public override bool HasConditionCheck()
		{
			return false;
		}

		protected override bool OnRepeatCheck()
		{
			_Count++;
			return _InfiniteLoop.value || _Count < _LoopCount.value;
		}
	}
}
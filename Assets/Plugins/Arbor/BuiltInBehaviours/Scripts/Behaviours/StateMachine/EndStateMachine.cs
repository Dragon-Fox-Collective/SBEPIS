//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.StateMachine.StateBehaviours
{
#if ARBOR_DOC_JA
	/// <summary>
	/// 現在ステートマシンを完了し親グラフに戻る。
	/// </summary>
#else
	/// <summary>
	/// End the state machine now and return to the parent graph.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("StateMachine/EndStateMachine")]
	[BuiltInBehaviour]
	public sealed class EndStateMachine : StateBehaviour
	{
#if ARBOR_DOC_JA
		/// <summary>
		/// 成功かどうか
		/// </summary>
#else
		/// <summary>
		/// Whether success
		/// </summary>
#endif
		[SerializeField]
		private FlexibleBool _Success = new FlexibleBool();

		// Use this for enter state
		public override void OnStateBegin()
		{
			INodeGraphContainer graphContianer = nodeGraph.ownerBehaviour as INodeGraphContainer;
			if (graphContianer != null)
			{
				graphContianer.OnFinishNodeGraph(nodeGraph, _Success.value);
			}
			else
			{
				stateMachine.Stop();
			}
		}
	}
}
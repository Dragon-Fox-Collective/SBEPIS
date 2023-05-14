//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.StateMachine.StateBehaviours
{
#if ARBOR_DOC_JA
	/// <summary>
	/// グラフ停止時に遷移する。終了処理を行いたい場合に常駐ステートに追加して遷移先で終了処理をしてください。
	/// </summary>
#else
	/// <summary>
	/// Transition when graph stops. If you want to perform termination processing, add it to the resident state and perform termination processing at the transition destination.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Transition/OnGraphStopTransition")]
	[BuiltInBehaviour]
	public sealed class OnGraphStopTransition : StateBehaviour
	{
#if ARBOR_DOC_JA
		/// <summary>
		/// 遷移先ステートへのリンク
		/// </summary>
#else
		/// <summary>
		/// Link to destination state
		/// </summary>
#endif
		[SerializeField]
		[FixedTransitionTiming(TransitionTiming.Immediate)]
		private StateLink _NextState = new StateLink();

		protected override void OnGraphStop()
		{
			Transition(_NextState, TransitionTiming.Immediate);
		}
	}
}
//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.StateMachine.StateBehaviours
{
#if ARBOR_DOC_JA
	/// <summary>
	/// NodeGraphを停止する。
	/// </summary>
#else
	/// <summary>
	/// Stop NodeGraph.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("NodeGraph/StopNodeGraph")]
	[BuiltInBehaviour]
	public sealed class StopNodeGraph : StateBehaviour
	{
#if ARBOR_DOC_JA
		/// <summary>
		/// 停止するNodeGraph
		/// </summary>
#else
		/// <summary>
		/// NodeGraph to stop playback
		/// </summary>
#endif
		[SlotType(typeof(NodeGraph))]
		[SerializeField]
		private FlexibleComponent _TargetGraph = new FlexibleComponent();

		// Use this for enter state
		public override void OnStateBegin()
		{
			NodeGraph nodeGraph = _TargetGraph.value as NodeGraph;
			if (nodeGraph != null)
			{
				nodeGraph.Stop();
			}
		}
	}
}
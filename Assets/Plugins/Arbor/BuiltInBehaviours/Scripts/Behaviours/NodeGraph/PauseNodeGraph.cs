//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.StateMachine.StateBehaviours
{
#if ARBOR_DOC_JA
	/// <summary>
	/// NodeGraphをポーズする。
	/// </summary>
#else
	/// <summary>
	/// Pause NodeGraph.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("NodeGraph/PauseNodeGraph")]
	[BuiltInBehaviour]
	public sealed class PauseNodeGraph : StateBehaviour
	{
#if ARBOR_DOC_JA
		/// <summary>
		/// ポーズするNodeGraph
		/// </summary>
#else
		/// <summary>
		/// Pause NodeGraph
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
				nodeGraph.Pause();
			}
		}
	}
}
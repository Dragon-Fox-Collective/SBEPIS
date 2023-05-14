//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.BehaviourTree.Actions
{
#if ARBOR_DOC_JA
	/// <summary>
	/// NodeGraphをポーズする。
	/// </summary>
#else
	/// <summary>
	/// Pause NodeGraph
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("NodeGraph/PauseNodeGraph")]
	[BuiltInBehaviour]
	public sealed class PauseNodeGraph : ActionBehaviour
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

		protected override void OnExecute()
		{
			NodeGraph nodeGraph = _TargetGraph.value as NodeGraph;
			if (nodeGraph != null)
			{
				nodeGraph.Pause();
				FinishExecute(true);
			}
			else
			{
				FinishExecute(false);
			}
		}
	}
}
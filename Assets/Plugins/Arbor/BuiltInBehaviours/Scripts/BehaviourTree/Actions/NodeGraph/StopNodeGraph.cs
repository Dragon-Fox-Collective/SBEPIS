//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.BehaviourTree.Actions
{
#if ARBOR_DOC_JA
	/// <summary>
	/// NodeGraphを停止する。
	/// </summary>
#else
	/// <summary>
	/// Stop NodeGraph
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("NodeGraph/StopNodeGraph")]
	[BuiltInBehaviour]
	public sealed class StopNodeGraph : ActionBehaviour
	{
#if ARBOR_DOC_JA
		/// <summary>
		/// 再生停止するNodeGraph
		/// </summary>
#else
		/// <summary>
		/// NodeGraph to stop playback
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
				nodeGraph.Stop();
				FinishExecute(true);
			}
			else
			{
				FinishExecute(false);
			}
		}
	}
}
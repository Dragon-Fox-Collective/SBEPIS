//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.BehaviourTree.Actions
{
#if ARBOR_DOC_JA
	/// <summary>
	/// NodeGraphを再生する。
	/// </summary>
#else
	/// <summary>
	/// Play NodeGraph
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("NodeGraph/PlayNodeGraph")]
	[BuiltInBehaviour]
	public sealed class PlayNodeGraph : ActionBehaviour
	{
#if ARBOR_DOC_JA
		/// <summary>
		/// 再生開始するNodeGraph
		/// </summary>
#else
		/// <summary>
		/// Start playback NodeGraph
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
				nodeGraph.Play();
				FinishExecute(true);
			}
			else
			{
				FinishExecute(false);
			}
		}
	}
}
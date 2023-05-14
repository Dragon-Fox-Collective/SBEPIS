//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.BehaviourTree.Actions
{
#if ARBOR_DOC_JA
	/// <summary>
	/// NodeGraphを再開する。
	/// </summary>
#else
	/// <summary>
	/// Resume NodeGraph
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("NodeGraph/ResumeNodeGraph")]
	[BuiltInBehaviour]
	public sealed class ResumeNodeGraph : ActionBehaviour
	{
#if ARBOR_DOC_JA
		/// <summary>
		/// 再開するNodeGraph
		/// </summary>
#else
		/// <summary>
		/// Resume NodeGraph
		/// </summary>
#endif
		[SlotType(typeof(NodeGraph))]
		[SerializeField]
		private FlexibleComponent TargetGraph = new FlexibleComponent();

		protected override void OnExecute()
		{
			NodeGraph nodeGraph = TargetGraph.value as NodeGraph;
			if (nodeGraph != null)
			{
				nodeGraph.Resume();
				FinishExecute(true);
			}
			else
			{
				FinishExecute(false);
			}
		}
	}
}
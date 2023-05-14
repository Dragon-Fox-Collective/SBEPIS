//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.StateMachine.StateBehaviours
{
#if ARBOR_DOC_JA
	/// <summary>
	/// NodeGraphを再開する。
	/// </summary>
#else
	/// <summary>
	/// Resume NodeGraph.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("NodeGraph/ResumeNodeGraph")]
	[BuiltInBehaviour]
	public sealed class ResumeNodeGraph : StateBehaviour
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
		private FlexibleComponent _TargetGraph = new FlexibleComponent();

		// Use this for enter state
		public override void OnStateBegin()
		{
			NodeGraph nodeGraph = _TargetGraph.value as NodeGraph;
			if (nodeGraph != null)
			{
				nodeGraph.Resume();
			}
		}
	}
}
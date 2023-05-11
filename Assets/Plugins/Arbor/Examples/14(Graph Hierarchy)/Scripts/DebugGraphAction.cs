//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
#if ARBOR_SUPPORT_UGUI
using UnityEngine;

namespace Arbor.Examples
{
	using Arbor.BehaviourTree;

	/// <summary>
	/// An action that displays the active node information.
	/// </summary>
	[AddComponentMenu("")]
	[AddBehaviourMenu("Examples/DebugGraphAction")]
	public sealed class DebugGraphAction : ActionBehaviour
	{
		/// <summary>
		/// DebugGraph
		/// </summary>
		private DebugGraph _DebugGraph = null;

		/// <summary>
		/// Called only once when a node becomes active.
		/// </summary>
		protected override void OnAwake()
		{
			_DebugGraph = new DebugGraph(nodeGraph);
		}

		/// <summary>
		/// Called when entering a node.
		/// </summary>
		protected override void OnStart()
		{
			// Initialize
			_DebugGraph.InitializeEvent();

			// Output node to log
			_DebugGraph.Log(node.ToString());
		}

		/// <summary>
		/// Called when executing a node.
		/// </summary>
		protected override void OnExecute()
		{
			if (_DebugGraph.isNextClick)
			{
				// Return success immediately.
				FinishExecute(true);
			}
		}

		/// <summary>
		/// Called when leaving a node.
		/// </summary>
		protected override void OnEnd()
		{
			if (_DebugGraph != null)
			{
				_DebugGraph.ReleaseEvent();
			}
		}
	}
}
#endif
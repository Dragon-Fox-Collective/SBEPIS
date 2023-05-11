//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// NodeGraphの名前を出力する。
	/// </summary>
#else
	/// <summary>
	/// Output the node graph name.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("NodeGraph/NodeGraph.GetName")]
	[BehaviourTitle("NodeGraph.GetName")]
	[BuiltInBehaviour]
	public sealed class NodeGraphGetNameCalculator : Calculator
	{
		#region Serialize fields

		/// <summary>
		/// NodeGraph
		/// </summary>
		[SerializeField]
		[SlotType(typeof(NodeGraph))]
		private FlexibleComponent _Graph = new FlexibleComponent();

#if ARBOR_DOC_JA
		/// <summary>
		/// 名前の出力スロット
		/// </summary>
#else
		/// <summary>
		/// Name output slot
		/// </summary>
#endif
		[SerializeField]
		private OutputSlotString _Output = new OutputSlotString();

		#endregion // Serialize fields

		public override bool OnCheckDirty()
		{
			return true;
		}

		// Use this for calculate
		public override void OnCalculate()
		{
			NodeGraph nodeGraph = _Graph.value as NodeGraph;

			if (nodeGraph != null)
			{
				_Output.SetValue(nodeGraph.graphName);
			}
		}
	}
}
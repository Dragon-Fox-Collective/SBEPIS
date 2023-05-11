//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// ルートグラフを出力する。
	/// </summary>
#else
	/// <summary>
	/// Output the root graph.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("NodeGraph/NodeGraph.GetRootGraph")]
	[BehaviourTitle("GetRootGraph")]
	[BuiltInBehaviour]
	public sealed class NodeGraphGetRootNodeGraphCalculator : Calculator
	{
		#region Serialize fields

#if ARBOR_DOC_JA
		/// <summary>
		/// ルートグラフの出力スロット
		/// </summary>
#else
		/// <summary>
		/// Root graph output slot
		/// </summary>
#endif
		[SerializeField]
		[SlotType(typeof(NodeGraph))]
		private OutputSlotAny _Output = new OutputSlotAny();

		#endregion // Serialize fields

		// Use this for calculate
		public override void OnCalculate()
		{
			_Output.SetValue(nodeGraph.rootGraph);
		}
	}
}
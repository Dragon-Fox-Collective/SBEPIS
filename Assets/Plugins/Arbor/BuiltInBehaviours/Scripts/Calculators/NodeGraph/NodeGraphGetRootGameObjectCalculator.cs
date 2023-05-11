//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// ルートグラフのGameObjectを出力する。
	/// </summary>
#else
	/// <summary>
	/// Output the root graph GameObject.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("NodeGraph/NodeGraph.GetRootGameObject")]
	[BehaviourTitle("GetRootGameObject")]
	[BuiltInBehaviour]
	public sealed class NodeGraphGetRootGameObjectCalculator : Calculator
	{
		#region Serialize fields

#if ARBOR_DOC_JA
		/// <summary>
		/// ルートのGameObjectの出力スロット
		/// </summary>
#else
		/// <summary>
		/// Root GameObject output slot
		/// </summary>
#endif
		[SerializeField]
		private OutputSlotGameObject _Output = new OutputSlotGameObject();

		#endregion // Serialize fields

		// Use this for calculate
		public override void OnCalculate()
		{
			_Output.SetValue(nodeGraph.rootGraph.gameObject);
		}
	}
}
//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Examples
{
	/// <summary>
	/// Example of calculator using data flow
	/// </summary>
	[AddComponentMenu("")]
	[AddBehaviourMenu("Examples/DataFlowExampleNewDataCalculator")]
	public sealed class DataFlowExampleNewDataCalculator : Calculator
	{
		/// <summary>
		/// string field by FlexibleString
		/// </summary>
		[SerializeField]
		private FlexibleString _StringValue = new FlexibleString();

		/// <summary>
		/// string field by FlexibleInt
		/// </summary>
		[SerializeField]
		private FlexibleInt _IntValue = new FlexibleInt();

		/// <summary>
		/// Output DataFlowExampleData
		/// </summary>
		[SerializeField]
		private OutputSlotDataFlowExampleData _Output = new OutputSlotDataFlowExampleData();

		/// <summary>
		/// Called when calculating an calculator node
		/// </summary>
		public override void OnCalculate()
		{
			// new DataLinkExampleData
			DataFlowExampleData exampleData = new DataFlowExampleData();

			// set values of DataLinkExampleData
			exampleData.stringValue = _StringValue.value;
			exampleData.intValue = _IntValue.value;

			// output data
			_Output.SetValue(exampleData);
		}
	}
}

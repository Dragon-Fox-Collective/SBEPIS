//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------

namespace Arbor.Examples
{
	/// <summary>
	/// Example of structure to use for data flow
	/// </summary>
	[System.Serializable]
	[RenamedFrom("Arbor.Example.DataFlowExampleData")]
	public struct DataFlowExampleData
	{
		/// <summary>
		/// string value
		/// </summary>
		public string stringValue;

		/// <summary>
		/// int value
		/// </summary>
		public int intValue;

		/// <summary>
		/// values to string
		/// </summary>
		/// <returns>string</returns>
		public override string ToString()
		{
			return stringValue + " : " + intValue;
		}
	}

	/// <summary>
	/// Input slot of DataFlowExampleData
	/// </summary>
	[System.Serializable]
	public sealed class InputSlotDataFlowExampleData : InputSlot<DataFlowExampleData>
	{
	}

	/// <summary>
	/// Output slot of DataFlowExampleData
	/// </summary>
	[System.Serializable]
	public sealed class OutputSlotDataFlowExampleData : OutputSlot<DataFlowExampleData>
	{
	}
}
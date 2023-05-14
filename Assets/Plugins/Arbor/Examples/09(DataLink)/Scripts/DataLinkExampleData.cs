//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------

namespace Arbor.Examples
{
	/// <summary>
	/// Examples of structure for use in DataLink attribute
	/// Note: It is a specification that DataLink can not be used for fields other than UnityObject.
	/// </summary>
	[System.Serializable]
	[RenamedFrom("Arbor.Example.DataLinkExampleData")]
	public struct DataLinkExampleData
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
	/// Output slot of DataLinkExampleData
	/// </summary>
	[System.Serializable]
	public sealed class OutputSlotDataLinkExampleData : OutputSlot<DataLinkExampleData>
	{
	}
}
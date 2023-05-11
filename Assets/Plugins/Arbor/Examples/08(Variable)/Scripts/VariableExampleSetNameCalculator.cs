//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Examples
{
	/// <summary>
	/// Set name of VariableExampleData.
	/// </summary>
	[AddComponentMenu("")]
	[AddBehaviourMenu("Examples/VariableExampleSetNameCalculator")]
	public sealed class VariableExampleSetNameCalculator : Calculator
	{
		/// <summary>
		/// Input ExampleData
		/// </summary>
		[SerializeField]
		private InputSlotVariableExampleData _Input = new InputSlotVariableExampleData();

		/// <summary>
		/// New name
		/// </summary>
		[SerializeField]
		private FlexibleString _Name = new FlexibleString();

		/// <summary>
		/// Output slot
		/// </summary>
		[SerializeField]
		private OutputSlotVariableExampleData _Output = new OutputSlotVariableExampleData();

		/// <summary>
		/// Called when calculating an calculator node
		/// </summary>
		public override void OnCalculate()
		{
			// Input value
			VariableExampleData exampleData = null;
			if (_Input.GetValue(ref exampleData) && exampleData != null)
			{
				// Duplicate VariableExampleData so as not to destroy the original data.
				VariableExampleData newExampleData = new VariableExampleData();
				newExampleData.icon = exampleData.icon;
				newExampleData.name = exampleData.name;

				// Set name
				newExampleData.name = _Name.value;

				// Output value
				_Output.SetValue(newExampleData);
			}
		}
	}
}
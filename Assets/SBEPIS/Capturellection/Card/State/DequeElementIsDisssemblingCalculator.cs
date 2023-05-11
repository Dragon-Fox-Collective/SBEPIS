using Arbor;
using UnityEngine;

namespace SBEPIS.Capturellection.CardState
{
	[AddBehaviourMenu("DequeElement/DequeElement.IsDisassembling")]
	[BehaviourTitle("DequeElement.IsDisassembling")]
	public class DequeElementIsDisassemblingCalculator : Calculator
	{
		[SerializeField] private FlexibleDequeElement dequeElement = new();
		[SerializeField] private OutputSlotBool isDisassembling = new();
		
		public override void OnCalculate() => isDisassembling.SetValue(dequeElement.value.IsDisassembling);
	}
}

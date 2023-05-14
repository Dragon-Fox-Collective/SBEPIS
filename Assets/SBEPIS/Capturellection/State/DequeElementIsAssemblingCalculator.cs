using Arbor;
using UnityEngine;

namespace SBEPIS.Capturellection.State
{
	[AddBehaviourMenu("DequeElement/DequeElement.IsAssembling")]
	[BehaviourTitle("DequeElement.IsAssembling")]
	public class DequeElementIsAssemblingCalculator : Calculator
	{
		[SerializeField] private FlexibleDequeElement dequeElement = new();
		[SerializeField] private OutputSlotBool isAssembling = new();
		
		public override void OnCalculate() => isAssembling.SetValue(dequeElement.value.IsAssembling);
	}
}

using Arbor;
using UnityEngine;

namespace SBEPIS.Capturellection.CardState
{
	[AddBehaviourMenu("DequeElement/DequeElement.IsInLayoutArea")]
	[BehaviourTitle("DequeElement.IsInLayoutArea")]
	public class DequeElementIsInLayoutAreaCalculator : Calculator
	{
		[SerializeField] private FlexibleDequeElement dequeElement = new();
		[SerializeField] private OutputSlotBool isInLayoutArea = new();
		
		public override void OnCalculate() => isInLayoutArea.SetValue(dequeElement.value.IsInLayoutArea);
	}
}

using Arbor;
using UnityEngine;

namespace SBEPIS.Capturellection.State
{
	[AddBehaviourMenu("DequeElement/DequeElement.IsStored")]
	[BehaviourTitle("DequeElement.IsStored")]
	public class DequeElementIsStoredCalculator : Calculator
	{
		[SerializeField] private FlexibleDequeElement dequeElement = new();
		[SerializeField] private OutputSlotBool isStored = new();
		
		public override void OnCalculate() => isStored.SetValue(dequeElement.value.IsStored);
	}
}

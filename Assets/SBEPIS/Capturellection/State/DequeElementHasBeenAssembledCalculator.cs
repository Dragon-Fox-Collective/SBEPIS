using Arbor;
using UnityEngine;

namespace SBEPIS.Capturellection.State
{
	[AddBehaviourMenu("DequeElement/DequeElement.HasBeenAssembled")]
	[BehaviourTitle("DequeElement.HasBeenAssembled")]
	public class DequeElementHasBeenAssembledCalculator : Calculator
	{
		[SerializeField] private FlexibleDequeElement dequeElement = new();
		[SerializeField] private OutputSlotBool hasBeenAssembled = new();
		
		public override void OnCalculate() => hasBeenAssembled.SetValue(dequeElement.value.HasBeenAssembled);
	}
}

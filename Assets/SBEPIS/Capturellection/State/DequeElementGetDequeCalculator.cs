using Arbor;
using UnityEngine;

namespace SBEPIS.Capturellection.State
{
	[AddBehaviourMenu("DequeElement/DequeElement.GetDeque")]
	[BehaviourTitle("DequeElement.GetDeque")]
	public class DequeElementGetDequeCalculator : Calculator
	{
		[SerializeField] private FlexibleDequeElement dequeElement = new();
		[SerializeField] private OutputSlotDeque deque = new();
		
		public override void OnCalculate() => deque.SetValue(dequeElement.value.Deque);
	}
}

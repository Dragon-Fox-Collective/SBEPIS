using Arbor;
using UnityEngine;

namespace SBEPIS.Capturellection.State
{
	[AddBehaviourMenu("DequeElement/DequeElement.IsPageOpen")]
	[BehaviourTitle("DequeElement.IsPageOpen")]
	public class DequeElementIsPageOpenCalculator : Calculator
	{
		[SerializeField] private FlexibleDequeElement dequeElement = new();
		[SerializeField] private OutputSlotBool isPageOpen = new();
		
		public override void OnCalculate() => isPageOpen.SetValue(dequeElement.value.Page.IsOpen);
	}
}

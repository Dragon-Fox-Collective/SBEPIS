using Arbor;
using UnityEngine;

namespace SBEPIS.Capturellection.State
{
	[AddBehaviourMenu("DequeElement/DequeElement.GetDiajector")]
	[BehaviourTitle("DequeElement.GetDiajector")]
	public class DequeElementGetDiajectorCalculator : Calculator
	{
		[SerializeField] private FlexibleDequeElement dequeElement = new();
		[SerializeField] private OutputSlotDiajector diajector = new();
		
		public override void OnCalculate() => diajector.SetValue(dequeElement.value.Diajector);
	}
}

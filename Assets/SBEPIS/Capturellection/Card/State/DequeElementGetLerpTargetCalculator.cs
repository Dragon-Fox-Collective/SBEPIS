using Arbor;
using UnityEngine;
using UnityEngine.Serialization;

namespace SBEPIS.Capturellection.CardState
{
	[AddBehaviourMenu("DequeElement/DequeElement.GetLerpTarget")]
	[BehaviourTitle("DequeElement.GetLerpTarget")]
	public class DequeElementGetLerpTargetCalculator : Calculator
	{
		[SerializeField] private FlexibleDequeElement dequeElement = new();
		[SerializeField] private OutputSlotLerpTarget lerpTarget = new();
		
		public override void OnCalculate() => lerpTarget.SetValue(dequeElement.value.LerpTarget);
	}
}

using Arbor;
using UnityEngine;

namespace SBEPIS.Capturellection.State
{
	[AddBehaviourMenu("DequeElement/SetTargetIndex")]
	[BehaviourTitle("SetTargetIndex")]
	public class DequeElementSetTargetIndex : StateBehaviour
	{
		[SerializeField] private FlexibleDequeElement dequeElement = new();
		[Tooltip("Negative values count from the end of the list")]
		[SerializeField] private FlexibleInt index = new();
		[SerializeField] private IntParameterReference currentTargetIndex = new();
		
		public override void OnStateBegin()
		{
			int finalTargetIndex = index.value >= 0 ? index.value : dequeElement.value.Page.Diajector.LerpTargetCount + index.value - 1;
			if (finalTargetIndex > currentTargetIndex.value)
				CountUp(dequeElement.value, finalTargetIndex);
			else if (finalTargetIndex < currentTargetIndex.value)
				CountDown(dequeElement.value, finalTargetIndex);
		}
		
		private void CountUp(DequeElement dequeElement, int finalTargetIndex)
		{
			for (; currentTargetIndex.value < finalTargetIndex; currentTargetIndex.value++)
				dequeElement.TeleportToLerpTarget(currentTargetIndex.value + 1);
			dequeElement.TeleportToLerpTarget(currentTargetIndex.value + 1);
		}
		
		private void CountDown(DequeElement dequeElement, int finalTargetIndex)
		{
			for (; currentTargetIndex.value > finalTargetIndex; currentTargetIndex.value--)
				dequeElement.TeleportToLerpTarget(currentTargetIndex.value);
			dequeElement.TeleportToLerpTarget(currentTargetIndex.value);
		}
	}
}

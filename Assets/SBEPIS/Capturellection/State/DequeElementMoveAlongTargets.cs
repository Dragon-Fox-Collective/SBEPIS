using Arbor;
using SBEPIS.Utils;
using UnityEngine;

namespace SBEPIS.Capturellection.State
{
	[AddBehaviourMenu("DequeElement/MoveAlongTargets")]
	[BehaviourTitle("MoveAlongTargets")]
	public class DequeElementMoveAlongTargets : StateBehaviour
	{
		[SerializeField] private FlexibleDequeElement dequeElement;
		[SerializeField] private IntParameterReference targetIndex;
		[SerializeField] private FlexibleBool reversed;
		
		public override void OnStateBegin()
		{
			if (reversed.value) MoveBackward();
			else MoveForward();
		}
		
		private void MoveForward()
		{
			dequeElement.value.Animator.TargetTo(dequeElement.value.GetLerpTarget(targetIndex.value + 1), targetIndex.value < dequeElement.value.Page.Diajector.LerpTargetCount - 2 ? KeepMovingForward : null);
		}
		
		private void KeepMovingForward(LerpTargetAnimator animator)
		{
			targetIndex.value++;
			MoveForward();
		}
		
		private void MoveBackward()
		{
			dequeElement.value.Animator.TargetTo(dequeElement.value.GetLerpTarget(targetIndex.value), targetIndex.value > 0 ? KeepMovingBackward : null);
		}
		
		private void KeepMovingBackward(LerpTargetAnimator animator)
		{
			targetIndex.value--;
			MoveBackward();
		}
		
		public override void OnStateEnd()
		{
			dequeElement.value.Animator.Cancel();
		}
	}
}

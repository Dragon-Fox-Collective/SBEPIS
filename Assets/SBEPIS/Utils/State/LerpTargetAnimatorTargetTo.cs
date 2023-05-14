using Arbor;
using UnityEngine;

namespace SBEPIS.Utils.State
{
	[AddBehaviourMenu("LerpTargetAnimator/TargetTo")]
	[BehaviourTitle("TargetTo")]
	public class LerpTargetAnimatorTargetTo : StateBehaviour
	{
		[SerializeField] private FlexibleLerpTargetAnimator animator;
		[SerializeField] private FlexibleLerpTarget target;
		
		public override void OnStateBegin() => animator.value.TargetTo(target.value);
	}
}

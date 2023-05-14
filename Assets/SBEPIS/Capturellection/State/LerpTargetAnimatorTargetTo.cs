using Arbor;
using SBEPIS.Utils;
using UnityEngine;

namespace SBEPIS.Capturellection.State
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

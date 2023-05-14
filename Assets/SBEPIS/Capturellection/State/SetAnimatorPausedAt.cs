using Arbor;
using UnityEngine;

namespace SBEPIS.Capturellection.State
{
	[AddBehaviourMenu("LerpTargetAnimator/SetAnimatorPausedAt")]
	public class SetAnimatorPausedAt : StateBehaviour
	{
		[SerializeField] private FlexibleLerpTargetAnimator animator = new();
		[SerializeField] private FlexibleLerpTarget target = new();
		
		public override void OnStateBegin() => animator.value.SetPausedAt(target.value);
	}
}

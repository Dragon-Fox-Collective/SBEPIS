using Arbor;
using UnityEngine;

namespace SBEPIS.Utils.State
{
	[AddBehaviourMenu("LerpTargetAnimator/SetPausedAt")]
	[BehaviourTitle("SetPausedAt")]
	public class LerpTargetAnimatorSetPausedAt : StateBehaviour
	{
		[SerializeField] private FlexibleLerpTargetAnimator animator = new();
		[SerializeField] private FlexibleLerpTarget target = new();
		
		public override void OnStateBegin() => animator.value.SetPausedAt(target.value);
	}
}

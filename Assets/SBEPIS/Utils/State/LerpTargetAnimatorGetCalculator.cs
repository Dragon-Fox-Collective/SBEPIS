using System;
using Arbor;

namespace SBEPIS.Utils.State
{
	[AddBehaviourMenu("LerpTargetAnimator/LerpTargetAnimator.Get")]
	[BehaviourTitle("LerpTargetAnimator.Get")]
	public class LerpTargetAnimatorCalculator : GetCalculator<LerpTargetAnimator, OutputSlotLerpTargetAnimator> { }
	
	[Serializable]
	public class OutputSlotLerpTargetAnimator : OutputSlot<LerpTargetAnimator> { }
	
	[Serializable]
	public class FlexibleLerpTargetAnimator : FlexibleComponent<LerpTargetAnimator> { }
}

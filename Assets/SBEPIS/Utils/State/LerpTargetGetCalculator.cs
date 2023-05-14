using System;
using Arbor;

namespace SBEPIS.Utils.State
{
	[AddBehaviourMenu("LerpTarget/LerpTarget.Get")]
	[BehaviourTitle("LerpTarget.Get")]
	public class LerpTargetCalculator : GetCalculator<LerpTarget, OutputSlotLerpTarget> { }
	
	[Serializable]
	public class OutputSlotLerpTarget : OutputSlot<LerpTarget> { }
	
	[Serializable]
	public class FlexibleLerpTarget : FlexibleComponent<LerpTarget> { }
}

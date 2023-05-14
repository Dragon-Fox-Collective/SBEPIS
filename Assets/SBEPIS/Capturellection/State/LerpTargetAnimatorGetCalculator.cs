using System;
using Arbor;
using SBEPIS.Utils;
using UnityEngine;

namespace SBEPIS.Capturellection.State
{
	[AddBehaviourMenu("LerpTargetAnimator/LerpTargetAnimator.Get")]
	[BehaviourTitle("LerpTargetAnimator.Get")]
	public class LerpTargetAnimatorCalculator : GetCalculator<LerpTargetAnimator, OutputSlotLerpTargetAnimator> { }
	
	[Serializable]
	public class OutputSlotLerpTargetAnimator : OutputSlot<LerpTargetAnimator> { }
	
	[Serializable]
	public class FlexibleLerpTargetAnimator : FlexibleComponent<LerpTargetAnimator> { }
}

using System;
using Arbor;
using SBEPIS.Utils;
using UnityEngine;

namespace SBEPIS.Capturellection.CardState
{
	[AddBehaviourMenu("LerpTarget/LerpTarget.Get")]
	[BehaviourTitle("LerpTarget.Get")]
	public class LerpTargetCalculator : GetCalculator<LerpTarget, OutputSlotLerpTarget> { }
	
	[Serializable]
	public class OutputSlotLerpTarget : OutputSlot<LerpTarget> { }
	
	[Serializable]
	public class FlexibleLerpTarget : FlexibleComponent<LerpTarget> { }
}

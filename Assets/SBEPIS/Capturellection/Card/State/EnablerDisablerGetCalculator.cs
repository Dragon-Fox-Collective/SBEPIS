using System;
using Arbor;
using SBEPIS.Utils;
using UnityEngine;

namespace SBEPIS.Capturellection.CardState
{
	[AddBehaviourMenu("EnablerDisabler/EnablerDisabler.Get")]
	[BehaviourTitle("EnablerDisabler.Get")]
	public class EnablerDisablerGetCalculator : GetCalculator<EnablerDisabler, OutputSlotEnablerDisabler> { }
	
	[Serializable]
	public class OutputSlotEnablerDisabler : OutputSlot<EnablerDisabler> { }
	
	[Serializable]
	public class FlexibleEnablerDisabler : FlexibleComponent<EnablerDisabler> { }
}

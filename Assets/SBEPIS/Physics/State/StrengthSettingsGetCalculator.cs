using System;
using Arbor;

namespace SBEPIS.Physics.State
{
	//[AddBehaviourMenu("StrengthSettings/StrengthSettings.Get")]
	//[BehaviourTitle("StrengthSettings.Get")]
	//public class StrengthSettingsGetCalculator : GetCalculator<StrengthSettings, OutputSlotStrengthSettings> { }
	
	[Serializable]
	public class OutputSlotStrengthSettings : OutputSlot<StrengthSettings> { }
	
	[Serializable]
	public class FlexibleStrengthSettings : FlexibleField<StrengthSettings> { }
}

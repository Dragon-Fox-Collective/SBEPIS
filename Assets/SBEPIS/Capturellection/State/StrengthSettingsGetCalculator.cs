using System;
using Arbor;
using SBEPIS.Physics;

namespace SBEPIS.Capturellection.State
{
	//[AddBehaviourMenu("StrengthSettings/StrengthSettings.Get")]
	//[BehaviourTitle("StrengthSettings.Get")]
	//public class StrengthSettingsGetCalculator : GetCalculator<StrengthSettings, OutputSlotStrengthSettings> { }
	
	[Serializable]
	public class OutputSlotStrengthSettings : OutputSlot<StrengthSettings> { }
	
	[Serializable]
	public class FlexibleStrengthSettings : FlexibleField<StrengthSettings> { }
}

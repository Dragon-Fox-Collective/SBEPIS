using System;
using Arbor;

namespace SBEPIS.Capturellection.CardState
{
	[AddBehaviourMenu("Diajector/Diajector.Get")]
	[BehaviourTitle("Diajector.Get")]
	public class DiajectorGetCalculator : GetCalculator<Diajector, OutputSlotDiajector> { }
	
	[Serializable]
	public class OutputSlotDiajector : OutputSlot<Diajector> { }
	
	[Serializable]
	public class FlexibleDiajector : FlexibleComponent<Diajector> { }
}

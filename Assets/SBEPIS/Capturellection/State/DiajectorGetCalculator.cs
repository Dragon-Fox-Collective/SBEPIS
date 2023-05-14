using System;
using Arbor;
using SBEPIS.Utils.State;

namespace SBEPIS.Capturellection.State
{
	[AddBehaviourMenu("Diajector/Diajector.Get")]
	[BehaviourTitle("Diajector.Get")]
	public class DiajectorGetCalculator : GetCalculator<Diajector, OutputSlotDiajector> { }
	
	[Serializable]
	public class OutputSlotDiajector : OutputSlot<Diajector> { }
	
	[Serializable]
	public class FlexibleDiajector : FlexibleComponent<Diajector> { }
}

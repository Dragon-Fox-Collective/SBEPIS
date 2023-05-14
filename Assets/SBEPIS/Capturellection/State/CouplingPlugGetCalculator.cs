using System;
using Arbor;
using SBEPIS.Controller;

namespace SBEPIS.Capturellection.State
{
	[AddBehaviourMenu("CouplingPlug/CouplingPlug.Get")]
	[BehaviourTitle("CouplingPlug.Get")]
	public class CouplingPlugGetCalculator : GetCalculator<CouplingPlug, OutputSlotCouplingPlug> { }
	
	[Serializable]
	public class OutputSlotCouplingPlug : OutputSlot<CouplingPlug> { }
	
	[Serializable]
	public class FlexibleCouplingPlug : FlexibleComponent<CouplingPlug> { }
}

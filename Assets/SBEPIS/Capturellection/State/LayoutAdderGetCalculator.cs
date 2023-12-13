using System;
using Arbor;
using SBEPIS.Utils.State;

namespace SBEPIS.Capturellection.State
{
	[AddBehaviourMenu("LayoutAdder/LayoutAdder.Get")]
	[BehaviourTitle("LayoutAdder.Get")]
	public class LayoutAdderGetCalculator : GetCalculator<LayoutAdder, OutputSlotLayoutAdder> { }
	
	[Serializable]
	public class OutputSlotLayoutAdder : OutputSlot<LayoutAdder> { }
	
	[Serializable]
	public class FlexibleLayoutAdder : FlexibleComponent<LayoutAdder> { }
}

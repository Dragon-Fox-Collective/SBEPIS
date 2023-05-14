using System;
using Arbor;
using SBEPIS.Utils.State;

namespace SBEPIS.Capturellection.State
{
	[AddBehaviourMenu("DequeElement/DequeElement.Get")]
	[BehaviourTitle("DequeElement.Get")]
	public class DequeElementGetCalculator : GetCalculator<DequeElement, OutputSlotDequeElement> { }
	
	[Serializable]
	public class OutputSlotDequeElement : OutputSlot<DequeElement> { }
	
	[Serializable]
	public class FlexibleDequeElement : FlexibleComponent<DequeElement> { }
}

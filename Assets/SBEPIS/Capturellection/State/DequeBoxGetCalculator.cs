using System;
using Arbor;

namespace SBEPIS.Capturellection.State
{
	[AddBehaviourMenu("DequeBox/DequeBox.Get")]
	[BehaviourTitle("DequeBox.Get")]
	public class DequeBoxGetCalculator : GetCalculator<DequeBox, OutputSlotDequeBox> { }
	
	[Serializable]
	public class OutputSlotDequeBox : OutputSlot<DequeBox> { }
	
	[Serializable]
	public class FlexibleDequeBox : FlexibleComponent<DequeBox> { }
}

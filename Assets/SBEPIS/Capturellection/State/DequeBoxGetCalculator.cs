using System;
using Arbor;
using SBEPIS.Utils.State;

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

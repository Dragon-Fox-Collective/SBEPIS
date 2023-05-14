using System;
using Arbor;
using SBEPIS.Utils.State;

namespace SBEPIS.Capturellection.State
{
	[AddBehaviourMenu("DequeBoxOwner/DequeBoxOwner.Get")]
	[BehaviourTitle("DequeBoxOwner.Get")]
	public class DequeBoxOwnerGetCalculator : GetCalculator<DequeBoxOwner, OutputSlotDequeBoxOwner> { }
	
	[Serializable]
	public class OutputSlotDequeBoxOwner : OutputSlot<DequeBoxOwner> { }
	
	[Serializable]
	public class FlexibleDequeBoxOwner : FlexibleComponent<DequeBoxOwner> { }
}

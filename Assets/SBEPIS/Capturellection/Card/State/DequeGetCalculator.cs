using System;
using Arbor;

namespace SBEPIS.Capturellection.CardState
{
	[AddBehaviourMenu("Deque/Deque.Get")]
	[BehaviourTitle("Deque.Get")]
	public class DequeGetCalculator : GetCalculator<Deque, OutputSlotDeque> { }
	
	[Serializable]
	public class OutputSlotDeque : OutputSlot<Deque> { }
	
	[Serializable]
	public class FlexibleDeque : FlexibleComponent<Deque> { }
}

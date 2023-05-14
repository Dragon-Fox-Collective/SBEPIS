using System;
using Arbor;
using SBEPIS.Utils.State;

namespace SBEPIS.Capturellection.State
{
	[AddBehaviourMenu("Deque/Deque.Get")]
	[BehaviourTitle("Deque.Get")]
	public class DequeGetCalculator : GetCalculator<Deque, OutputSlotDeque> { }
	
	[Serializable]
	public class OutputSlotDeque : OutputSlot<Deque> { }
	
	[Serializable]
	public class FlexibleDeque : FlexibleComponent<Deque> { }
}

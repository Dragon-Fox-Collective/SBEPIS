using System;
using Arbor;
using UnityEngine;

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

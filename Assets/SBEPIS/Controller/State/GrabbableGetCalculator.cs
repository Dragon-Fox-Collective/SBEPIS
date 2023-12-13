using System;
using Arbor;
using SBEPIS.Utils.State;

namespace SBEPIS.Controller.State
{
	[AddBehaviourMenu("Grabbable/Grabbable.Get")]
	[BehaviourTitle("Grabbable.Get")]
	public class GrabbableGetCalculator : GetCalculator<Grabbable, OutputSlotGrabbable> { }
	
	[Serializable]
	public class OutputSlotGrabbable : OutputSlot<Grabbable> { }
	
	[Serializable]
	public class FlexibleGrabbable : FlexibleComponent<Grabbable> { }
}

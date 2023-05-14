using System;
using Arbor;
using SBEPIS.Controller;
using SBEPIS.Utils.State;
using UnityEngine;

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

using Arbor;
using UnityEngine;

namespace SBEPIS.Capturellection.State
{
	[AddBehaviourMenu("Grabbable/Grabbable.IsBeingHeld")]
	[BehaviourTitle("Grabbable.IsBeingHeld")]
	public class GrabbableIsBeingHeldCalculator : Calculator
	{
		[SerializeField] private FlexibleGrabbable grabbable = new();
		[SerializeField] private OutputSlotBool isBeingHeld = new();
		
		public override void OnCalculate() => isBeingHeld.SetValue(grabbable.value && grabbable.value.IsBeingHeld);
	}
}

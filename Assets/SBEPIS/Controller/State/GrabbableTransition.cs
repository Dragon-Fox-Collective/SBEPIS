using Arbor;
using SBEPIS.State;
using UnityEngine;
using UnityEngine.Events;

namespace SBEPIS.Controller.State
{
	[AddBehaviourMenu("Transition/GrabbableTransition")]
	public class GrabbableTransition : EventTransition<Grabber, Grabbable>
	{
		[SerializeField] private FlexibleGrabbable grabbable;
		
		[SerializeField] private StateLink onGrab;
		[SerializeField] private StateLink onDrop;
		
		protected override bool InitialValue => grabbable.value && grabbable.value.IsBeingHeld;
		protected override StateLink TrueLink => onGrab;
		protected override StateLink FalseLink => onDrop;
		protected override UnityEvent<Grabber, Grabbable> TrueEvent => grabbable.value ? grabbable.value.OnGrab : null;
		protected override UnityEvent<Grabber, Grabbable> FalseEvent => grabbable.value ? grabbable.value.OnDrop : null;
	}
}

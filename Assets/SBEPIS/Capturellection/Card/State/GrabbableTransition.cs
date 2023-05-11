using Arbor;
using SBEPIS.Controller;
using UnityEngine;
using UnityEngine.Events;

namespace SBEPIS.Capturellection.CardState
{
	[AddBehaviourMenu("Transition/GrabbableTransition")]
	public class GrabbableTransition : EventTransition<Grabber, Grabbable>
	{
		[SerializeField] private FlexibleGrabbable grabbable = new();
		
		[SerializeField] private StateLink onGrab = new();
		[SerializeField] private StateLink onDrop = new();
		
		protected override bool InitialValue => grabbable.value.IsBeingHeld;
		protected override StateLink TrueLink => onGrab;
		protected override StateLink FalseLink => onDrop;
		protected override UnityEvent<Grabber, Grabbable> TrueEvent => grabbable.value.onGrab;
		protected override UnityEvent<Grabber, Grabbable> FalseEvent => grabbable.value.onDrop;
	}
}

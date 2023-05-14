using Arbor;
using SBEPIS.Controller;
using UnityEngine;
using UnityEngine.Events;

namespace SBEPIS.Capturellection.State
{
	[AddBehaviourMenu("Transition/CouplingPlugTransition")]
	public class CouplingPlugTransition : EventTransition<CouplingPlug, CouplingSocket>
	{
		[SerializeField] private FlexibleCouplingPlug couplingPlug;
		
		[SerializeField] private StateLink onCouple;
		[SerializeField] private StateLink onDecouple;
		
		protected override bool InitialValue => couplingPlug.value && couplingPlug.value.IsCoupled;
		protected override StateLink TrueLink => onCouple;
		protected override StateLink FalseLink => onDecouple;
		protected override UnityEvent<CouplingPlug, CouplingSocket> TrueEvent => couplingPlug.value ? couplingPlug.value.onCouple : null;
		protected override UnityEvent<CouplingPlug, CouplingSocket> FalseEvent => couplingPlug.value ? couplingPlug.value.onDecouple : null;
	}
}

using Arbor;
using SBEPIS.State;
using UnityEngine;
using UnityEngine.Events;

namespace SBEPIS.Capturellection.State
{
	[AddBehaviourMenu("Transition/DequeElementInLayoutAreaTransition")]
	public class DequeElementInLayoutAreaTransition : EventTransition
	{
		[SerializeField] private FlexibleDequeElement dequeElement = new();
		
		[SerializeField] private StateLink onEnterLayoutArea = new();
		[SerializeField] private StateLink onExitLayoutArea = new();
		
		protected override bool InitialValue => dequeElement.value.IsInLayoutArea;
		protected override StateLink TrueLink => onEnterLayoutArea;
		protected override StateLink FalseLink => onExitLayoutArea;
		protected override UnityEvent TrueEvent => dequeElement.value.OnEnterLayoutArea;
		protected override UnityEvent FalseEvent => dequeElement.value.OnExitLayoutArea;
	}
}

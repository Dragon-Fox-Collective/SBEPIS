using Arbor;
using UnityEngine;
using UnityEngine.Events;

namespace SBEPIS.Capturellection.CardState
{
	[AddBehaviourMenu("Transition/LayoutAreaTransition")]
	public class LayoutAreaTransition : EventTransition
	{
		[SerializeField] private FlexibleDequeElement dequeElement = new();
		
		[SerializeField] private StateLink onEnterLayoutArea = new();
		[SerializeField] private StateLink onExitLayoutArea = new();
		
		protected override bool InitialValue => dequeElement.value.IsInLayoutArea;
		protected override StateLink TrueLink => onEnterLayoutArea;
		protected override StateLink FalseLink => onExitLayoutArea;
		protected override UnityEvent TrueEvent => dequeElement.value.onEnterLayoutArea;
		protected override UnityEvent FalseEvent => dequeElement.value.onExitLayoutArea;
	}
}

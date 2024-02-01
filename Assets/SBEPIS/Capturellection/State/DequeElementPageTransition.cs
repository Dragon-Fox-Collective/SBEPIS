using Arbor;
using SBEPIS.State;
using UnityEngine;
using UnityEngine.Events;

namespace SBEPIS.Capturellection.State
{
	[AddBehaviourMenu("Transition/DequeElementPageTransition")]
	public class DequeElementPageTransition : EventTransition
	{
		[SerializeField] private FlexibleDequeElement dequeElement = new();
		
		[SerializeField] private StateLink onOpenPage = new();
		[SerializeField] private StateLink onClosePage = new();
		
		protected override bool InitialValue => dequeElement.value.Page.IsOpen;
		protected override StateLink TrueLink => onOpenPage;
		protected override StateLink FalseLink => onClosePage;
		protected override UnityEvent TrueEvent => dequeElement.value.Page.OnOpen;
		protected override UnityEvent FalseEvent => dequeElement.value.Page.OnClose;
	}
}

using SBEPIS.Capturllection.DequeState;
using UnityEngine;

namespace SBEPIS.Capturllection
{
	[RequireComponent(typeof(Deque), typeof(DequeBoxStateMachine))]
	public class DequeBox : MonoBehaviour
	{
		public bool IsDeployed => state.IsDeployed;
		
		public DequeBoxOwner DequeBoxOwner { get; set; }
		
		public Deque Deque { get; private set; }
		private DequeBoxStateMachine state;
		
		private void Awake()
		{
			Deque = GetComponent<Deque>();
			state = GetComponent<DequeBoxStateMachine>();
		}
		
		public void SetCoupledState()
		{
			state.IsCoupled = true;
		}
		
		public void SetDecoupledState()
		{
			state.IsDeployed = true;
			state.IsCoupled = false;
		}
		
		public void SetUnbindState()
		{
			state.IsBound = false;
			state.IsDiajectorOpen = false;
			state.IsDeployed = false;
		}
		
		public void SetBindState()
		{
			state.IsBound = true;
			state.IsDiajectorOpen = Deque.DequeOwner.diajector.IsOpen;
			state.IsDeployed = Deque.DequeOwner.diajector.IsOpen;
		}
		
		public void OpenDiajector()
		{
			state.IsDiajectorOpen = true;
		}
		
		public void CloseDiajector()
		{
			state.IsDiajectorOpen = false;
		}
		
		public void RetrieveDeque()
		{
			CloseDiajector();
			state.IsDeployed = false;
		}

		public void TossDeque()
		{
			SetDecoupledState();
			state.Toss();
		}
	}
}

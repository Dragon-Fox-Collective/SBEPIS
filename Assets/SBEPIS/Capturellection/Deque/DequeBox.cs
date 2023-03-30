using System;
using SBEPIS.Capturellection.DequeState;
using UnityEngine;

namespace SBEPIS.Capturellection
{
	[RequireComponent(typeof(Deque), typeof(DequeBoxStateMachine))]
	public class DequeBox : MonoBehaviour
	{
		public bool IsDeployed => state.IsDeployed;
		
		[NonSerialized]
		public DequeBoxOwner dequeBoxOwner;
		
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
			state.IsDeployed = false;
		}
		
		public void SetBindState()
		{
			state.IsBound = true;
			state.IsDeployed = dequeBoxOwner.DequeOwner.diajector.IsOpen;
		}
		
		public void OpenDiajector()
		{
			state.TriggerDiajector();
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

using SBEPIS.Utils;
using UnityEngine;

namespace SBEPIS.Capturllection.DequeState
{
	public class DequeStateMachine : StateMachine
	{
		public DequeStateMachine(Animator state) : base(state) {}
		
		private static readonly int IsGrabbed = Animator.StringToHash("Is Grabbed");
		public bool isGrabbed
		{
			get => state.GetBool(IsGrabbed);
			set => state.SetBool(IsGrabbed, value);
		}
		
		private static readonly int IsCoupled = Animator.StringToHash("Is Coupled");
		public bool isCoupled
		{
			get => state.GetBool(IsCoupled);
			set => state.SetBool(IsCoupled, value);
		}
		
		private static readonly int IsBound = Animator.StringToHash("Is Bound");
		public bool isBound
		{
			get => state.GetBool(IsBound);
			set => state.SetBool(IsBound, value);
		}
		
		private static readonly int IsDiajectorOpen = Animator.StringToHash("Is Diajector Open");
		public bool isDiajectorOpen
		{
			get => state.GetBool(IsDiajectorOpen);
			set => state.SetBool(IsDiajectorOpen, value);
		}
		
		private static readonly int IsDeployed = Animator.StringToHash("Is Deployed");
		public bool isDeployed
		{
			get => state.GetBool(IsDeployed);
			set => state.SetBool(IsDeployed, value);
		}
		
		private static readonly int OnToss = Animator.StringToHash("On Toss");
		public void Toss() => state.SetTrigger(OnToss);
	}
}

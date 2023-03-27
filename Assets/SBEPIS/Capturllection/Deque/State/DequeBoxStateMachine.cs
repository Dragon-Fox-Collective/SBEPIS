using SBEPIS.Controller;
using SBEPIS.Physics;
using SBEPIS.Utils;
using UnityEngine;

namespace SBEPIS.Capturllection.DequeState
{
	[RequireComponent(typeof(DequeBox), typeof(GravitySum), typeof(CollisionTrigger))]
	[RequireComponent(typeof(CouplingPlug))]
	public class DequeBoxStateMachine : StateMachine
	{
		public DequeBox DequeBox { get; private set; }
		public GravitySum GravitySum { get; private set; }
		public CouplingPlug Plug { get; private set; }
		
		public DequeOwner DequeOwner => DequeBox.DequeBoxOwner.DequeOwner;
		public DequeBoxOwner DequeBoxOwner => DequeBox.DequeBoxOwner;
				
		private void Awake()
		{
			DequeBox = GetComponent<DequeBox>();
			GravitySum = GetComponent<GravitySum>();
			Plug = GetComponent<CouplingPlug>();
		}
		
		private static readonly int IsGrabbedKey = Animator.StringToHash("Is Grabbed");
		public bool IsGrabbed
		{
			get => state.GetBool(IsGrabbedKey);
			set => state.SetBool(IsGrabbedKey, value);
		}
		
		private static readonly int IsCoupledKey = Animator.StringToHash("Is Coupled");
		public bool IsCoupled
		{
			get => state.GetBool(IsCoupledKey);
			set => state.SetBool(IsCoupledKey, value);
		}
		
		private static readonly int IsBoundKey = Animator.StringToHash("Is Bound");
		public bool IsBound
		{
			get => state.GetBool(IsBoundKey);
			set => state.SetBool(IsBoundKey, value);
		}
		
		private static readonly int IsDiajectorOpenKey = Animator.StringToHash("Is Diajector Open");
		public bool IsDiajectorOpen
		{
			get => state.GetBool(IsDiajectorOpenKey);
			set => state.SetBool(IsDiajectorOpenKey, value);
		}
		
		private static readonly int IsDeployedKey = Animator.StringToHash("Is Deployed");
		public bool IsDeployed
		{
			get => state.GetBool(IsDeployedKey);
			set => state.SetBool(IsDeployedKey, value);
		}
		
		private static readonly int TossKey = Animator.StringToHash("On Toss");
		public void Toss() => state.SetTrigger(TossKey);
	}
}

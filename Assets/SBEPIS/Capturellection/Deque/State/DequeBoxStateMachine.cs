using System;
using SBEPIS.Controller;
using SBEPIS.Physics;
using SBEPIS.Utils;
using UnityEngine;

namespace SBEPIS.Capturellection.DequeState
{
	[RequireComponent(typeof(DequeBox), typeof(GravitySum), typeof(Grabbable))]
	[RequireComponent(typeof(CouplingPlug), typeof(LerpTargetAnimator))]
	public class DequeBoxStateMachine : StateMachine
	{
		[NonSerialized]
		public LerpTarget lerpTarget;
		
		public DequeBox DequeBox { get; private set; }
		public CouplingPlug Plug { get; private set; }
		public LerpTargetAnimator Animator { get; private set; }
		
		protected override void Awake()
		{
			base.Awake();
			DequeBox = GetComponent<DequeBox>();
			Plug = GetComponent<CouplingPlug>();
			Animator = GetComponent<LerpTargetAnimator>();
		}
		
		private static readonly int IsGrabbedKey = UnityEngine.Animator.StringToHash("Is Grabbed");
		public bool IsGrabbed
		{
			get => State.GetBool(IsGrabbedKey);
			set => State.SetBool(IsGrabbedKey, value);
		}
		
		private static readonly int IsCoupledKey = UnityEngine.Animator.StringToHash("Is Coupled");
		public bool IsCoupled
		{
			get => State.GetBool(IsCoupledKey);
			set => State.SetBool(IsCoupledKey, value);
		}
		
		private static readonly int IsDeployedKey = UnityEngine.Animator.StringToHash("Is Deployed");
		public bool IsDeployed
		{
			get => State.GetBool(IsDeployedKey);
			set => State.SetBool(IsDeployedKey, value);
		}
	}
}

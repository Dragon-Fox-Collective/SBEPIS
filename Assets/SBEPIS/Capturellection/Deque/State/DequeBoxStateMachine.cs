using System;
using KBCore.Refs;
using SBEPIS.Controller;
using SBEPIS.Physics;
using SBEPIS.Utils;
using UnityEngine;

namespace SBEPIS.Capturellection.DequeState
{
	[RequireComponent(typeof(GravitySum), typeof(Grabbable))]
	[RequireComponent(typeof(CouplingPlug), typeof(LerpTargetAnimator))]
	public class DequeBoxStateMachine : StateMachine
	{
		[NonSerialized]
		public LerpTarget lerpTarget;
		
		[SerializeField, HideInInspector, Self]
		private DequeBox dequeBox;
		public DequeBox DequeBox => dequeBox;
		
		[SerializeField, HideInInspector, Self]
		private CouplingPlug plug;
		public CouplingPlug Plug => plug;
		
		[SerializeField, HideInInspector, Self]
		private LerpTargetAnimator animator;
		public LerpTargetAnimator Animator => animator;
		
		private void OnValidate() => this.ValidateRefs();
		
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

using System.Diagnostics.CodeAnalysis;
using SBEPIS.Utils;
using UnityEngine;

namespace SBEPIS.Capturellection.CardState
{
	public class DequeStorableStateMachine : StateMachine
	{
		public DequeElement Card { get; private set; }
		
		[MaybeNull]
		public InventoryStorableCaptureLayoutAdder LayoutAdder { get; private set; }
		
		protected override void Awake()
		{
			base.Awake();
			Card = GetComponent<DequeElement>();
			LayoutAdder = GetComponent<InventoryStorableCaptureLayoutAdder>();
		}
		
		private static readonly int IsGrabbedKey = Animator.StringToHash("Is Grabbed");
		public bool IsGrabbed
		{
			get => State.GetBool(IsGrabbedKey);
			set => State.SetBool(IsGrabbedKey, value);
		}
		
		private static readonly int IsPageOpenKey = Animator.StringToHash("Is Page Open");
		public bool IsPageOpen
		{
			get => State.GetBool(IsPageOpenKey);
			set => State.SetBool(IsPageOpenKey, value);
		}
		
		private static readonly int IsAssemblingKey = Animator.StringToHash("Is Assembling");
		public bool IsAssembling
		{
			get => State.GetBool(IsAssemblingKey);
			set => State.SetBool(IsAssemblingKey, value);
		}
		
		private static readonly int IsDisassemblingKey = Animator.StringToHash("Is Disassembling");
		public bool IsDisassembling
		{
			get => State.GetBool(IsDisassemblingKey);
			set => State.SetBool(IsDisassemblingKey, value);
		}
		
		private static readonly int HasBeenAssembledKey = Animator.StringToHash("Has Been Assembled");
		public bool HasBeenAssembled
		{
			get => State.GetBool(HasBeenAssembledKey);
			set => State.SetBool(HasBeenAssembledKey, value);
		}
		
		private static readonly int IsBoundKey = Animator.StringToHash("Is Bound");
		public bool IsBound
		{
			get => State.GetBool(IsBoundKey);
			set => State.SetBool(IsBoundKey, value);
		}
		
		private static readonly int IsInLayoutAreaKey = Animator.StringToHash("Is In Layout Area");
		public bool IsInLayoutArea
		{
			get => State.GetBool(IsInLayoutAreaKey);
			set => State.SetBool(IsInLayoutAreaKey, value);
		}
		
		private static readonly int TargetNumberKey = Animator.StringToHash("Target Number");
		public int TargetNumber
		{
			get => State.GetInteger(TargetNumberKey);
			set => State.SetInteger(TargetNumberKey, value);
		}
		
		private static readonly int ForceOpenKey = Animator.StringToHash("On Force Open");
		public void ForceOpen() => State.SetTrigger(ForceOpenKey);
		
		private static readonly int ForceCloseKey = Animator.StringToHash("On Force Close");
		public void ForceClose() => State.SetTrigger(ForceCloseKey);
	}
}

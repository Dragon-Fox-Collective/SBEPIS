using System.Diagnostics.CodeAnalysis;
using KBCore.Refs;
using SBEPIS.Physics;
using SBEPIS.UI;
using SBEPIS.Utils;
using UnityEngine;

namespace SBEPIS.Capturellection.CardState
{
	[RequireComponent(typeof(EnablerDisabler))]
	public class DequeElementStateMachine : StateMachine
	{
		[SerializeField, Self] private DequeElement card;
		public DequeElement Card => card;
		
		[SerializeField, Self] private EnablerDisabler enablerDisabler;
		public EnablerDisabler EnablerDisabler => enablerDisabler;
		
		[SerializeField, Self(Flag.Optional)] private InventoryStorableCaptureLayoutAdder layoutAdder;
		[MaybeNull] public InventoryStorableCaptureLayoutAdder LayoutAdder => layoutAdder;
		
		[SerializeField, Self(Flag.Optional)] private new Rigidbody rigidbody;
		public Rigidbody Rigidbody => rigidbody;
		
		[SerializeField, Anywhere] private StrengthSettings cardStrength;
		public StrengthSettings CardStrength => cardStrength;
		
		[SerializeField, Anywhere] private ElectricArc electricArcPrefab;
		public ElectricArc ElectricArcPrefab => electricArcPrefab;
		
		/// <summary>
		/// When moving between two targets, this is the index of the first one on the list.
		/// That means it's the target you're coming from if not reversed,
		/// and the target you're going to if reversed.
		/// </summary>
		public int TargetIndex { get; set; }
		
		private void OnValidate() => this.ValidateRefs();
		
		private void Awake()
		{
			card.dequeEvents.onSet.AddListener((_, _) => IsBound = true);
			card.dequeEvents.onUnset.AddListener((_, _, _) => IsBound = false);
			IsBound = card.IsStored;
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
		
		private static readonly int ForceOpenKey = Animator.StringToHash("On Force Open");
		public void ForceOpen() => State.SetTrigger(ForceOpenKey);
		
		private static readonly int ForceCloseKey = Animator.StringToHash("On Force Close");
		public void ForceClose() => State.SetTrigger(ForceCloseKey);
	}
}

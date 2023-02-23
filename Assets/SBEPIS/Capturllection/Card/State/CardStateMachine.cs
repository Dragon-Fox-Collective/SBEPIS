using SBEPIS.Utils;
using UnityEngine;

namespace SBEPIS.Capturllection.CardState
{
	public class CardStateMachine : StateMachine
	{
		public CardStateMachine(Animator state) : base(state) {}
		
		private static readonly int IsGrabbed = Animator.StringToHash("Is Grabbed");
		public bool isGrabbed
		{
			get => state.GetBool(IsGrabbed);
			set => state.SetBool(IsGrabbed, value);
		}
		
		private static readonly int IsPageOpen = Animator.StringToHash("Is Page Open");
		public bool isPageOpen
		{
			get => state.GetBool(IsPageOpen);
			set => state.SetBool(IsPageOpen, value);
		}
		
		private static readonly int IsAssembling = Animator.StringToHash("Is Assembling");
		public bool isAssembling
		{
			get => state.GetBool(IsAssembling);
			set => state.SetBool(IsAssembling, value);
		}
		
		private static readonly int IsDisassembling = Animator.StringToHash("Is Disassembling");
		public bool isDisassembling
		{
			get => state.GetBool(IsDisassembling);
			set => state.SetBool(IsDisassembling, value);
		}
		
		private static readonly int HasBeenAssembled = Animator.StringToHash("Has Been Assembled");
		public bool hasBeenAssembled
		{
			get => state.GetBool(HasBeenAssembled);
			set => state.SetBool(HasBeenAssembled, value);
		}
		
		private static readonly int IsBound = Animator.StringToHash("Is Bound");
		public bool isBound
		{
			get => state.GetBool(IsBound);
			set => state.SetBool(IsBound, value);
		}
		
		private static readonly int IsInLayoutArea = Animator.StringToHash("Is In Layout Area");
		public bool isInLayoutArea
		{
			get => state.GetBool(IsInLayoutArea);
			set => state.SetBool(IsInLayoutArea, value);
		}
		
		private static readonly int TargetNumber = Animator.StringToHash("Target Number");
		public int targetNumber
		{
			get => state.GetInteger(TargetNumber);
			set => state.SetInteger(TargetNumber, value);
		}
		
		private static readonly int OnForceClose = Animator.StringToHash("On Force Close");
		public void ForceClose() => state.SetTrigger(OnForceClose);
	}
}

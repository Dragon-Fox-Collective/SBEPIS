using SBEPIS.Utils;
using UnityEngine;

namespace SBEPIS.Capturllection.CardState
{
	public class CardStateMachine : StateMachine
	{
		private static readonly int IsGrabbedKey = Animator.StringToHash("Is Grabbed");
		public bool IsGrabbed
		{
			get => state.GetBool(IsGrabbedKey);
			set => state.SetBool(IsGrabbedKey, value);
		}
		
		private static readonly int IsPageOpenKey = Animator.StringToHash("Is Page Open");
		public bool IsPageOpen
		{
			get => state.GetBool(IsPageOpenKey);
			set => state.SetBool(IsPageOpenKey, value);
		}
		
		private static readonly int IsAssemblingKey = Animator.StringToHash("Is Assembling");
		public bool IsAssembling
		{
			get => state.GetBool(IsAssemblingKey);
			set => state.SetBool(IsAssemblingKey, value);
		}
		
		private static readonly int IsDisassemblingKey = Animator.StringToHash("Is Disassembling");
		public bool IsDisassembling
		{
			get => state.GetBool(IsDisassemblingKey);
			set => state.SetBool(IsDisassemblingKey, value);
		}
		
		private static readonly int HasBeenAssembledKey = Animator.StringToHash("Has Been Assembled");
		public bool HasBeenAssembled
		{
			get => state.GetBool(HasBeenAssembledKey);
			set => state.SetBool(HasBeenAssembledKey, value);
		}
		
		private static readonly int IsBoundKey = Animator.StringToHash("Is Bound");
		public bool IsBound
		{
			get => state.GetBool(IsBoundKey);
			set => state.SetBool(IsBoundKey, value);
		}
		
		private static readonly int IsInLayoutAreaKey = Animator.StringToHash("Is In Layout Area");
		public bool IsInLayoutArea
		{
			get => state.GetBool(IsInLayoutAreaKey);
			set => state.SetBool(IsInLayoutAreaKey, value);
		}
		
		private static readonly int TargetNumberKey = Animator.StringToHash("Target Number");
		public int TargetNumber
		{
			get => state.GetInteger(TargetNumberKey);
			set => state.SetInteger(TargetNumberKey, value);
		}
		
		private static readonly int ForceOpenKey = Animator.StringToHash("On Force Open");
		public void ForceOpen() => state.SetTrigger(ForceOpenKey);
		
		private static readonly int ForceCloseKey = Animator.StringToHash("On Force Close");
		public void ForceClose() => state.SetTrigger(ForceCloseKey);
	}
}

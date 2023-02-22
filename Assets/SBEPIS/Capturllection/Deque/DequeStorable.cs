using SBEPIS.Controller;
using SBEPIS.Thaumaturgy;
using System;
using System.Collections.Generic;
using System.Linq;
using SBEPIS.Utils;
using UnityEngine;

namespace SBEPIS.Capturllection
{
	[RequireComponent(typeof(Grabbable), typeof(SplitTextureSetup), typeof(Animator))]
	[RequireComponent(typeof(LerpTargetAnimator))]
	public class DequeStorable : MonoBehaviour
	{
		public bool isStoringAllowed = true;
		public readonly List<Func<bool>> storePredicates = new();
		
		public Grabbable grabbable { get; private set; }
		public SplitTextureSetup split { get; private set; }
		public Animator state { get; private set; }
		public LerpTargetAnimator animator { get; private set; }
		
		public DequeOwner owner { get; set; }

		public bool isStored => owner;
		public bool canStore => storePredicates.All(predicate => predicate.Invoke());

		private readonly List<DequeCaptureLayout> layouts = new();

		public static readonly int IsGrabbed = Animator.StringToHash("Is Grabbed");
		public static readonly int IsPageOpen = Animator.StringToHash("Is Page Open");
		public static readonly int IsAssembling = Animator.StringToHash("Is Assembling");
		public static readonly int IsDisassembling = Animator.StringToHash("Is Disassembling");
		public static readonly int HasBeenAssembled = Animator.StringToHash("Has Been Assembled");
		public static readonly int IsBound = Animator.StringToHash("Is Bound");
		public static readonly int IsInLayoutArea = Animator.StringToHash("Is In Layout Area");
		public static readonly int TargetNumber = Animator.StringToHash("Target Number");
		public static readonly int ForceClose = Animator.StringToHash("On Force Close");

		private void Awake()
		{
			grabbable = GetComponent<Grabbable>();
			split = GetComponent<SplitTextureSetup>();
			state = GetComponent<Animator>();
			animator = GetComponent<LerpTargetAnimator>();
			
			storePredicates.Add(() => isStoringAllowed);
			storePredicates.Add(() => !isStored);

			// This sucks but it's the best place to put it for now :/
			Capturellectainer container = GetComponent<Capturellectainer>();
			if (container)
				storePredicates.Add(() => container.capturedItem);

			Punchable punchable = GetComponent<Punchable>();
			if (punchable)
				storePredicates.Add(() => punchable.punchedBits.isPerfectlyGeneric);
		}
		
		public void SetStateGrabbed(bool grabbed)
		{
			state.SetBool(IsGrabbed, grabbed);
		}

		private void OnTriggerEnter(Collider other)
		{
			if (other.attachedRigidbody && other.attachedRigidbody.TryGetComponent(out DequeCaptureLayout layout))
			{
				layouts.Add(layout);
				if (layouts.Count == 1)
					state.SetBool(IsInLayoutArea, true);
			}
		}
		
		private void OnTriggerExit(Collider other)
		{
			if (other.attachedRigidbody && other.attachedRigidbody.TryGetComponent(out DequeCaptureLayout layout))
			{
				layouts.Remove(layout);
				if (layouts.Count == 0)
					state.SetBool(IsInLayoutArea, true);
			}
		}
	}
}
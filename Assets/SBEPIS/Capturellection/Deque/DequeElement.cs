using System;
using KBCore.Refs;
using SBEPIS.Utils;
using SBEPIS.Utils.State;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace SBEPIS.Capturellection
{
	[RequireComponent(typeof(LerpTargetAnimator))]
	public class DequeElement : ValidatedMonoBehaviour
	{
		[SerializeField, Self] private LerpTargetAnimator animator;
		public LerpTargetAnimator Animator => animator;
		
		[SerializeField, Anywhere(Flag.Optional)] private InvokeTransitionReference forceOpen;
		[SerializeField, Anywhere(Flag.Optional)] private InvokeTransitionReference forceClose;
		
		[SerializeField, Anywhere(Flag.Optional)] private Renderer bounds;
		public Vector3 Size => bounds ? ExtensionMethods.Multiply(bounds.localBounds.size, bounds.transform.localScale) : Vector3.zero;
		
		[SerializeField, Anywhere] private Transform root;
		
		[FormerlySerializedAs("dequeOwnerEvents")]
		public EventProperty<DequeElement, Deque, SetCardDequeEvent, UnsetCardDequeEvent> dequeEvents = new();
		public Deque Deque
		{
			get => dequeEvents.Get();
			set => dequeEvents.Set(this, value);
		}
		
		public UnityEvent onStartAssembling;
		public bool IsAssembling { get; private set; }
		public UnityEvent onStartDisassembling;
		public bool IsDisassembling { get; private set; }
		public UnityEvent onStopAssemblingAndDisassembling;
		
		public bool IsStored => Deque;
		
		public bool HasBeenAssembled { get; set; }
		
		public UnityEvent onEnterLayoutArea = new();
		public UnityEvent onExitLayoutArea = new();
		private bool isInLayoutArea;
		public bool IsInLayoutArea
		{
			get => isInLayoutArea;
			set
			{
				if (!isInLayoutArea && value)
				{
					isInLayoutArea = true;
					onEnterLayoutArea.Invoke();
				}
				else if (isInLayoutArea && !value)
				{
					isInLayoutArea = false;
					onExitLayoutArea.Invoke();
				}
			}
		}
		
		private DiajectorPage page;
		public DiajectorPage Page
		{
			get => page;
			set
			{
				if (page == value)
					return;
				if (page && value)
					throw new InvalidOperationException($"Tried to replace the page on {this} before it was nulled");
				page = value;
			}
		}
		public Diajector Diajector => Page.Diajector;
		
		public bool ShouldBeDisplayed => Diajector.ShouldCardBeDisplayed(this);
		
		public void StartAssembling()
		{
			if (IsAssembling)
				return;
			IsAssembling = true;
			IsDisassembling = false;
			onStartAssembling.Invoke();
		}
		
		public void StartDisassembling()
		{
			if (IsDisassembling)
				return;
			IsAssembling = false;
			IsDisassembling = true;
			onStartDisassembling.Invoke();
		}
		
		public void StopAssemblingAndDisassembling()
		{
			if (!IsAssembling && !IsDisassembling)
				return;
			IsAssembling = false;
			IsDisassembling = false;
			onStopAssemblingAndDisassembling.Invoke();
		}
		
		public void ForceOpen()
		{
			StopAssemblingAndDisassembling();
			HasBeenAssembled = true;
			if (forceOpen) forceOpen.Invoke();
		}
		
		public void ForceClose()
		{
			StopAssemblingAndDisassembling();
			if (forceClose) forceClose.Invoke();
		}
		
		public LerpTarget LerpTarget => Page.GetLerpTarget(this);
		public LerpTarget GetLerpTarget(int index) => Page.Diajector.GetLerpTargetAtIndex(this, index);
		public void TeleportToLerpTarget(int index) => Animator.TeleportTo(GetLerpTarget(index));
		
		public void Foo() => print("foo");
		
		public void SetParent(Transform parent) => root.SetParent(parent);
		public Transform Parent => root.parent;
	}
}
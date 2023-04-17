using System;
using KBCore.Refs;
using SBEPIS.Capturellection.CardState;
using SBEPIS.Utils;
using UnityEngine;
using UnityEngine.Serialization;

namespace SBEPIS.Capturellection
{
	[RequireComponent(typeof(LerpTargetAnimator), typeof(DequeElementStateMachine))]
	public class DequeElement : MonoBehaviour
	{
		[SerializeField, Self] private DequeElementStateMachine state;
		public DequeElementStateMachine State => state;
		[SerializeField, Self] private LerpTargetAnimator animator;
		public LerpTargetAnimator Animator => animator;
		
		[SerializeField, Anywhere(Flag.Optional)] private Renderer bounds;
		public Vector3 Size => bounds ? ExtensionMethods.Multiply(bounds.localBounds.size, bounds.transform.localScale) : Vector3.zero;
		
		private void OnValidate() => this.ValidateRefs();
		
		[FormerlySerializedAs("dequeOwnerEvents")]
		public EventProperty<DequeElement, Deque, SetCardDequeEvent, UnsetCardDequeEvent> dequeEvents = new();
		public Deque Deque
		{
			get => dequeEvents.Get();
			set => dequeEvents.Set(this, value);
		}
		
		public bool IsStored => Deque;
		
		private Diajector diajector;
		public Diajector Diajector
		{
			get => diajector;
			set
			{
				if (diajector == value)
					return;
				if (diajector && value)
					throw new InvalidOperationException($"Tried to replace the diajector on {this} before it was nulled");
				diajector = value;
			}
		}
		public bool ShouldBeDisplayed => diajector.ShouldCardBeDisplayed(this);
	}
}
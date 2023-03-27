using SBEPIS.Controller;
using System;
using System.Collections.Generic;
using System.Linq;
using SBEPIS.Capturllection.CardState;
using SBEPIS.Utils;
using UnityEngine;
using UnityEngine.Events;

namespace SBEPIS.Capturllection
{
	[RequireComponent(typeof(Grabbable), typeof(LerpTargetAnimator), typeof(CardStateMachine))]
	public class Card : MonoBehaviour
	{
		public Renderer bounds;
		public bool isStoringAllowed = true;
		public readonly List<Func<bool>> storePredicates = new();
		
		public UnityEvent<DequeOwner, Card> onSetOwner = new();
		
		public Grabbable Grabbable { get; private set; }
		public CardStateMachine State { get; private set; }
		public LerpTargetAnimator Animator { get; private set; }
		public Capturellectainer Container { get; private set; }

		private DequeOwner owner;
		public DequeOwner Owner
		{
			get => owner;
			set
			{
				if (owner == value)
					return;
				
				owner = value;
				
				State.IsBound = owner;
				transform.SetParent(owner ? owner.cardParent : null);
				onSetOwner.Invoke(owner, this);
			}
		}
		
		public bool isStored => owner;
		public bool canStore => storePredicates.All(predicate => predicate.Invoke());
		
		public bool canStoreInto => Container && Container.isEmpty;
		
		private List<DiajectorCaptureLayout> layouts = new();
		
		private void Awake()
		{
			Grabbable = GetComponent<Grabbable>();
			State = GetComponent<CardStateMachine>();
			Animator = GetComponent<LerpTargetAnimator>();
			Container = GetComponent<Capturellectainer>();
			
			storePredicates.Add(() => isStoringAllowed);
			storePredicates.Add(() => !isStored);
			
			if (Container)
				storePredicates.Add(() => Container.capturedItem);
		}
		
		public void SetStateGrabbed(bool grabbed) => State.IsGrabbed = grabbed;
		
		private void OnTriggerEnter(Collider other)
		{
			if (other.attachedRigidbody && other.attachedRigidbody.TryGetComponent(out DiajectorCaptureLayout layout) && canStore)
				AddLayout(layout);
		}
		
		private void OnTriggerExit(Collider other)
		{
			if (other.attachedRigidbody && other.attachedRigidbody.TryGetComponent(out DiajectorCaptureLayout layout) && layout.HasTemporaryTarget(this))
				RemoveLayout(layout);
		}
		
		private void AddLayout(DiajectorCaptureLayout layout)
		{
			layouts.Add(layout);
			if (layouts.Count == 1)
				State.IsInLayoutArea = true;
			layout.AddTemporaryTarget(this);
		}
		
		private void RemoveLayout(DiajectorCaptureLayout layout)
		{
			layouts.Add(layout);
			if (layouts.Count == 1)
				State.IsInLayoutArea = true;
			layout.AddTemporaryTarget(this);
		}
		
		public DiajectorCaptureLayout PopAllLayouts()
		{
			DiajectorCaptureLayout lastLayout = layouts[^1];
			while (layouts.Count > 0)
				RemoveLayout(layouts[0]);
			return lastLayout;
		}
	}
}
using SBEPIS.Controller;
using System.Collections.Generic;
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
		
		public UnityEvent<DequeOwner, Card> onSetOwner = new();
		
		public Grabbable Grabbable { get; private set; }
		public CardStateMachine State { get; private set; }
		public LerpTargetAnimator Animator { get; private set; }
		public Capturellectainer Container { get; private set; }

		private DequeOwner dequeOwner;
		public DequeOwner DequeOwner
		{
			get => dequeOwner;
			set
			{
				if (dequeOwner == value)
					return;
				
				dequeOwner = value;
				
				State.IsBound = dequeOwner;
				transform.SetParent(dequeOwner ? dequeOwner.cardParent : null);
				onSetOwner.Invoke(dequeOwner, this);
			}
		}
		
		public bool IsStored => dequeOwner;
		
		public bool CanStoreInto => Container && Container.IsEmpty;
		
		private List<DiajectorCaptureLayout> layouts = new();
		
		private void Awake()
		{
			Grabbable = GetComponent<Grabbable>();
			State = GetComponent<CardStateMachine>();
			Animator = GetComponent<LerpTargetAnimator>();
			Container = GetComponent<Capturellectainer>();
		}
		
		private void OnTriggerEnter(Collider other)
		{
			if (other.attachedRigidbody && other.attachedRigidbody.TryGetComponent(out DiajectorCaptureLayout layout) && !IsStored)
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
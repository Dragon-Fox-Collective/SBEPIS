using SBEPIS.Controller;
using System.Collections.Generic;
using SBEPIS.Capturellection.CardState;
using SBEPIS.Utils;
using UnityEngine;
using UnityEngine.Events;

namespace SBEPIS.Capturellection
{
	[RequireComponent(typeof(Grabbable), typeof(LerpTargetAnimator), typeof(CardStateMachine))]
	public class DequeStorable : MonoBehaviour
	{
		public Renderer bounds;
		
		public EventProperty<DequeStorable, DequeOwner, SetCardOwnerEvent, UnsetCardOwnerEvent> dequeOwnerEvents = new();
		public DequeOwner DequeOwner
		{
			get => dequeOwnerEvents.Get();
			set => dequeOwnerEvents.Set(this, value, dequeOwner => dequeOwner.cardOwnerSlaveEvents);
		}
		
		public Grabbable Grabbable { get; private set; }
		public CardStateMachine State { get; private set; }
		public LerpTargetAnimator Animator { get; private set; }
		public Capturellectainer Container { get; private set; }

		public bool IsStored => DequeOwner;
		
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
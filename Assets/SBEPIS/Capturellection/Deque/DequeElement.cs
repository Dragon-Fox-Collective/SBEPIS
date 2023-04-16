using SBEPIS.Controller;
using System.Collections.Generic;
using KBCore.Refs;
using SBEPIS.Capturellection.CardState;
using SBEPIS.Utils;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace SBEPIS.Capturellection
{
	[RequireComponent(typeof(Grabbable), typeof(LerpTargetAnimator), typeof(DequeElementStateMachine))]
	public class DequeElement : MonoBehaviour
	{
		[SerializeField, Self]
		private Grabbable grabbable;
		public Grabbable Grabbable => grabbable;
		
		[SerializeField, Self]
		private DequeElementStateMachine state;
		public DequeElementStateMachine State => state;
		
		[SerializeField, Self]
		private LerpTargetAnimator animator;
		public LerpTargetAnimator Animator => animator;
		
		private void OnValidate() => this.ValidateRefs();
		
		public Renderer bounds;
		
		[FormerlySerializedAs("dequeOwnerEvents")]
		public EventProperty<DequeElement, Deque, SetCardDequeEvent, UnsetCardDequeEvent> dequeEvents = new();
		public Deque Deque
		{
			get => dequeEvents.Get();
			set => dequeEvents.Set(this, value);
		}
		
		public bool IsStored => Deque;
	}
}
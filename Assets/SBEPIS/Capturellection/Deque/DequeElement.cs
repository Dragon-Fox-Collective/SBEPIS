using SBEPIS.Controller;
using System.Collections.Generic;
using SBEPIS.Capturellection.CardState;
using SBEPIS.Utils;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace SBEPIS.Capturellection
{
	[RequireComponent(typeof(Grabbable), typeof(LerpTargetAnimator), typeof(DequeStorableStateMachine))]
	public class DequeElement : MonoBehaviour
	{
		public Renderer bounds;
		
		[FormerlySerializedAs("dequeOwnerEvents")]
		public EventProperty<DequeElement, Deque, SetCardDequeEvent, UnsetCardDequeEvent> dequeEvents = new();
		public Deque Deque
		{
			get => dequeEvents.Get();
			set => dequeEvents.Set(this, value);
		}
		
		public Grabbable Grabbable { get; private set; }
		public DequeStorableStateMachine State { get; private set; }
		public LerpTargetAnimator Animator { get; private set; }
		
		public bool IsStored => Deque;
		
		private void Awake()
		{
			Grabbable = GetComponent<Grabbable>();
			State = GetComponent<DequeStorableStateMachine>();
			Animator = GetComponent<LerpTargetAnimator>();
		}
	}
}
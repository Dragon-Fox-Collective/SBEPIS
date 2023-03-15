using System;
using SBEPIS.Physics;
using SBEPIS.UI;
using SBEPIS.Utils;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace SBEPIS.Capturllection
{
	[RequireComponent(typeof(LerpTarget))]
	public class CardTarget : MonoBehaviour
	{
		[FormerlySerializedAs("onCardCreated")]
		public UnityEvent<DequeStorable> onCardBound = new();
		public UnityEvent onPrepareCard = new();
		public UnityEvent onGrab = new();
		public UnityEvent onDrop = new();
		
		public DequeStorable card { get; set; }
		
		public LerpTarget lerpTarget { get; private set; }
		public DiajectorPage page { get; private set; }

		public void Awake()
		{
			lerpTarget = GetComponent<LerpTarget>();
			page = GetComponentInParent<DiajectorPage>();
		}

		public void DropTargettingCard()
		{
			if (card.grabbable.isBeingHeld)
				card.grabbable.grabbingGrabber.Drop();
		}

		public void AttachToTarget(LerpTargetAnimator animator)
		{
			card.state.hasBeenAssembled = true;
		}

		public void DetatchFromTarget(LerpTargetAnimator animator)
		{
			card.state.hasBeenAssembled = false;
		}
	}
}

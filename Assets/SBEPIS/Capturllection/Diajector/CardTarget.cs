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
		public UnityEvent<Card> onCardBound = new();
		public UnityEvent onPrepareCard = new();
		public UnityEvent onGrab = new();
		public UnityEvent onDrop = new();
		
		public Card card { get; set; }
		
		public LerpTarget lerpTarget { get; private set; }
		public DiajectorPage page { get; private set; }

		public void Awake()
		{
			lerpTarget = GetComponent<LerpTarget>();
			page = GetComponentInParent<DiajectorPage>();
		}

		public void DropTargettingCard()
		{
			if (card.Grabbable.isBeingHeld)
				card.Grabbable.grabbingGrabber.Drop();
		}

		public void AttachToTarget(LerpTargetAnimator animator)
		{
			card.State.HasBeenAssembled = true;
		}

		public void DetatchFromTarget(LerpTargetAnimator animator)
		{
			card.State.HasBeenAssembled = false;
		}
	}
}

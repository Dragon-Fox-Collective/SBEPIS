using System;
using KBCore.Refs;
using SBEPIS.Controller;
using SBEPIS.Physics;
using SBEPIS.UI;
using SBEPIS.Utils;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace SBEPIS.Capturellection
{
	[RequireComponent(typeof(LerpTarget))]
	public class CardTarget : MonoBehaviour
	{
		[SerializeField, Self]
		private LerpTarget lerpTarget;
		public LerpTarget LerpTarget => lerpTarget;
		
		private void OnValidate() => this.ValidateRefs();
		
		[FormerlySerializedAs("onCardCreated")]
		public UnityEvent<DequeElement> onCardBound = new();
		public UnityEvent onPrepareCard = new();
		public UnityEvent onGrab = new();
		public UnityEvent onDrop = new();

		private DequeElement card;
		private Grabbable cardGrabbable;
		public DequeElement Card
		{
			get => card;
			set
			{
				card = value;
				cardGrabbable = card.GetComponent<Grabbable>();
			}
		}

		public void DropTargettingCard()
		{
			if (cardGrabbable)
				cardGrabbable.Drop();
		}

		public void AttachToTarget(LerpTargetAnimator animator)
		{
			Card.State.HasBeenAssembled = true;
		}

		public void DetatchFromTarget(LerpTargetAnimator animator)
		{
			Card.State.HasBeenAssembled = false;
		}
	}
}

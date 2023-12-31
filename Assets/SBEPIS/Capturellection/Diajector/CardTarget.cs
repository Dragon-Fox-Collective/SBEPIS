using KBCore.Refs;
using SBEPIS.Controller;
using SBEPIS.Utils;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace SBEPIS.Capturellection
{
	[RequireComponent(typeof(LerpTarget))]
	public class CardTarget : ValidatedMonoBehaviour
	{
		[SerializeField, Self] private LerpTarget lerpTarget;
		public LerpTarget LerpTarget => lerpTarget;
		
		[SerializeField, Self(Flag.Optional)] private DequeElement card;
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
		
		[SerializeField] private float dealDelay = 0.1f;
		public float DealDelay => dealDelay;
		
		[FormerlySerializedAs("onCardCreated")]
		public UnityEvent<DequeElement> onCardBound = new();
		public UnityEvent onPrepareCard = new();
		public UnityEvent onGrab = new();
		public UnityEvent onDrop = new();

		public bool HasBeenAssembled
		{
			get => Card.HasBeenAssembled;
			set => Card.HasBeenAssembled = value;
		}
		
		public void DropTargettingCard()
		{
			if (cardGrabbable)
				cardGrabbable.Drop();
		}
		
		public void StopAssemblingAndDisassemblingCard()
		{
			if (card)
				card.StopAssemblingAndDisassembling();
		}
	}
}

using SBEPIS.Controller;
using System.Collections.Generic;
using SBEPIS.Utils;
using UnityEngine;

namespace SBEPIS.Capturllection
{
	public class DequeCaptureLayout : MonoBehaviour
	{
		public CardTarget cardTargetPrefab;
		public float cardZ = -1;
		public float fetchableCardY = 0.1f;

		private Diajector diajector;
		private readonly Dictionary<DequeStorable, CardTarget> targets = new();
		private readonly List<CardTarget> providedTargets = new();
		private DequePage dequePage;

		private void Awake()
		{
			diajector = GetComponentInParent<Diajector>();
			dequePage = GetComponentInParent<DequePage>();
		}

		private void FixedUpdate()
		{
			TickAndLayoutTargets();
		}

		private void OnTriggerEnter(Collider other)
		{
			if (!diajector.isBound || !other.attachedRigidbody)
				return;
			DequeStorable card = other.attachedRigidbody.GetComponent<DequeStorable>();
			if (!card || card.isStored || !card.canStore)
				return;

			if (card.grabbable.isBeingHeld)
				AddTemporaryTarget(card);
			else
				AddCard(card, AddCardTarget(card));
		}

		private void OnTriggerExit(Collider other)
		{
			if (!diajector.isBound || !other.attachedRigidbody)
				return;
			DequeStorable card = other.attachedRigidbody.GetComponent<DequeStorable>();
			if (!card || card.isStored || !card.canStore)
				return;

			if (card.grabbable.isBeingHeld)
				RemoveTemporaryTarget(card);
		}

		private CardTarget AddCardTarget(DequeStorable card)
		{
			CardTarget newTarget = Instantiate(cardTargetPrefab, transform);
			newTarget.card = card;
			targets.Add(card, newTarget);
			providedTargets.Insert(diajector.deque.definition.GetIndexToInsertAt(providedTargets, newTarget), newTarget);
			return newTarget;
		}

		private void RemoveCardTarget(DequeStorable card)
		{
			CardTarget target = targets[card];
			targets.Remove(card);
			providedTargets.Remove(target);
			Destroy(target);
		}

		public CardTarget AddTemporaryTarget(DequeStorable card)
		{
			card.grabbable.onDrop.AddListener(MakeCardPermanent);
			CardTarget target = AddCardTarget(card);
			target.isTemporary = true;
			return target;
		}

		public void RemoveTemporaryTarget(DequeStorable card)
		{
			card.grabbable.onDrop.RemoveListener(MakeCardPermanent);
			RemoveCardTarget(card);
		}

		private void MakeCardPermanent(Grabber grabber, Grabbable grabbable)
		{
			print("MakeCardPermanent");
			DequeStorable card = grabbable.GetComponent<DequeStorable>();
			card.grabbable.onDrop.RemoveListener(MakeCardPermanent);
			targets[card].isTemporary = false;
			AddCard(card, targets[card]);
		}

		private void AddCard(DequeStorable card, CardTarget target)
		{
			if (!diajector.isBound)
				return;

			Capturllectainer container = card.GetComponent<Capturllectainer>();
			if (container)
			{
				container.onRetrieve.AddListener(RemoveCard);
				container.retrievePredicates.Add(CanFetch);
			}

			ProceduralAnimation anim = dequePage.AddCard(card, target);
			anim.SeekEnd();
			anim.onPlay.Invoke();
			anim.onEnd.Invoke();
		}

		private void RemoveCard(Capturllectainer container, Capturllectable item)
		{
			container.onRetrieve.RemoveListener(RemoveCard);
			container.retrievePredicates.Remove(CanFetch);

			DequeStorable card = container.GetComponent<DequeStorable>();
			dequePage.RemoveCard(card);
			RemoveCardTarget(card);
		}

		private bool CanFetch(Capturllectainer container) => CanFetch(container.GetComponent<DequeStorable>());
		private bool CanFetch(DequeStorable card) => CanFetch(targets[card]);
		private bool CanFetch(CardTarget target) => diajector.deque.definition.CanFetch(providedTargets, target);
		
		private void TickAndLayoutTargets()
		{
			if (!diajector.isBound)
				return;
			
			diajector.deque.definition.TickDeque(providedTargets, Time.fixedDeltaTime);
			diajector.deque.definition.LayoutTargets(providedTargets);
			
			foreach (CardTarget target in targets.Values)
			{
				target.transform.localPosition += Vector3.forward * cardZ;
				target.transform.localRotation *= Quaternion.Euler(0, 180, 0);

				if (CanFetch(target))
					target.transform.position += target.transform.up * fetchableCardY;
			}
		}
	}
}

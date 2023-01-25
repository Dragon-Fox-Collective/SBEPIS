using SBEPIS.Controller;
using SBEPIS.Items;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SBEPIS.Capturllection
{
	public class DequeLayout : MonoBehaviour
	{
		public Diajector diajector;
		public CardTarget cardTargetPrefab;
		public float cardZ = -1;

		private readonly Dictionary<DequeStorable, CardTarget> targets = new();
		private readonly List<CardTarget> providedTargets = new();
		private DequePage dequePage;

		private void Start()
		{
			dequePage = GetComponentInParent<DequePage>();
			if (!dequePage)
				Debug.LogError($"Could not get a DequePage from {this}");
		}

		private void FixedUpdate()
		{
			LayoutTargets();
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
			CardTarget newTarget = Instantiate(cardTargetPrefab.gameObject, transform).GetComponent<CardTarget>();
			newTarget.card = card;
			targets.Add(card, newTarget);
			providedTargets.Add(newTarget);
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
				container.retrievePredicates.Add(CanRetrieve);
			}

			ProceduralAnimation animation = dequePage.AddCard(card, target);
			animation.SeekEnd();
			animation.onPlay.Invoke();
			animation.onEnd.Invoke();
		}

		private void RemoveCard(Capturllectainer container, Capturllectable item)
		{
			container.onRetrieve.RemoveListener(RemoveCard);
			container.retrievePredicates.Remove(CanRetrieve);

			DequeStorable card = container.GetComponent<DequeStorable>();
			dequePage.RemoveCard(card);
			RemoveCardTarget(card);
		}

		private bool CanRetrieve(Capturllectainer container) => diajector.deque.dequeType.CanRetrieve(providedTargets, targets[container.GetComponent<DequeStorable>()]);

		private void LayoutTargets()
		{
			if (!diajector.deque)
				return;

			if (providedTargets.Count > 0)
				diajector.deque.dequeType.LayoutTargets(providedTargets);
			foreach (CardTarget target in targets.Values)
			{
				target.transform.localPosition = target.transform.localPosition + Vector3.forward * cardZ;
				target.transform.localRotation *= Quaternion.Euler(0, 180, 0);
			}
		}
	}
}

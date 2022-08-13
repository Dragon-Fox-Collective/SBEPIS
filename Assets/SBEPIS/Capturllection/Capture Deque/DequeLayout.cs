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
		private readonly Dictionary<DequeStorable, CardTarget> tempTargets = new();
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
			targets.Add(card, newTarget);
			return newTarget;
		}

		private void RemoveCardTarget(DequeStorable card)
		{
			CardTarget target = targets[card];
			targets.Remove(card);
			Destroy(target);
		}

		public CardTarget AddTemporaryTarget(DequeStorable card)
		{
			card.grabbable.onDrop.AddListener(MakeCardPermanent);
			CardTarget target = AddCardTarget(card);
			tempTargets.Add(card, target);
			return target;
		}

		public void RemoveTemporaryTarget(DequeStorable card)
		{
			card.grabbable.onDrop.RemoveListener(MakeCardPermanent);

			RemoveCardTarget(card);
			tempTargets.Remove(card);
		}

		private void MakeCardPermanent(Grabber grabber, Grabbable grabbable)
		{
			DequeStorable card = grabbable.GetComponent<DequeStorable>();
			card.grabbable.onDrop.RemoveListener(MakeCardPermanent);
			tempTargets.Remove(card);
			AddCard(card, targets[card]);
		}

		private void AddCard(DequeStorable card, CardTarget target)
		{
			if (!dequePage)
				return;

			Capturllectainer container = card.GetComponent<Capturllectainer>();
			if (container)
				container.onRetrieve.AddListener(RemoveCard);

			ProceduralAnimation animation = dequePage.AddCard(card, target);
			animation.SeekEnd();
			animation.onPlay.Invoke();
			animation.onEnd.Invoke();
		}

		private void RemoveCard(Capturllectainer container, Capturllectable item)
		{
			container.onRetrieve.RemoveListener(RemoveCard);

			DequeStorable card = container.GetComponent<DequeStorable>();
			dequePage.RemoveCard(card);
			RemoveCardTarget(card);
		}

		private void LayoutTargets()
		{
			if (!diajector.deque)
				return;

			diajector.deque.dequeType.LayoutTargets(targets.Values);
			foreach (CardTarget target in targets.Values)
			{
				target.transform.localPosition = target.transform.localPosition + Vector3.forward * cardZ;
				target.transform.localRotation *= Quaternion.Euler(0, 180, 0);
			}
		}
	}
}

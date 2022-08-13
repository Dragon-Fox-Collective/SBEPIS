using SBEPIS.Controller;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SBEPIS.Capturllection
{
	[RequireComponent(typeof(DequePage))]
	public class DequeLayout : MonoBehaviour
	{
		public Diajector diajector;
		public CardTarget cardTargetPrefab;
		public float cardZ = -1;

		private readonly List<CardTarget> targets = new();
		private DequePage dequePage;

		private void Awake()
		{
			dequePage = GetComponent<DequePage>();
		}

		private void FixedUpdate()
		{
			LayoutTargets();
		}

		private void OnTriggerEnter(Collider other)
		{
			if (!other.attachedRigidbody)
				return;
			DequeStorable card = other.attachedRigidbody.GetComponent<DequeStorable>();
			if (!card || card.isStored)
				return;

			if (card.grabbable.isBeingHeld)
				card.grabbable.onDrop.AddListener(AddCard);
			else
				AddCard(card);
		}

		private void OnTriggerExit(Collider other)
		{
			if (!other.attachedRigidbody)
				return;
			DequeStorable card = other.attachedRigidbody.GetComponent<DequeStorable>();
			if (!card || card.isStored)
				return;

			card.grabbable.onDrop.RemoveListener(AddCard);
		}

		private void AddCard(Grabber grabber, Grabbable grabbable) => AddCard(grabbable.GetComponent<DequeStorable>());

		private void AddCard(DequeStorable card)
		{
			if (!dequePage)
				return;

			card.grabbable.onDrop.RemoveListener(AddCard);

			ProceduralAnimation animation = dequePage.AddCard(card, AddCardTarget());
			animation.SeekEnd();
			animation.onPlay.Invoke();
			animation.onEnd.Invoke();
		}

		private CardTarget AddCardTarget()
		{
			CardTarget newTarget = Instantiate(cardTargetPrefab.gameObject, transform).GetComponent<CardTarget>();
			targets.Add(newTarget);
			return newTarget;
		}

		private void LayoutTargets()
		{
			if (!diajector.deque)
				return;

			diajector.deque.dequeType.LayoutTargets(targets);
			foreach (CardTarget target in targets)
			{
				target.transform.localPosition = target.transform.localPosition + Vector3.forward * cardZ;
				target.transform.localRotation *= Quaternion.Euler(0, 180, 0);
			}
		}
	}
}

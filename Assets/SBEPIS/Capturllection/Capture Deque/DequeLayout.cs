using SBEPIS.Controller;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SBEPIS.Capturllection
{
	[RequireComponent(typeof(DequePage))]
	public class DequeLayout : MonoBehaviour
	{
		public CardTarget cardTargetPrefab;
		public float cardDistance = 10;
		public float cardZ = -1;

		private readonly List<CardTarget> targets = new();
		private DequePage dequePage;

		private void Awake()
		{
			dequePage = GetComponent<DequePage>();
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

			Vector3 position = cardDistance * (targets.Count - 1) / 2 * Vector3.left + cardZ * Vector3.forward;
			foreach (CardTarget target in targets)
			{
				target.transform.localPosition = position;
				target.transform.localRotation = Quaternion.Euler(0, 180, 0);
				position += Vector3.right * cardDistance;
			}

			return newTarget;
		}
	}
}

using System;
using SBEPIS.Controller;
using System.Collections.Generic;
using System.Linq;
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
		
		private void TickAndLayoutTargets()
		{
			if (!diajector.isBound)
				return;
			
			diajector.dequeBox.owner.storage.Tick(Time.fixedDeltaTime);
			diajector.dequeBox.owner.storage.LayoutTargets(targets);
			
			foreach ((DequeStorable card, CardTarget target) in targets)
			{
				target.transform.localPosition += Vector3.forward * cardZ;
				target.transform.localRotation *= Quaternion.Euler(0, 180, 0);
				
				if (CanFetch(card))
					target.transform.position += target.transform.up * fetchableCardY;
			}
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
				AddPermanentTarget(card);
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
			return newTarget;
		}
		
		private void RemoveCardTarget(DequeStorable card)
		{
			CardTarget target = targets[card];
			targets.Remove(card);
			Destroy(target.gameObject);
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
			DequeStorable card = grabbable.GetComponent<DequeStorable>();
			card.grabbable.onDrop.RemoveListener(MakeCardPermanent);
			CardTarget target = targets[card];
			target.isTemporary = false;
			AddCard(card, target);
		}
		
		public CardTarget AddPermanentTarget(DequeStorable card)
		{
			CardTarget target = AddCardTarget(card);
			AddCard(card, target);
			return target;
		}

		public void RemovePermanentTarget(DequeStorable card)
		{
			RemoveCard(card.GetComponent<Capturellectainer>(), card);
		}
		
		private void AddCard(DequeStorable card, CardTarget target)
		{
			if (!diajector.isBound)
				return;
			
			Capturellectainer container = card.GetComponent<Capturellectainer>();
			if (container)
			{
				container.onRetrieve.AddListener(RemoveCard);
				container.retrievePredicates.Add(CanFetch);
			}
			
			LerpTargetAnimator animator = dequePage.AddCard(card, target);
			diajector.dequeBox.lowerTarget.onMoveFrom.Invoke(animator);
			if (!card.grabbable.isBeingHeld)
			{
				animator.TeleportTo(target.lerpTarget);
			}
			else
			{
				animator.SetPausedAt(target.lerpTarget);
				target.onGrab.Invoke();
			}
		}
		
		private void RemoveCard(Capturellectainer container, Capturllectable item) => RemoveCard(container, container.GetComponent<DequeStorable>());
		private void RemoveCard(Capturellectainer container, DequeStorable card)
		{
			container.onRetrieve.RemoveListener(RemoveCard);
			container.retrievePredicates.Remove(CanFetch);
			
			dequePage.RemoveCard(card);
			RemoveCardTarget(card);
		}
		
		private bool CanFetch(Capturellectainer card) => CanFetch(card.GetComponent<DequeStorable>());
		private bool CanFetch(DequeStorable card) => diajector.dequeBox.owner.storage.CanFetch(card);
		
		public void SyncCards(DequeStorage cards)
		{
			foreach ((DequeStorable card,  CardTarget target) in targets.Where(pair => !cards.Contains(pair.Key)).ToList())
				RemovePermanentTarget(card);

			foreach (DequeStorable card in cards.Where(card => !targets.ContainsKey(card)))
				AddPermanentTarget(card);
		}

		private void OnEnable()
		{
			SyncCards(diajector.dequeBox.owner.storage);
		}
	}
}

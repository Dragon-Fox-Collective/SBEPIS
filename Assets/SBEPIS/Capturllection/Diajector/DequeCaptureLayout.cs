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

			DequeStorage storage = diajector.owner.storage;
			storage.Tick(Time.fixedDeltaTime);
			storage.LayoutTargets(targets);
			
			foreach ((DequeStorable card, CardTarget target) in targets)
			{
				target.transform.localPosition += Vector3.forward * cardZ;
				target.transform.localRotation *= Quaternion.Euler(0, 180, 0);
				
				if (storage.CanFetch(card))
					target.transform.position += target.transform.up * fetchableCardY;
			}
		}
		
		public CardTarget AddTemporaryTarget(DequeStorable card)
		{
			if (targets.ContainsKey(card))
				throw new ArgumentException($"Tried to add a target of {card} to {this} but {targets[card]} already exists");
			
			CardTarget newTarget = Instantiate(cardTargetPrefab, transform);
			newTarget.card = card;
			targets.Add(card, newTarget);
			return newTarget;
		}
		
		public void RemoveTemporaryTarget(DequeStorable card)
		{
			CardTarget target = targets[card];
			targets.Remove(card);
			Destroy(target.gameObject);
		}

		public bool HasTemporaryTarget(DequeStorable card) => targets.ContainsKey(card) && !diajector.owner.storage.Contains(card);

		public CardTarget AddPermanentTargetAndCard(DequeStorable card)
		{
			CardTarget target = HasTemporaryTarget(card) ? targets[card] : AddTemporaryTarget(card);
			card.owner = diajector.owner;
			dequePage.AddCard(card, target);
			diajector.owner.dequeBox.lowerTarget.onMoveFrom.Invoke(card.animator);
			return target;
		}
		
		public void RemovePermanentTargetAndCard(DequeStorable card)
		{
			dequePage.RemoveCard(card);
			RemoveTemporaryTarget(card);
		}

		public void SyncCards() => SyncCards(diajector.owner.storage);
		public void SyncCards(DequeStorage cards)
		{
			foreach ((DequeStorable card, CardTarget target) in targets.Where(pair => !cards.Contains(pair.Key)).ToList())
				RemovePermanentTargetAndCard(card);

			foreach (DequeStorable card in cards.Where(card => !targets.ContainsKey(card)))
			{
				CardTarget target = AddPermanentTargetAndCard(card);
				if (card.grabbable.isBeingHeld)
				{
					card.animator.SetPausedAt(target.lerpTarget);
					target.onGrab.Invoke();
				}
				else
				{
					card.animator.TeleportTo(diajector.owner.dequeBox.lowerTarget);
				}
			}
		}
	}
}

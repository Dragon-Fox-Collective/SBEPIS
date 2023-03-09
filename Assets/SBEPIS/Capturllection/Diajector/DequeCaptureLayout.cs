using System;
using SBEPIS.Controller;
using System.Collections.Generic;
using System.Linq;
using SBEPIS.Utils;
using UnityEngine;
using UnityEngine.Serialization;

namespace SBEPIS.Capturllection
{
	public class DequeCaptureLayout : MonoBehaviour
	{
		public CardTarget cardTargetPrefab;
		public float cardZ = -1;
		[FormerlySerializedAs("fetchableCardY")]
		public float fetchableCardZ = 0.1f;
		public Transform directionEndpoint;
		
		private Diajector diajector;
		private readonly Dictionary<DequeStorable, CardTarget> targets = new();
		private DequePage dequePage;
		
		private void Awake()
		{
			diajector = GetComponentInParent<Diajector>();
			dequePage = GetComponentInParent<DequePage>();
		}
		
		private void Update()
		{
			TickAndLayoutTargets(Time.deltaTime);
		}
		
		private void TickAndLayoutTargets(float deltaTime)
		{
			if (!diajector.isBound)
				return;
			
			Storable inventory = diajector.owner.inventory;
			inventory.state.direction = transform.InverseTransformPoint(directionEndpoint.position).normalized;
			inventory.Tick(deltaTime);
			Vector3 inventorySize = inventory.maxPossibleSize;
			inventory.position = Vector3.forward * (cardZ - inventorySize.z / 2);
			inventory.rotation = Quaternion.identity;
			
			foreach ((DequeStorable card, CardTarget target) in targets)
			{
				inventory.LayoutTarget(card, target);
				target.transform.localRotation *= Quaternion.Euler(0, 180, 0);
				if (inventory.CanFetch(card))
					target.transform.localPosition += Vector3.forward * fetchableCardZ;
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

		public bool HasTemporaryTarget(DequeStorable card) => targets.ContainsKey(card) && !diajector.owner.inventory.Contains(card);

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

		public void SyncCards() => SyncCards(diajector.owner.inventory);
		public void SyncCards(Storable inventory)
		{
			foreach ((DequeStorable card, CardTarget target) in targets.Where(pair => !inventory.Contains(pair.Key)).ToList())
				RemovePermanentTargetAndCard(card);

			foreach (DequeStorable card in inventory.Where(card => !targets.ContainsKey(card)))
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

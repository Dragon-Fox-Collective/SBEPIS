using System;
using System.Collections.Generic;
using System.Linq;
using SBEPIS.Utils;
using UnityEngine;
using UnityEngine.Serialization;

namespace SBEPIS.Capturllection
{
	public class DiajectorCaptureLayout : MonoBehaviour
	{
		public CardTarget cardTargetPrefab;
		public float cardZ = -1;
		[FormerlySerializedAs("fetchableCardY")]
		public float fetchableCardZ = 0.1f;
		public Transform directionEndpoint;

		private Storable inventory;
		public Storable Inventory
		{
			set
			{
				inventory = value;
				inventory.transform.SetParent(transform);
			}
		}
		
		private Diajector diajector;
		private readonly Dictionary<Card, CardTarget> targets = new();
		private DiajectorPage page;
		
		private void Awake()
		{
			diajector = GetComponentInParent<Diajector>();
			page = GetComponentInParent<DiajectorPage>();
		}
		
		private void Update()
		{
			TickAndLayoutTargets(Time.deltaTime);
		}
		
		private void TickAndLayoutTargets(float deltaTime)
		{
			if (!diajector.IsBound)
				return;
			
			inventory.state.direction = transform.InverseTransformPoint(directionEndpoint.position).normalized;
			inventory.Tick(deltaTime);
			Vector3 inventorySize = inventory.maxPossibleSize;
			inventory.position = transform.forward * (cardZ + inventorySize.z / 2);
			inventory.rotation = Quaternion.identity;
			
			foreach ((Card card, CardTarget target) in targets)
			{
				inventory.LayoutTarget(card, target);
				target.transform.localRotation *= Quaternion.Euler(0, 180, 0);
				if (inventory.CanFetch(card))
					target.transform.position += -transform.forward * fetchableCardZ;
			}
		}
		
		public CardTarget AddTemporaryTarget(Card card)
		{
			if (targets.ContainsKey(card))
				throw new ArgumentException($"Tried to add a target of {card} to {this} but {targets[card]} already exists");
			
			CardTarget newTarget = Instantiate(cardTargetPrefab, transform);
			newTarget.card = card;
			targets.Add(card, newTarget);
			return newTarget;
		}
		
		public void RemoveTemporaryTarget(Card card)
		{
			CardTarget target = targets[card];
			targets.Remove(card);
			Destroy(target.gameObject);
		}

		public bool HasTemporaryTarget(Card card) => targets.ContainsKey(card) && !inventory.Contains(card);

		public CardTarget AddPermanentTargetAndCard(Card card)
		{
			CardTarget target = HasTemporaryTarget(card) ? targets[card] : AddTemporaryTarget(card);
			page.AddCard(card, target);
			diajector.DequeOwner.Deque.lowerTarget.onMoveFrom.Invoke(card.Animator);
			return target;
		}
		
		public void RemovePermanentTargetAndCard(Card card)
		{
			page.RemoveCard(card);
			RemoveTemporaryTarget(card);
		}

		public void SyncCards()
		{
			if (!inventory)
			{
				foreach ((Card card, CardTarget _) in targets.ToList())
					RemovePermanentTargetAndCard(card);
				return;
			}
			
			foreach ((Card card, CardTarget _) in targets.Where(pair => !inventory.Contains(pair.Key)).ToList())
				RemovePermanentTargetAndCard(card);
			
			foreach (Card card in inventory.Where(card => !targets.ContainsKey(card)))
			{
				CardTarget target = AddPermanentTargetAndCard(card);
				if (card.Grabbable.isBeingHeld)
				{
					card.Animator.SetPausedAt(target.lerpTarget);
					target.onGrab.Invoke();
				}
				else
				{
					card.Animator.TeleportTo(diajector.DequeOwner.Deque.lowerTarget);
				}
			}
		}
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

namespace SBEPIS.Capturellection
{
	public class DiajectorCaptureLayout : MonoBehaviour
	{
		public CardTarget cardTargetPrefab;
		public float cardZ = -1;
		[FormerlySerializedAs("fetchableCardY")]
		public float fetchableCardZ = 0.1f;
		public Transform directionEndpoint;

		private Inventory inventory;
		private Diajector diajector;
		private readonly Dictionary<DequeStorable, CardTarget> targets = new();
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
		
		public void SetStorableParent(Inventory inventory)
		{
			this.inventory = inventory;
			inventory.SetStorableParent(transform);
		}
		
		private void TickAndLayoutTargets(float deltaTime)
		{
			inventory.Direction = directionEndpoint ? transform.InverseTransformPoint(directionEndpoint.position).normalized : Vector3.zero;
			inventory.Tick(deltaTime);
			Vector3 inventorySize = inventory.MaxPossibleSize;
			inventory.Position = transform.forward * (cardZ + inventorySize.z / 2);
			inventory.Rotation = Quaternion.identity;
			
			foreach ((DequeStorable card, CardTarget target) in targets)
			{
				inventory.LayoutTarget(card, target);
				target.transform.localRotation *= Quaternion.Euler(0, 180, 0);
				if (inventory.CanFetch(card))
					target.transform.position += -transform.forward * fetchableCardZ;
			}
		}
		
		public CardTarget AddTemporaryTarget(DequeStorable card)
		{
			if (targets.TryGetValue(card, out CardTarget target))
				throw new ArgumentException($"Tried to add a target of {card} to {this} but {target} already exists");
			
			CardTarget newTarget = Instantiate(cardTargetPrefab, transform);
			newTarget.Card = card;
			targets.Add(card, newTarget);
			return newTarget;
		}
		
		public void RemoveTemporaryTarget(DequeStorable card)
		{
			CardTarget target = targets[card];
			targets.Remove(card);
			Destroy(target.gameObject);
		}

		public bool HasTemporaryTarget(DequeStorable card) => targets.ContainsKey(card) && !inventory.Contains(card);

		public CardTarget AddPermanentTargetAndCard(DequeStorable card)
		{
			CardTarget target = HasTemporaryTarget(card) ? targets[card] : AddTemporaryTarget(card);
			page.AddCard(card, target);
			diajector.deque.lowerTarget.onMoveFrom.Invoke(card.Animator);
			return target;
		}
		
		public void RemovePermanentTargetAndCard(DequeStorable card)
		{
			page.RemoveCard(card);
			RemoveTemporaryTarget(card);
		}

		public void SyncCards()
		{
			if (!inventory)
			{
				foreach ((DequeStorable card, CardTarget _) in targets.ToList())
					RemovePermanentTargetAndCard(card);
				return;
			}
			
			foreach ((DequeStorable card, CardTarget _) in targets.Where(pair => !inventory.Contains(pair.Key)).ToList())
				RemovePermanentTargetAndCard(card);
			
			foreach (DequeStorable card in inventory.Where(card => !targets.ContainsKey(card)))
			{
				CardTarget target = AddPermanentTargetAndCard(card);
				if (card.Grabbable.IsBeingHeld)
				{
					card.Animator.SetPausedAt(target.LerpTarget);
					target.onGrab.Invoke();
				}
				else
				{
					card.Animator.TeleportTo(diajector.deque.lowerTarget);
				}
			}
		}
	}
}

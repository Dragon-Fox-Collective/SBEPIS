using System;
using System.Collections.Generic;
using System.Linq;
using KBCore.Refs;
using SBEPIS.Controller;
using UnityEngine;
using UnityEngine.Serialization;

namespace SBEPIS.Capturellection
{
	public class DiajectorCaptureLayout : ValidatedMonoBehaviour
	{
		[SerializeField, Parent(Flag.IncludeInactive)] private DiajectorPageCreator pageCreator;
		[SerializeField, Parent(Flag.IncludeInactive)] private DiajectorPage page;
		
		public Inventory inventory;
		public CardTarget cardTargetPrefab;
		public float cardZ = -1;
		[FormerlySerializedAs("fetchableCardY")]
		public float fetchableCardZ = 0.1f;
		public Transform directionEndpoint;
		
		private readonly Dictionary<InventoryStorable, CardTarget> targets = new();
		
		private void Update()
		{
			TickAndLayoutTargets(Time.deltaTime);
		}
		
		public void SetInventoryStorableParent(Inventory inventory)
		{
			inventory.SetStorableParent(transform);
		}
		
		private void TickAndLayoutTargets(float deltaTime)
		{
			inventory.Direction = directionEndpoint ? transform.InverseTransformPoint(directionEndpoint.position).normalized : Vector3.zero;
			inventory.Tick(deltaTime);
			Vector3 inventorySize = inventory.MaxPossibleSize;
			inventory.Position = transform.forward * (cardZ + inventorySize.z / 2);
			inventory.Rotation = Quaternion.identity;
			
			foreach ((InventoryStorable card, CardTarget target) in targets)
			{
				inventory.LayoutTarget(card, target);
				target.transform.localRotation *= Quaternion.Euler(0, 180, 0);
				if (inventory.CanFetch(card))
					target.transform.position += -transform.forward * fetchableCardZ;
			}
		}
		
		public CardTarget AddTemporaryTarget(InventoryStorable card)
		{
			if (targets.TryGetValue(card, out CardTarget target))
				throw new ArgumentException($"Tried to add a target of {card} to {this} but {target} already exists");
			
			CardTarget newTarget = Instantiate(cardTargetPrefab, transform);
			newTarget.Card = card.DequeElement;
			targets.Add(card, newTarget);
			return newTarget;
		}
		
		public void RemoveTemporaryTarget(InventoryStorable card)
		{
			CardTarget target = targets[card];
			targets.Remove(card);
			Destroy(target.gameObject);
		}

		public bool HasTemporaryTarget(InventoryStorable card) => targets.ContainsKey(card) && !inventory.Contains(card);

		public CardTarget AddPermanentTargetAndCard(InventoryStorable card)
		{
			CardTarget target = HasTemporaryTarget(card) ? targets[card] : AddTemporaryTarget(card);
			page.AddCard(card.DequeElement, target);
			pageCreator.StartTarget.onMoveFrom.Invoke(card.DequeElement.Animator);
			return target;
		}
		
		public void RemovePermanentTargetAndCard(InventoryStorable card)
		{
			page.RemoveCard(card.DequeElement);
			RemoveTemporaryTarget(card);
		}

		public void SyncCards()
		{
			if (!inventory)
			{
				foreach ((InventoryStorable card, CardTarget _) in targets.ToList())
					RemovePermanentTargetAndCard(card);
				return;
			}
			
			foreach ((InventoryStorable card, CardTarget _) in targets.Where(pair => !inventory.Contains(pair.Key)).ToList())
				RemovePermanentTargetAndCard(card);
			
			foreach (InventoryStorable card in inventory.Where(card => !targets.ContainsKey(card)))
			{
				CardTarget target = AddPermanentTargetAndCard(card);
				if (card.TryGetComponent(out Grabbable grabbable) && grabbable.IsBeingHeld)
				{
					card.DequeElement.Animator.SetPausedAt(target.LerpTarget);
					target.onGrab.Invoke();
				}
				else
				{
					card.DequeElement.Animator.TeleportTo(pageCreator.StartTarget);
				}
			}
		}
	}
}

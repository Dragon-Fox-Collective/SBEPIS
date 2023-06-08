using System;
using System.Collections.Generic;
using System.Linq;
using KBCore.Refs;
using SBEPIS.Controller;
using UnityEngine;

namespace SBEPIS.Capturellection
{
	public class DiajectorCaptureLayout : ValidatedMonoBehaviour
	{
		[SerializeField, Parent(Flag.IncludeInactive)] private DiajectorPageCreator pageCreator;
		[SerializeField, Parent(Flag.IncludeInactive)] private DiajectorPage page;
		
		[SerializeField, Anywhere] private Inventory inventory;
		public Inventory Inventory => inventory;
		[SerializeField, Anywhere] private CardTarget cardTargetPrefab;
		[SerializeField] private float cardZ = -1;
		[SerializeField] private float fetchableCardZ = 0.1f;
		[SerializeField, Anywhere] private Transform directionEndpoint;
		
		private readonly Dictionary<InventoryStorable, CardTarget> targets = new();
		
		private void Update()
		{
			LayoutTargets();
		}
		
		private void LayoutTargets()
		{
			inventory.Layout(directionEndpoint ? transform.InverseTransformPoint(directionEndpoint.position).normalized : Vector3.zero);
			
			Vector3 inventorySize = inventory.MaxPossibleSize;
			inventory.Position = Vector3.back * (cardZ + inventorySize.z / 2);
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
			if (targets.TryGetValue(card, out CardTarget oldTarget))
				throw new ArgumentException($"Tried to add a target of {card} to {this} but {oldTarget} already exists");
			
			CardTarget target = Instantiate(cardTargetPrefab, transform);
			target.Card = card.DequeElement;
			targets.Add(card, target);
			return target;
		}
		
		public void RemoveTemporaryTarget(InventoryStorable card)
		{
			targets.Remove(card, out CardTarget target);
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
		
		public void SyncRemoveOldCard(InventoryStorable card) => RemovePermanentTargetAndCard(card);
		public void SyncAddNewCard(InventoryStorable card)
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

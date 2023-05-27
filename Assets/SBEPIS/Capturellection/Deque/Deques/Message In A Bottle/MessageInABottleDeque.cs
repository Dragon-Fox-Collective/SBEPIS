using System.Collections.Generic;
using System.Linq;
using SBEPIS.Capturellection.Storage;
using SBEPIS.Utils;
using UnityEngine;

namespace SBEPIS.Capturellection.Deques
{
	public class MessageInABottleDeque : LaidOutDeque<LinearSettings, LinearLayout, MessageInABottleState>
	{
		[SerializeField] private GameObject bottlePrefab;
		
		public override bool CanFetch(MessageInABottleState state, InventoryStorable card) => true;
		
		public void RemoveBottle(MessageInABottleState state, Bottle bottle)
		{
			if (!state.Inventory.Contains(bottle.Slot))
				return;
			
			bottle.Page = bottle.Card.DequeElement.Page;
			
			bottle.SlotIndex = state.Inventory.IndexOf(bottle.Slot);
			
			bottle.Card.DequeElement.SetParent(null);
			bottle.Card.Inventory = null;
			
			state.Inventory.Remove(bottle.Slot);
			state.bottles.Remove(bottle.Slot);
			Destroy(bottle.Slot.gameObject);
			
			bottle.Layout.SyncCards();
		}
		
		public override IEnumerable<Storable> LoadStorableHook(MessageInABottleState state, Storable storable)
		{
			InventoryStorable sampleCard = storable.First();
			
			Bottle bottle = Instantiate(bottlePrefab).GetComponentInChildren<Bottle>();
			bottle.Card.DequeElement.SetParent(sampleCard.DequeElement.Parent);
			bottle.Card.Inventory = sampleCard.Inventory;
			bottle.Deque = this;
			bottle.OriginalStorable = storable;
			
			GameObject slotObject = new();
			bottle.Slot = slotObject.AddComponent<StorableSlot>();
			bottle.Slot.Parent = storable.Parent;
			bottle.Slot.name = "Bottle Slot";
			bottle.Slot.Load(bottle.Card);
			
			state.bottles.Add(bottle.Slot, bottle);
			
			CollisionTrigger collisionTrigger = bottle.GetComponent<CollisionTrigger>();
			void ReplaceBottle(float _)
			{
				collisionTrigger.onCollide.RemoveListener(ReplaceBottle);
				
				RemoveBottle(state, bottle);
				
				state.Inventory.Insert(bottle.SlotIndex, storable);
				
				bottle.Layout.SyncCards();
				bottle.Page.StartAssemblyForCards(storable.Select(card => card.DequeElement));
				
				Destroy(bottle.Root.gameObject);
			}
			collisionTrigger.onCollide.AddListener(ReplaceBottle);
			
			yield return bottle.Slot;
		}
		
		public override IEnumerable<Storable> SaveStorableHook(MessageInABottleState state, Storable slot)
		{
			state.bottles.Remove(slot, out Bottle bottle);
			Storable storable = bottle.OriginalStorable;
			Destroy(bottle.Slot.gameObject);
			Destroy(bottle.Root.gameObject);
			yield return storable;
		}
	}
	
	public class MessageInABottleState : InventoryState, DirectionState
	{
		public List<Storable> Inventory { get; set; } = new();
		public Vector3 Direction { get; set; }
		public readonly Dictionary<Storable, Bottle> bottles = new();
	}
}

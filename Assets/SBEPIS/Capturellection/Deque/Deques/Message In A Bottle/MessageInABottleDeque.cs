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
		
		public override IEnumerable<Storable> LoadStorableHook(MessageInABottleState state, Storable storable)
		{
			InventoryStorable sampleCard = storable.First();
			
			GameObject bottleObject = Instantiate(bottlePrefab);
			InventoryStorable bottle = bottleObject.GetComponentInChildren<InventoryStorable>();
			bottle.DequeElement.SetParent(sampleCard.DequeElement.Parent);
			bottle.Inventory = sampleCard.Inventory;
			
			GameObject slotObject = new();
			StorableSlot slot = slotObject.AddComponent<StorableSlot>();
			slot.Parent = storable.Parent;
			slot.name = "Bottle Slot";
			slot.Load(bottle);
			
			state.slots.Add(storable, slot);
			state.bottles.Add(slot, bottleObject);
			state.originalStorables.Add(slot, storable);
			
			CollisionTrigger collisionTrigger = bottle.GetComponent<CollisionTrigger>();
			void ReplaceBottle()
			{
				collisionTrigger.onCollide.RemoveListener(ReplaceBottle);

				DiajectorPage page = bottle.DequeElement.Page;
				
				bottle.DequeElement.SetParent(null);
				bottle.Inventory = null;

				int index = state.Inventory.IndexOf(slot);
				state.Inventory.Remove(slot);
				state.slots.Remove(storable);
				state.bottles.Remove(slot);
				state.originalStorables.Remove(slot);
				Destroy(slotObject);
				
				state.Inventory.Insert(index, storable);
				
				DiajectorCaptureLayout layout = bottle.DequeElement.Page.GetComponentInChildren<DiajectorCaptureLayout>();
				layout.SyncCards();
				page.StartAssemblyForCards(storable.Select(card => card.DequeElement));
			}
			collisionTrigger.onCollide.AddListener(ReplaceBottle);
			
			yield return slot;
		}
		
		public override IEnumerable<Storable> SaveStorableHook(MessageInABottleState state, Storable storable)
		{
			state.originalStorables.Remove(storable, out Storable originalStorable);
			state.slots.Remove(originalStorable, out StorableSlot slot);
			state.bottles.Remove(slot, out GameObject bottle);
			Destroy(slot.gameObject);
			Destroy(bottle);
			yield return originalStorable;
		}
	}
	
	public class MessageInABottleState : InventoryState, DirectionState
	{
		public List<Storable> Inventory { get; set; } = new();
		public Vector3 Direction { get; set; }
		public readonly Dictionary<Storable, Storable> originalStorables = new();
		public readonly Dictionary<Storable, StorableSlot> slots = new();
		public readonly Dictionary<StorableSlot, GameObject> bottles = new();
	}
}

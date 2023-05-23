using System.Collections.Generic;
using System.Linq;
using SBEPIS.Capturellection.Storage;
using UnityEngine;

namespace SBEPIS.Capturellection.Deques
{
	public class MessageInABottleDeque : LaidOutDeque<LinearSettings, LinearLayout, MessageInABottleState>
	{
		[SerializeField] private GameObject bottlePrefab;
		
		public override bool CanFetch(MessageInABottleState state, InventoryStorable card) => false;
		
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

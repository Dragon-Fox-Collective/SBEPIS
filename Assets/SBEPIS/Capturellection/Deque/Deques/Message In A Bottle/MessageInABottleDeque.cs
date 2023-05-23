using System.Collections.Generic;
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
			InventoryStorable bottle = Instantiate(bottlePrefab).GetComponentInChildren<InventoryStorable>();
			GameObject slotObject = new();
			StorableSlot slot = slotObject.AddComponent<StorableSlot>();
			slot.name = "Bottle Slot";
			slot.Load(bottle);
			state.originalStorables.Add(slot, storable);
			
			yield return slot;
		}
		
		public override IEnumerable<Storable> SaveStorableHook(MessageInABottleState state, Storable storable)
		{
			yield return state.originalStorables[storable];
		}
	}
	
	public class MessageInABottleState : InventoryState, DirectionState
	{
		public List<Storable> Inventory { get; set; } = new();
		public Vector3 Direction { get; set; }
		public readonly Dictionary<Storable, Storable> originalStorables = new();
	}
}

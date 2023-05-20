using System.Collections.Generic;
using SBEPIS.Capturellection.Storage;
using UnityEngine;

namespace SBEPIS.Capturellection.Deques
{
	public class MessageInABottleDeque : LaidOutDeque<LinearSettings, LinearLayout, MessageInABottleState>
	{
		[SerializeField] private GameObject bottlePrefab;
		
		public override bool CanFetch(MessageInABottleState state, InventoryStorable card) => false;
		
		public override IEnumerable<Storable> LoadCardPreHook(MessageInABottleState state, Storable storable)
		{
			InventoryStorable bottle = Instantiate(bottlePrefab).GetComponentInChildren<InventoryStorable>();
			state.OriginalStorables.Add(bottle, storable);
			
			GameObject slotObject = new();
			StorableSlot slot = slotObject.AddComponent<StorableSlot>();
			slot.name = "Bottle Slot";
			slot.Load(bottle);
			
			yield return slot;
		}
		
		public override InventoryStorable SaveCardPostHook(MessageInABottleState state, InventoryStorable card)
		{
			return state.OriginalStorables[card];
		}
	}
	
	public class MessageInABottleState : InventoryState, DirectionState
	{
		public List<Storable> Inventory { get; set; } = new();
		public Vector3 Direction { get; set; }
		public Dictionary<InventoryStorable, Storable> OriginalStorables { get; } = new();
	}
}

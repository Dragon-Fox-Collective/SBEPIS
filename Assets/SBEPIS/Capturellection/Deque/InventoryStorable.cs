using KBCore.Refs;
using SBEPIS.Utils;
using UnityEngine;

namespace SBEPIS.Capturellection
{
	[RequireComponent(typeof(DequeElement))]
	public class InventoryStorable : ValidatedMonoBehaviour
	{
		[SerializeField, Self] private DequeElement dequeElement;
		public DequeElement DequeElement => dequeElement;
		
		public EventProperty<InventoryStorable, Inventory, SetCardInventoryEvent, UnsetCardInventoryEvent> inventoryEvents = new();
		public Inventory Inventory
		{
			get => inventoryEvents.Get();
			set => inventoryEvents.Set(this, value);
		}
		
		private void Awake()
		{
			inventoryEvents.onSet.AddListener((_, inventory) => dequeElement.Deque = inventory.deque);
			inventoryEvents.onUnset.AddListener((_, _, _) => dequeElement.Deque = null);
		}
	}
}
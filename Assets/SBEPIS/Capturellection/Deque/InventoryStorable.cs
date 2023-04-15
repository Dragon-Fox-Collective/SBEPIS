using System;
using SBEPIS.Utils;
using UnityEngine;
using UnityEngine.Serialization;

namespace SBEPIS.Capturellection
{
	[RequireComponent(typeof(DequeElement))]
	public class InventoryStorable : MonoBehaviour
	{
		public DequeElement DequeElement { get; private set; }
		
		public EventProperty<InventoryStorable, Inventory, SetCardInventoryEvent, UnsetCardInventoryEvent> inventoryEvents = new();
		public Inventory Inventory
		{
			get => inventoryEvents.Get();
			set => inventoryEvents.Set(this, value);
		}

		private void Awake()
		{
			DequeElement = GetComponent<DequeElement>();
			inventoryEvents.onSet.AddListener((_, inventory) => DequeElement.Deque = inventory.deque);
			inventoryEvents.onUnset.AddListener((_, _, _) => DequeElement.Deque = null);
		}
	}
}
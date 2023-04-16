using System;
using KBCore.Refs;
using SBEPIS.Utils;
using UnityEngine;
using UnityEngine.Serialization;

namespace SBEPIS.Capturellection
{
	[RequireComponent(typeof(DequeElement))]
	public class InventoryStorable : MonoBehaviour
	{
		[SerializeField, Self]
		private DequeElement dequeElement;
		public DequeElement DequeElement => dequeElement;
		
		private void OnValidate() => this.ValidateRefs();
		
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
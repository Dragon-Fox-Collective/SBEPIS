using System;
using Cysharp.Threading.Tasks;
using KBCore.Refs;
using SBEPIS.Capturellection.Storage;
using SBEPIS.Utils;
using UnityEngine;
using UnityEngine.Events;

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
		
		public UnityEvent OnSyncAdded = new();
		public UnityEvent OnSyncRemoved = new();
		
		public bool IsBeingFetched { get; set; }
		
		private void Awake()
		{
			inventoryEvents.onSet.AddListener((_, inventory) => dequeElement.Deque = inventory.deque);
			inventoryEvents.onUnset.AddListener((_, _, _) => dequeElement.Deque = null);
		}
		
		public UniTask Interact<TState>(DequeRuleset targetRuleset, DequeInteraction<TState> action) => Inventory.Interact(this, targetRuleset, action);
		public UniTask Interact<TState>(DequeRuleset targetRuleset, Action<TState, InventoryStorable> action) => Interact<TState>(targetRuleset, (state, card) => { action(state, card); return UniTask.CompletedTask; });
	}
}
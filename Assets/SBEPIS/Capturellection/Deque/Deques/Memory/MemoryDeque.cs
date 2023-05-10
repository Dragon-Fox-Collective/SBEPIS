using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using SBEPIS.Capturellection.Storage;
using UnityEngine;

namespace SBEPIS.Capturellection.Deques
{
	public class MemoryDeque : LaidOutDeque<FlippedGridLayout, MemoryState>
	{
		[SerializeField] private ProxyCaptureContainer memoryCardPrefab;
		
		public override UniTask<DequeStoreResult> StoreItemHook(List<Storable> inventory, MemoryState state, Capturellectable item, DequeStoreResult oldResult)
		{
			inventory.Shuffle();
			return base.StoreItemHook(inventory, state, item, oldResult);
		}
		
		public override async UniTask<Capturellectable> FetchItem(List<Storable> inventory, MemoryState state, InventoryStorable card)
		{
			Storable storable = StorableWithCard(inventory, card);
			if (!state.FlippedStorable && !storable.HasAllCardsEmpty)
			{
				state.FlippedStorable = storable;
				state.wasFlippedThisAttempt = true;
				return null;
			}
			
			if (storable == state.FlippedPair)
				return await storable.FetchItem(card);
			
			return null;
		}
		
		public override UniTask<Capturellectable> FetchItemHook(List<Storable> inventory, MemoryState state, InventoryStorable card, Capturellectable oldItem)
		{
			if (!state.wasFlippedThisAttempt) state.FlippedStorable = null;
			state.wasFlippedThisAttempt = false;
			return base.FetchItemHook(inventory, state, card, oldItem);
		}
		
		public override IEnumerable<Storable> LoadCardPreHook(List<Storable> inventory, MemoryState state, Storable storable)
		{
			print($"Instantiating to {storable.transform.parent}");
			Dictionary<InventoryStorable, List<ProxyCaptureContainer>> proxies = new();
			(Storable, Storable) newStorables = (InstantiateStorable(state, storable, proxies), InstantiateStorable(state, storable, proxies));
			state.pairs.Add(newStorables.Item1, newStorables.Item2);
			state.pairs.Add(newStorables.Item2, newStorables.Item1);
			return ExtensionMethods.EnumerableOf(newStorables.Item1, newStorables.Item2);
		}
		private Storable InstantiateStorable(MemoryState state, Storable storable, Dictionary<InventoryStorable, List<ProxyCaptureContainer>> proxies)
		{
			Storable newStorable = StorableGroupDefinition.GetNewStorable(storable is StorableGroup storableGroup ? storableGroup.definition : null);
			newStorable.transform.SetParent(storable.transform.parent);
			
			newStorable.Load(storable.Select(card => InstantiateCard(state, card, proxies.GetEnsured(card))).ToList());
			return newStorable;
		}
		private InventoryStorable InstantiateCard(MemoryState state, InventoryStorable card, List<ProxyCaptureContainer> proxies)
		{
			ProxyCaptureContainer proxy = Instantiate(memoryCardPrefab, card.transform.parent);
			proxy.realContainer = card.GetComponent<CaptureContainer>();
			proxy.otherProxies = proxies;
			proxies.Add(proxy);
			
			InventoryStorable newCard = proxy.GetComponent<InventoryStorable>();
			newCard.Inventory = card.Inventory;
			
			FlipTracker flipTracker = newCard.GetComponent<FlipTracker>();
			flipTracker.Flip(state, false);
			state.flipTrackers.Add(newCard, flipTracker);
			
			return newCard;
		}
		
		public override void LoadCardPostHook(List<Storable> inventory, MemoryState state, Storable storable)
		{
			inventory.Shuffle();
		}
		
		public override InventoryStorable SaveCardHook(List<Storable> inventory, MemoryState state, InventoryStorable card)
		{
			inventory.Shuffle();
			
			state.flipTrackers.Remove(card);
			
			Storable storable = inventory.Find(storable => storable.Contains(card));
			state.pairs.Remove(storable);
			
			ProxyCaptureContainer proxy = card.GetComponent<ProxyCaptureContainer>();
			return proxy.otherProxies[0] == proxy ? proxy.realContainer.GetComponent<InventoryStorable>() : null;
		}
	}
	
	[Serializable]
	public class MemoryState : DirectionState, FlippedState
	{
		public Vector3 Direction { get; set; }
		private Storable flippedStorable;
		public Storable FlippedStorable
		{
			get => flippedStorable;
			set
			{
				if (flippedStorable) flippedStorable.ForEach(card => flipTrackers[card].Flip(this, false));
				flippedStorable = value;
				if (flippedStorable) flippedStorable.ForEach(card => flipTrackers[card].Flip(this, true));
			}
		}
		public bool wasFlippedThisAttempt;
		public Dictionary<Storable, Storable> pairs = new();
		public Storable FlippedPair => FlippedStorable ? pairs[FlippedStorable] : null;
		public Dictionary<InventoryStorable, FlipTracker> flipTrackers = new();
	}
}

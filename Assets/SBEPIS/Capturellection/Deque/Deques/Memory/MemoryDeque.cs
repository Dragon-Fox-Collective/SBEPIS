using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using KBCore.Refs;
using SBEPIS.Capturellection.Storage;
using UnityEngine;

namespace SBEPIS.Capturellection.Deques
{
	public class MemoryDeque : LaidOutDeque<FlippedGridSettings, FlippedGridLayout, MemoryState>
	{
		[SerializeField, Anywhere] private GameObject memoryCardPrefab;
		
		private void OnValidate() => this.ValidateRefs();
		
		public override bool CanFetch(MemoryState state, InventoryStorable card)
		{
			Storable storable = StorableWithCard(state, card);
			return storable.CanFetch(card) && (storable.HasAllCardsEmpty || (state.FlippedStorables.Contains(storable) && state.FlippedStorables.Contains(state.pairs[storable])));
		}
		
		public override UniTask<DequeStoreResult> StoreItemHook(MemoryState state, Capturellectable item, DequeStoreResult oldResult)
		{
			state.Inventory.Shuffle();
			return base.StoreItemHook(state, item, oldResult);
		}
		
		public override async UniTask<Capturellectable> FetchItem(MemoryState state, InventoryStorable card)
		{
			Storable storable = Flip(state, card);
			if (storable != null)
			{
				state.ClearFlippedStorables();
				return await storable.FetchItem(card);
			}
			
			return null;
		}
		
		public Storable Flip(MemoryState state, InventoryStorable card)
		{
			Storable storable = StorableWithCard(state, card);
			
			if (state.FlippedStorables.Contains(storable))
				return state.FlippedStorables.Contains(state.pairs[storable]) ? storable : null;
			
			if (state.FlippedStorables.Count >= 2 && !storable.HasAllCardsEmpty)
				state.ClearFlippedStorables();
			
			if (state.FlippedStorables.Count < 2 && !storable.HasAllCardsEmpty)
			{
				state.AddFlippedStorable(storable);
				return null;
			}
			
			return null;
		}
		
		public override IEnumerable<Storable> LoadStorableHook(MemoryState state, Storable storable)
		{
			(Storable, Storable) newStorables = (InstantiateStorable(storable), InstantiateStorable(storable));
			state.pairs.Add(newStorables.Item1, newStorables.Item2);
			state.pairs.Add(newStorables.Item2, newStorables.Item1);
			return ExtensionMethods.EnumerableOf(newStorables.Item1, newStorables.Item2);
		}
		private Storable InstantiateStorable(Storable storable)
		{
			Storable newStorable = storable.GetNewStorableLikeThis();
			newStorable.Parent = storable.Parent;
			newStorable.Load(storable.ToList());
			return newStorable;
		}
		private InventoryStorable InstantiateCard(MemoryState state, InventoryStorable card, List<ProxyCaptureContainer> proxies)
		{
			MemoryDequeCard newCard = Instantiate(memoryCardPrefab).GetComponentInChildren<MemoryDequeCard>();
			newCard.Card.DequeElement.SetParent(card.DequeElement.Parent);
			
			newCard.Deque = this;
			
			newCard.Proxy.realContainer = card.GetComponent<CaptureContainer>();
			newCard.Proxy.otherProxies = proxies;
			proxies.Add(newCard.Proxy);
			
			newCard.Card.Inventory = card.Inventory;
			
			newCard.FlipTracker.Flip(state, false);
			state.flipTrackers.Add(newCard.Card, newCard.FlipTracker);
			
			return newCard.Card;
		}
		
		public override IEnumerable<Storable> SaveStorableHook(MemoryState state, Storable storable)
		{
			state.Inventory.Shuffle();
			
			state.flipTrackers.Remove(card);
			
			state.pairs.Remove(storable);
			
			ProxyCaptureContainer proxy = card.GetComponent<ProxyCaptureContainer>();
			return proxy.otherProxies[0] == proxy ? proxy.realContainer.GetComponent<InventoryStorable>() : null;
		}
	}
	
	[Serializable]
	public class MemoryState : InventoryState, DirectionState, FlippedState
	{
		public List<Storable> Inventory { get; set; } = new();
		public Vector3 Direction { get; set; }
		public List<Storable> FlippedStorables { get; } = new();
		public Dictionary<Storable, Storable> pairs = new();
		public Dictionary<InventoryStorable, FlipTracker> flipTrackers = new();
		
		public void AddFlippedStorable(Storable storable)
		{
			storable.ForEach(card => flipTrackers[card].Flip(this, true));
			FlippedStorables.Add(storable);
		}
		
		public void ClearFlippedStorables()
		{
			FlippedStorables.ForEach(storable => storable.ForEach(card => flipTrackers[card].Flip(this, false)));
			FlippedStorables.Clear();
		}
	}
}

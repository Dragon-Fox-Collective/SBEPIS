using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using KBCore.Refs;
using SBEPIS.Capturellection.Storage;
using UnityEngine;

namespace SBEPIS.Capturellection.Deques
{
	public class MemoryDeque : LaidOutRuleset<FlippedGridSettings, FlippedGridLayout, MemoryState>
	{
		[SerializeField, Anywhere] private GameObject memoryCardPrefab;
		
		private void OnValidate() => this.ValidateRefs();
		
		protected override bool CanFetch(MemoryState state, InventoryStorable card)
		{
			Storable storable = StorableWithCard(state, card);
			return storable.CanFetch(card) && (storable.HasAllCardsEmpty || (state.FlippedStorables.Contains(storable) && state.FlippedStorables.Contains(state.Pairs[storable])));
		}
		
		protected override UniTask<StoreResult> StoreItemHook(MemoryState state, Capturellectable item, StoreResult oldResult)
		{
			state.Inventory.Shuffle();
			return base.StoreItemHook(state, item, oldResult);
		}
		
		protected override async UniTask<FetchResult> FetchItem(MemoryState state, InventoryStorable card)
		{
			Storable storable = Flip(state, card);
			if (storable != null)
			{
				state.ClearFlippedStorables();
				return await storable.FetchItem(card);
			}
			
			return new FetchResult(null);
		}
		
		public Storable Flip(MemoryState state, InventoryStorable card)
		{
			Storable storable = StorableWithCard(state, card);
			
			if (state.FlippedStorables.Contains(storable))
				return state.FlippedStorables.Contains(state.Pairs[storable]) ? storable : null;
			
			if (state.FlippedStorables.Count >= 2 && !storable.HasAllCardsEmpty)
				state.ClearFlippedStorables();
			
			if (state.FlippedStorables.Count < 2 && !storable.HasAllCardsEmpty)
			{
				state.AddFlippedStorable(storable);
				return null;
			}
			
			return null;
		}
		
		protected override IEnumerable<Storable> LoadStorableHook(MemoryState state, Storable storable)
		{
			Dictionary<InventoryStorable, List<ProxyCaptureContainer>> proxies = new();
			(Storable, Storable) newStorables = (InstantiateStorable(state, storable, proxies), InstantiateStorable(state, storable, proxies));
			state.Pairs.Add(newStorables.Item1, newStorables.Item2);
			state.Pairs.Add(newStorables.Item2, newStorables.Item1);
			return ExtensionMethods.EnumerableOf(newStorables.Item1, newStorables.Item2);
		}
		private Storable InstantiateStorable(MemoryState state, Storable storable, Dictionary<InventoryStorable, List<ProxyCaptureContainer>> proxies)
		{
			Storable newStorable = storable.GetNewStorableLikeThis();
			newStorable.Parent = storable.Parent;
			newStorable.LoadInit(storable.Select(card => InstantiateCard(state, card, proxies.GetEnsured(card))).ToList());
			state.OriginalStorables.Add(newStorable, storable);
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
			state.FlipTrackers.Add(newCard.Card, newCard.FlipTracker);
			
			return newCard.Card;
		}
		
		protected override IEnumerable<Storable> SaveStorableHook(MemoryState state, Storable storable)
		{
			storable.ForEach(card => state.FlipTrackers.Remove(card));
			state.Pairs.Remove(storable);
			state.OriginalStorables.Remove(storable, out Storable originalStorable);
			yield return originalStorable;
		}
	}
	
	[Serializable]
	public class MemoryState : InventoryState, DirectionState, FlippedState
	{
		public CallbackList<Storable> Inventory { get; set; } = new();
		public Vector3 Direction { get; set; }
		public List<Storable> FlippedStorables { get; } = new();
		public readonly Dictionary<Storable, Storable> Pairs = new();
		public readonly Dictionary<InventoryStorable, FlipTracker> FlipTrackers = new();
		public readonly Dictionary<Storable, Storable> OriginalStorables = new();
		
		public void AddFlippedStorable(Storable storable)
		{
			storable.ForEach(card => FlipTrackers[card].Flip(this, true));
			FlippedStorables.Add(storable);
		}
		
		public void ClearFlippedStorables()
		{
			FlippedStorables.ForEach(storable => storable.ForEach(card => FlipTrackers[card].Flip(this, false)));
			FlippedStorables.Clear();
		}
	}
}

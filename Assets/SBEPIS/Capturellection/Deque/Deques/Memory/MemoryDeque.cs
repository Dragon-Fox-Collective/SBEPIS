using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using SBEPIS.Capturellection.Storage;
using UnityEngine;

namespace SBEPIS.Capturellection.Deques
{
	public class MemoryDeque : SingleDeque<FlippedGridLayout, MemoryState>
	{
		[SerializeField] private ProxyCaptureContainer memoryCardPrefab;
		[SerializeField] private FlippedGridLayout layout;
		
		public override void Tick(List<Storable> inventory, MemoryState state, float deltaTime) => layout.Tick(inventory, state, deltaTime);
		
		public override UniTask<DequeStoreResult> StoreItemHook(List<Storable> inventory, MemoryState state, Capturellectable item, DequeStoreResult oldResult)
		{
			inventory.Shuffle();
			return base.StoreItemHook(inventory, state, item, oldResult);
		}
		
		public override async UniTask<Capturellectable> FetchItem(List<Storable> inventory, MemoryState state, InventoryStorable card)
		{
			Storable storable = inventory.Find(storable => storable.Contains(card));
			if (!state.FlippedStorable)
			{
				state.FlippedStorable = storable;
				return null;
			}
			else
			{
				state.FlippedStorable = null;
				if (storable == state.FlippedPair)
				{
					Capturellectable item = await storable.FetchItem(card);
					return item;
				}
				else
					return null;
			}
		}
		
		public override UniTask<Capturellectable> FetchItemHook(List<Storable> inventory, MemoryState state, InventoryStorable card, Capturellectable oldItem)
		{
			inventory.Shuffle();
			return base.FetchItemHook(inventory, state, card, oldItem);
		}
		
		public override IEnumerable<Storable> LoadCardPreHook(List<Storable> inventory, MemoryState state, Storable storable)
		{
			print($"Instantiating to {storable.transform.parent}");
			Dictionary<InventoryStorable, List<ProxyCaptureContainer>> proxies = new();
			(Storable, Storable) newStorables = (InstantiateStorable(storable, proxies), InstantiateStorable(storable, proxies));
			state.pairs.Add(newStorables.Item1, newStorables.Item2);
			state.pairs.Add(newStorables.Item2, newStorables.Item1);
			return ExtensionMethods.EnumerableOf(newStorables.Item1, newStorables.Item2);
		}
		private Storable InstantiateStorable(Storable storable, Dictionary<InventoryStorable, List<ProxyCaptureContainer>> proxies)
		{
			Storable newStorable = StorableGroupDefinition.GetNewStorable(storable is StorableGroup storableGroup ? storableGroup.definition : null);
			newStorable.transform.SetParent(storable.transform.parent);
			
			newStorable.Load(storable.Select(card => InstantiateCard(card, proxies.GetEnsured(card))).ToList());
			return newStorable;
		}
		private InventoryStorable InstantiateCard(InventoryStorable card, List<ProxyCaptureContainer> proxies)
		{
			ProxyCaptureContainer proxy = Instantiate(memoryCardPrefab, card.transform.parent);
			proxy.RealContainer = card.GetComponent<CaptureContainer>();
			proxy.OtherProxies = proxies;
			proxies.Add(proxy);
			
			InventoryStorable newCard = proxy.GetComponent<InventoryStorable>();
			newCard.Inventory = card.Inventory;
			return newCard;
		}
		
		public override void LoadCardPostHook(List<Storable> inventory, MemoryState state, Storable storable)
		{
			inventory.Shuffle();
		}
		
		public override InventoryStorable SaveCardHook(List<Storable> inventory, MemoryState state, InventoryStorable card)
		{
			inventory.Shuffle();
			
			Storable storable = inventory.Find(storable => storable.Contains(card));
			state.pairs.Remove(storable);
			
			ProxyCaptureContainer proxy = card.GetComponent<ProxyCaptureContainer>();
			return proxy.OtherProxies[0] == proxy ? proxy.RealContainer.GetComponent<InventoryStorable>() : null;
		}
		
		protected override FlippedGridLayout SettingsPageLayoutData => layout;
	}
	
	[Serializable]
	public class MemoryState : FlippedGridState
	{
		public Dictionary<Storable, Storable> pairs = new();
		public Storable FlippedPair => FlippedStorable ? pairs[FlippedStorable] : null;
	}
}

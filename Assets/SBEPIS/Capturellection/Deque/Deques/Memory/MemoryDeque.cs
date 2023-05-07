using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using SBEPIS.Capturellection.Storage;
using UnityEngine;

namespace SBEPIS.Capturellection.Deques
{
	public class MemoryDeque : DequeBase<MemoryState>
	{
		public bool offsetXFromEnd = false;
		public float offsetX = 0.05f;
		public bool offsetYFromEnd = false;
		public float offsetY = 0.05f;
		public ProxyCaptureContainer memoryCardPrefab;
		
		public override void Tick(List<Storable> inventory, MemoryState state, float deltaTime)
		{
			foreach (Storable storable in inventory)
			{
				storable.state.Direction = Quaternion.Euler(0, 0, -60) * state.Direction;
				storable.Tick(deltaTime);
			}

			List<Vector3> sizes = inventory.Select(storable => storable.MaxPossibleSize).ToList();
			Vector3 absDirection = state.Direction.Select(Mathf.Abs);
			float lengthSum = offsetXFromEnd ?
				-offsetX * (inventory.Count - 1) + sizes.Select(size => Vector3.Project(size, absDirection)).Aggregate(ExtensionMethods.Add).magnitude :
				offsetX * (inventory.Count - 1);
			
			Vector3 right = -lengthSum / 2 * state.Direction;
			foreach ((Storable storable, Vector3 size) in inventory.Zip(sizes))
			{
				float length = Vector3.Project(size, absDirection).magnitude;
				right += state.Direction * (offsetXFromEnd ? length / 2 : 0);
				
				storable.Position = right;
				storable.Rotation = ArrayDeque.GetOffsetRotation(state.Direction);
				
				right += state.Direction * (offsetX + (offsetXFromEnd ? length / 2 : 0));
			}
		}
		
		public override UniTask<DequeStoreResult> StoreItemHook(List<Storable> inventory, MemoryState state, Capturellectable item, DequeStoreResult oldResult)
		{
			inventory.Shuffle();
			return base.StoreItemHook(inventory, state, item, oldResult);
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
			ProxyCaptureContainer proxy = card.GetComponent<ProxyCaptureContainer>();
			
			inventory.Shuffle();
		}
	}
}

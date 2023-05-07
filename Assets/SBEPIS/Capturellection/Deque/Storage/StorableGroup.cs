using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace SBEPIS.Capturellection.Storage
{
	public class StorableGroup : Storable
	{
		public StorableGroupDefinition definition;
		public List<Storable> inventory = new();
		
		public override Vector3 MaxPossibleSize => definition.Ruleset.GetMaxPossibleSizeOf(inventory, state);
		
		public override int InventoryCount => inventory.Count;
		
		public override bool HasNoCards => inventory.Count == 0;
		public override bool HasAllCards => inventory.Count == definition.MaxStorables && inventory.All(storable => storable.HasAllCards);
		
		public override bool HasAllCardsEmpty => inventory.All(storable => storable.HasAllCardsEmpty);
		public override bool HasAllCardsFull => inventory.All(storable => storable.HasAllCardsFull);

		public override void Tick(float deltaTime) => definition.Ruleset.Tick(inventory, state, deltaTime);
		public override void LayoutTarget(InventoryStorable card, CardTarget target) => inventory.Find(storable => storable.Contains(card)).LayoutTarget(card, target);
		
		public override bool CanFetch(InventoryStorable card) => definition.Ruleset.CanFetchFrom(inventory, state, card);
		public override bool Contains(InventoryStorable card) => inventory.Any(storable => storable.Contains(card));
		
		public override async UniTask<StorableStoreResult> StoreItem(Capturellectable item)
		{
			DequeStoreResult res = await definition.Ruleset.StoreItem(inventory, state, item);
			res = await definition.Ruleset.StoreItemHook(inventory, state, item, res);
			
			if (res.ejectedItem && res.ejectedItem.TryGetComponent(out InventoryStorable flushedCard))
			{
				List<InventoryStorable> cards = new(){ flushedCard };
				await FlushCards(cards, res.flushIndex);
				if (cards.Count == 0)
					res.ejectedItem = null;
			}
			
			return res.ToStorableResult();
		}

		public override async UniTask<Capturellectable> FetchItem(InventoryStorable card)
		{
			Capturellectable item = await definition.Ruleset.FetchItem(inventory, state, card);
			item = await definition.Ruleset.FetchItemHook(inventory, state, card, item);
			return item;
		}
		
		public override async UniTask FlushCards(List<InventoryStorable> cards) => await FlushCards(cards, 0);
		private async UniTask FlushCards(List<InventoryStorable> cards, int originalIndex)
		{
			if (HasAllCards || cards.Count == 0)
				return;
			
			foreach (Storable storable in inventory.Skip(originalIndex).Concat(inventory.Take(originalIndex)))
			{
				await storable.FlushCards(cards);
				if (cards.Count == 0)
					break;
			}
			
			while (inventory.Count < definition.MaxStorables)
			{
				Storable storable = StorableGroupDefinition.GetNewStorable(definition.Subdefinition);
				storable.transform.SetParent(transform);
				await storable.FlushCards(cards);
				
				IEnumerable<Storable> hookedStorables = await definition.Ruleset.FlushCardPreHook(inventory, state, storable);
				foreach (Storable hookedStorable in hookedStorables)
				{
					await definition.Ruleset.FlushCard(inventory, state, hookedStorable);
					await definition.Ruleset.FlushCardPostHook(inventory, state, hookedStorable);
				}

				if (cards.Count == 0)
					break;
			}
		}
		
		public override async UniTask<InventoryStorable> FetchCard(InventoryStorable card)
		{
			card = await definition.Ruleset.FetchCard(inventory, state, card);
			card = await definition.Ruleset.FetchCardHook(inventory, state, card);
			return card;
		}
		
		public override void Load(List<InventoryStorable> cards)
		{
			if (HasAllCards || cards.Count == 0)
				return;
			
			foreach (Storable storable in inventory)
			{
				storable.Load(cards);
				if (cards.Count == 0)
					break;
			}
			
			while (inventory.Count < definition.MaxStorables)
			{
				Storable storable = StorableGroupDefinition.GetNewStorable(definition.Subdefinition);
				storable.transform.SetParent(transform);
				storable.Load(cards);
				
				IEnumerable<Storable> hookedStorables = definition.Ruleset.LoadCardPreHook(inventory, state, storable);
				foreach (Storable hookedStorable in hookedStorables)
				{
					inventory.Add(hookedStorable);
					definition.Ruleset.LoadCardPostHook(inventory, state, hookedStorable);
				}

				if (cards.Count == 0)
					break;
			}
		}
		public override void Save(List<InventoryStorable> cards)
		{
			if (HasNoCards)
				return;

			foreach (Storable storable in inventory.ToList())
			{
				storable.Save(cards);
				inventory.Remove(storable);
				definition.Ruleset.SaveCardHook(inventory, state, card);
				Destroy(storable);
			}
		}
		
		public override IEnumerable<Texture2D> GetCardTextures(InventoryStorable card, IEnumerable<IEnumerable<Texture2D>> textures, int indexOfThisInParent)
		{
			if (Contains(card))
			{
				textures = textures.Append(definition.Ruleset.GetCardTextures());
				int index = inventory.FindIndex(storable => storable.Contains(card));
				Storable storable = inventory[index];
				return storable.GetCardTextures(card, textures, index);
			}
			else
				return definition.Ruleset.GetCardTextures().ToList();
		}
		
		public override IEnumerator<InventoryStorable> GetEnumerator() => inventory.SelectMany(storable => storable).GetEnumerator();
	}
}

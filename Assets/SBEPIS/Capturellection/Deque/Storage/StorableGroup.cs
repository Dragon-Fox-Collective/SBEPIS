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
		
		public override Vector3 MaxPossibleSize => definition.ruleset.GetMaxPossibleSizeOf(inventory, state);
		
		public override int InventoryCount => inventory.Count;
		
		public override bool HasNoCards => inventory.Count == 0;
		public override bool HasAllCards => inventory.Count == definition.maxStorables && inventory.All(storable => storable.HasAllCards);
		
		public override bool HasAllCardsEmpty => inventory.All(storable => storable.HasAllCardsEmpty);
		public override bool HasAllCardsFull => inventory.All(storable => storable.HasAllCardsFull);

		public override void Tick(float deltaTime) => definition.ruleset.Tick(inventory, state, deltaTime);
		public override void LayoutTarget(DequeStorable card, CardTarget target) => inventory.Find(storable => storable.Contains(card)).LayoutTarget(card, target);
		
		public override bool CanFetch(DequeStorable card) => definition.ruleset.CanFetchFrom(inventory, state, card);
		public override bool Contains(DequeStorable card) => inventory.Any(storable => storable.Contains(card));
		
		public override async UniTask<(DequeStorable, Capturellectainer, Capturellectable)> Store(Capturellectable item)
		{
			int storeIndex = await definition.ruleset.GetIndexToStoreInto(inventory, state);
			Storable storable = inventory[storeIndex];
			inventory.Remove(storable);
			
			(DequeStorable card, Capturellectainer container, Capturellectable ejectedItem) = await storable.Store(item);
			int restoreIndex = await definition.ruleset.GetIndexToInsertBetweenAfterStore(inventory, state, storable, storeIndex);
			inventory.Insert(restoreIndex, storable);
			
			if (ejectedItem && ejectedItem.TryGetComponent(out DequeStorable flushedCard))
			{
				List<DequeStorable> cards = new(){ flushedCard };
				await Flush(cards, storeIndex);
				if (cards.Count == 0)
					ejectedItem = null;
			}
			
			return (card, container, ejectedItem);
		}
		
		public override async UniTask<Capturellectable> Fetch(DequeStorable card)
		{
			Storable storable = inventory.First(storable => storable.Contains(card));
			int fetchIndex = inventory.IndexOf(storable);
			inventory.Remove(storable);
			
			Capturellectable item = await storable.Fetch(card);
			int restoreIndex = await definition.ruleset.GetIndexToInsertBetweenAfterFetch(inventory, state, storable, fetchIndex);
			inventory.Insert(restoreIndex, storable);
			
			return item;
		}
		
		public override async UniTask Flush(List<DequeStorable> cards) => await Flush(cards, 0);
		public async UniTask Flush(List<DequeStorable> cards, int originalIndex)
		{
			if (HasAllCards || cards.Count == 0)
				return;
			
			foreach (Storable storable in inventory.Skip(originalIndex).Concat(inventory.Take(originalIndex)))
			{
				await storable.Flush(cards);
				if (cards.Count == 0)
					break;
			}
			
			while (inventory.Count < definition.maxStorables)
			{
				Storable storable = StorableGroupDefinition.GetNewStorable(definition.subdefinition);
				storable.transform.SetParent(transform);
				await storable.Flush(cards);
				
				int insertIndex = await definition.ruleset.GetIndexToFlushBetween(inventory, state, storable);
				inventory.Insert(insertIndex, storable);
				
				if (cards.Count == 0)
					break;
			}
		}
		
		public override void Load(List<DequeStorable> cards)
		{
			if (HasAllCards || cards.Count == 0)
				return;
			
			foreach (Storable storable in inventory)
			{
				storable.Load(cards);
				if (cards.Count == 0)
					break;
			}
			
			while (inventory.Count < definition.maxStorables)
			{
				Storable storable = StorableGroupDefinition.GetNewStorable(definition.subdefinition);
				storable.transform.SetParent(transform);
				storable.Load(cards);
				
				inventory.Add(storable);
				
				if (cards.Count == 0)
					break;
			}
		}
		public override void Save(List<DequeStorable> cards)
		{
			if (HasNoCards)
				return;

			foreach (Storable storable in inventory.ToList())
			{
				storable.Save(cards);
				inventory.Remove(storable);
				Destroy(storable);
			}
		}
		
		public override IEnumerable<Texture2D> GetCardTextures(DequeStorable card, IEnumerable<IEnumerable<Texture2D>> textures, int indexOfThisInParent)
		{
			if (Contains(card))
			{
				textures = textures.Append(definition.ruleset.GetCardTextures());
				int index = inventory.FindIndex(storable => storable.Contains(card));
				Storable storable = inventory[index];
				return storable.GetCardTextures(card, textures, index);
			}
			else
				return definition.ruleset.GetCardTextures().ToList();
		}
		
		public override IEnumerator<DequeStorable> GetEnumerator() => inventory.SelectMany(storable => storable).GetEnumerator();
	}
}

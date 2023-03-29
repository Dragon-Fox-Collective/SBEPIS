using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace SBEPIS.Capturllection
{
	public class StorableGroup : Storable
	{
		public StorableGroupDefinition definition;
		public List<Storable> inventory = new();
		
		public override Vector3 maxPossibleSize => definition.ruleset.GetMaxPossibleSizeOf(inventory, state);
		
		public override int inventoryCount => inventory.Count;
		
		public override bool hasNoCards => inventory.Count == 0;
		public override bool hasAllCards => inventory.Count == definition.maxStorables && inventory.All(storable => storable.hasAllCards);
		
		public override bool hasAllCardsEmpty => inventory.All(storable => storable.hasAllCardsEmpty);
		public override bool hasAllCardsFull => inventory.All(storable => storable.hasAllCardsFull);

		public override void Tick(float deltaTime) => definition.ruleset.Tick(inventory, state, deltaTime);
		public override void LayoutTarget(Card card, CardTarget target) => inventory.Find(storable => storable.Contains(card)).LayoutTarget(card, target);
		
		public override bool CanFetch(Card card) => definition.ruleset.CanFetchFrom(inventory, state, card);
		public override bool Contains(Card card) => inventory.Any(storable => storable.Contains(card));
		
		public override async UniTask<(Card, Capturellectainer, Capturellectable)> Store(Capturellectable item)
		{
			int storeIndex = await definition.ruleset.GetIndexToStoreInto(inventory, state);
			Storable storable = inventory[storeIndex];
			inventory.Remove(storable);
			
			(Card card, Capturellectainer container, Capturellectable ejectedItem) = await storable.Store(item);
			int restoreIndex = await definition.ruleset.GetIndexToInsertBetweenAfterStore(inventory, state, storable, storeIndex);
			inventory.Insert(restoreIndex, storable);
			
			if (ejectedItem && ejectedItem.TryGetComponent(out Card flushedCard))
			{
				List<Card> cards = new(){ flushedCard };
				await Flush(cards, storeIndex);
				if (cards.Count == 0)
					ejectedItem = null;
			}
			
			return (card, container, ejectedItem);
		}
		
		public override async UniTask<Capturellectable> Fetch(Card card)
		{
			Storable storable = inventory.First(storable => storable.Contains(card));
			int fetchIndex = inventory.IndexOf(storable);
			inventory.Remove(storable);
			
			Capturellectable item = await storable.Fetch(card);
			int restoreIndex = await definition.ruleset.GetIndexToInsertBetweenAfterFetch(inventory, state, storable, fetchIndex);
			inventory.Insert(restoreIndex, storable);
			
			return item;
		}
		
		public override async UniTask Flush(List<Card> cards) => await Flush(cards, 0);
		public async UniTask Flush(List<Card> cards, int originalIndex)
		{
			if (hasAllCards || cards.Count == 0)
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
		
		public override void Load(List<Card> cards)
		{
			if (hasAllCards || cards.Count == 0)
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
		
		public override IEnumerable<Texture2D> GetCardTextures(Card card, IEnumerable<IEnumerable<Texture2D>> textures, int indexOfThisInParent)
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
		
		public override IEnumerator<Card> GetEnumerator() => inventory.SelectMany(storable => storable).GetEnumerator();
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SBEPIS.Capturllection
{
	[Serializable]
	public class StorableGroup : Storable
	{
		public StorableGroupDefinition definition;
		public List<Storable> inventory = new();
		
		public override bool hasNoCards => inventory.Count == 0;
		public override bool hasAllCards => inventory.Count == definition.maxStorables && inventory.All(storable => storable.hasAllCards);
		
		public override bool hasAllCardsEmpty => inventory.All(storable => storable.hasAllCardsEmpty);
		public override bool hasAllCardsFull => inventory.All(storable => storable.hasAllCardsFull);

		public override void Tick(float deltaTime)
		{
			definition.ruleset.Tick(inventory, deltaTime);
			foreach (Storable storable in inventory)
				storable.Tick(deltaTime);
		}
		public override void Layout()
		{
			definition.ruleset.Layout(inventory);
			foreach (Storable storable in inventory)
				storable.Layout();
		}
		public override void LayoutTarget(DequeStorable card, CardTarget target) => inventory.Find(storable => storable.Contains(card)).LayoutTarget(card, target);
		
		public override bool CanFetch(DequeStorable card) => definition.ruleset.CanFetchFrom(inventory, card);
		public override bool Contains(DequeStorable card) => inventory.Any(storable => storable.Contains(card));
		
		public override (DequeStorable, Capturellectainer) Store(Capturllectable item, out Capturllectable ejectedItem)
		{
			int storeIndex = definition.ruleset.GetIndexToStoreInto(inventory);
			Storable storable = inventory[storeIndex];
			inventory.Remove(storable);
			
			(DequeStorable card, Capturellectainer container) = storable.Store(item, out ejectedItem);
			
			int restoreIndex = definition.ruleset.GetIndexToInsertBetweenAfterStore(inventory, storable, storeIndex);
			inventory.Insert(restoreIndex, storable);
			
			if (ejectedItem.TryGetComponent(out DequeStorable flushedCard))
			{
				List<DequeStorable> cards = new(){ flushedCard };
				Flush(cards);
				if (cards.Count == 0)
					ejectedItem = null;
			}
			
			return (card, container);
		}
		
		public override Capturllectable Fetch(DequeStorable card)
		{
			Storable storable = inventory.First(storable => storable.Contains(card));
			int fetchIndex = inventory.IndexOf(storable);
			inventory.Remove(storable);
			
			Capturllectable item = storable.Fetch(card);
			
			int restoreIndex = definition.ruleset.GetIndexToInsertBetweenAfterFetch(inventory, storable, fetchIndex);
			inventory.Insert(restoreIndex, storable);
			
			return item;
		}
		
		public override void Flush(List<DequeStorable> cards) => Flush(cards, 0);
		public void Flush(List<DequeStorable> cards, int originalIndex)
		{
			if (hasAllCards || cards.Count == 0)
				return;
			
			foreach (Storable storable in inventory.Skip(originalIndex).Concat(inventory.Take(originalIndex)))
			{
				storable.Flush(cards);
				if (cards.Count == 0)
					return;
			}
			
			while (inventory.Count < definition.maxStorables)
			{
				Storable storable = StorableGroupDefinition.GetNewStorable(definition.subdefinition);
				storable.transform.SetParent(transform);
				storable.Flush(cards);
				
				int insertIndex = definition.ruleset.GetIndexToFlushBetween(inventory, storable);
				inventory.Insert(insertIndex, storable);
				
				if (cards.Count == 0)
					return;
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

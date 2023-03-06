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
		
		public StorableGroup(StorableGroupDefinition definition)
		{
			this.definition = definition;
		}
		
		public override void Tick(float deltaTime) => definition.ruleset.Tick(inventory, deltaTime);
		public override void Layout() => definition.ruleset.Layout(inventory);
		
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
				DequeStorable remainingCard = Flush(flushedCard);
				if (!remainingCard)
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

		public override DequeStorable Flush(DequeStorable card) => Flush(card, 0);
		public DequeStorable Flush(DequeStorable card, int originalIndex)
		{
			if (hasAllCards)
				return card;
			
			foreach (Storable storable in inventory.Skip(originalIndex).Concat(inventory.Take(originalIndex)))
			{
				card = storable.Flush(card);
				if (!card)
					return null;
			}
			
			while (inventory.Count < definition.maxStorables)
			{
				Storable storable = definition.GetNewStorable();
				card = storable.Flush(card);
				
				int insertIndex = definition.ruleset.GetIndexToFlushBetween(inventory, storable);
				inventory.Insert(insertIndex, storable);
				
				if (!card)
					return null;
			}
			
			return card;
		}
		
		public override IEnumerable<DequeStorable> Save() => inventory.SelectMany(card => card.Save());
		public override IEnumerable<DequeStorable> Load(IEnumerable<DequeStorable> newInventory) => inventory.Aggregate(newInventory, (current, storable) => storable.Load(current));
		public override void Clear()
		{
			foreach (Storable storable in inventory)
				storable.Clear();
			inventory.Clear();
		}
	}
}

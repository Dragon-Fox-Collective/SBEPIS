using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SBEPIS.Capturllection
{
	[Serializable]
	public class StorableGroup : Storable
	{
		public DequeRuleset ruleset;
		public List<Storable> inventory = new();
		
		
		public IEnumerable<DequeRuleset> nestedRulesets;

		public override Vector3 position { get; set; }
		public override Quaternion rotation { get; set; }
		
		public override bool isEmpty => inventory.All(storable => storable.isEmpty);

		public StorableGroup(DequeRuleset ruleset) : this(ruleset, -1, Enumerable.Empty<DequeRuleset>()) { }
		
		public StorableGroup(DequeRuleset ruleset, int maxStorables, IEnumerable<DequeRuleset> nestedRulesets)
		{
			this.ruleset = ruleset;
		}
		
		public override void Tick(float deltaTime) => ruleset.Tick(inventory, deltaTime);
		public override void Layout() => ruleset.Layout(inventory);
		
		public override bool CanFetch(DequeStorable card) => ruleset.CanFetchFrom(inventory, card);
		public override bool Contains(DequeStorable card) => inventory.Any(storable => storable.Contains(card));
		
		public override (DequeStorable, Capturellectainer) Store(Capturllectable item, out Capturllectable ejectedItem)
		{
			int storeIndex = ruleset.GetIndexToStoreInto(inventory);
			Storable storable = inventory[storeIndex];
			inventory.Remove(storable);
			
			(DequeStorable card, Capturellectainer container) = storable.Store(item, out ejectedItem);

			int restoreIndex = ruleset.GetIndexToInsertStorableBetweenAfterStore(inventory, storable, storeIndex);
			inventory.Insert(restoreIndex, storable);

			if (ejectedItem.TryGetComponent(out DequeStorable flushedCard))
			{
				Flush(flushedCard);
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
			
			int restoreIndex = ruleset.GetIndexToInsertStorableBetweenAfterFetch(inventory, storable, fetchIndex);
			inventory.Insert(restoreIndex, storable);
			
			return item;
		}
		
		public void Flush(DequeStorable card)
		{
			StorableSlot storable = new(card);

			int insertIndex = ruleset.GetIndexToFlushCardBetween(inventory, card);
			inventory.Insert(insertIndex, storable);
		}

		public override IEnumerable<DequeStorable> Save() => inventory.SelectMany(card => card.Save());
		public override void Load(IEnumerable<DequeStorable> inventory) => ;
		public override void Clear()
		{
			foreach (Storable storable in inventory)
				storable.Clear();
			inventory.Clear();
		}
	}
}

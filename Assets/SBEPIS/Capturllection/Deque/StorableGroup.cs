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
		public List<Storable> inventory;

		public override Vector3 position { get; set; }
		public override Quaternion rotation { get; set; }

		public override bool isEmpty => inventory.All(storable => storable.isEmpty);

		public override void Tick(float deltaTime) => ruleset.Tick(inventory, deltaTime);
		
		public override void Layout() => ruleset.Layout(inventory);
		
		public override bool CanFetch(DequeStorable card) => ruleset.CanFetchFrom(inventory, card);
		
		public override bool Contains(DequeStorable card) => inventory.Any(storable => storable.Contains(card));
		
		public override (DequeStorable, Capturellectainer) Store(Capturllectable item, out Capturllectable ejectedItem)
		{
			int storeIndex = ruleset.GetIndexToStoreInto(inventory);
			Storable storable = inventory[storeIndex];
			inventory.RemoveAt(storeIndex);
			
			(DequeStorable card, Capturellectainer container) = storable.Store(item, out ejectedItem);
			
			Replace(storable, storeIndex);
			
			return (card, container);
		}
		
		public override Capturllectable Fetch(DequeStorable card)
		{
			if (!CanFetch(card))
				return null;
			
			Storable storable = inventory.First(storable => storable.Contains(card));
			int fetchIndex = inventory.IndexOf(storable);
			inventory.Remove(storable);
			Capturllectable item = storable.Fetch(card);
			
			int restoreIndex = ruleset.GetIndexToInsertStorableBetweenAfterStore(inventory, storable, fetchIndex);
			inventory.Insert(restoreIndex, storable);
			
			return item;
		}
		
		public override void Flush(DequeStorable card)
		{
			card.state.hasBeenAssembled = false;
			int insertIndex = ruleset.GetIndexToFlushCardBetween(inventory, card);
			inventory.Insert(insertIndex, card);
		}
	}
}

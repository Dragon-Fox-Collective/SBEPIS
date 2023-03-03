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

		public override bool isEmpty => inventory.All(storable => storable.isEmpty);

		public override void Tick(float deltaTime) => ruleset.Tick(inventory, deltaTime);
		
		public override void Layout() => ruleset.Layout(inventory);
		
		public override bool CanFetch(DequeStorable card) => ruleset.CanFetchFrom(inventory, card);

		public override (DequeStorable, Capturellectainer) Store(Capturllectable item, out Capturllectable ejectedItem)
		{
			int storeIndex = ruleset.GetIndexToStoreInto(inventory);
			DequeStorable card = inventory[storeIndex];
			inventory.RemoveAt(storeIndex);
			
			Capturellectainer container = card.GetComponent<Capturellectainer>();
			ejectedItem = container.Fetch();
			container.Capture(item);
			StoreCard(card);

			return (card, container);
		}
		
		public override Capturllectable Fetch(DequeStorable card)
		{
			if (!CanFetch(card))
				return null;

			inventory.Remove(card);
			
			Capturllectable item = container.Fetch();
			StoreCard(card);
			
			return item;
		}
		
		public override void Flush(DequeStorable card)
		{
			card.state.hasBeenAssembled = false;
			int insertIndex = ruleset.GetIndexToInsertCardBetween(inventory, card);
			inventory.Insert(insertIndex, card);
		}
	}
}

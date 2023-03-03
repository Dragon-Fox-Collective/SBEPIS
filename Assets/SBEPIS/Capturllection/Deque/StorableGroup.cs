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

		public void Tick(float deltaTime) => ruleset.Tick(inventory, deltaTime);

		public void Layout() => ruleset.Layout(inventory);
		
		public override bool CanFetch(DequeStorable card) => ruleset.CanFetchFrom(inventory, card);

		public void StoreCard(DequeStorable card)
		{
			card.state.hasBeenAssembled = false;
			int insertIndex = deque.GetIndexToInsertCardBetween(cards, card);
			cards.Insert(insertIndex, card);
		}

		public override (DequeStorable, Capturellectainer) Store(Capturllectable item, out Capturllectable ejectedItem)
		{
			int storeIndex = deque.GetIndexToStoreInto(cards);
			DequeStorable card = cards[storeIndex];
			cards.RemoveAt(storeIndex);
			
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

			cards.Remove(card);
			
			Capturllectable item = container.Fetch();
			StoreCard(card);
			
			return item;
		}
	}
}

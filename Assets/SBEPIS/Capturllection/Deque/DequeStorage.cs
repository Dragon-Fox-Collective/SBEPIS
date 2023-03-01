using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SBEPIS.Capturllection
{
	public class DequeStorage : MonoBehaviour, IEnumerable<DequeStorable>
	{
		public int initialCardCount = 5;
		
		public DequeStorable cardPrefab;
		
		public DequeLayer definition { get; set; }
		
		private List<DequeStorable> cards = new();
		
		public void CreateInitialCards(DequeOwner owner)
		{
			for (int _ = 0; _ < initialCardCount; _++)
			{
				DequeStorable card = Instantiate(cardPrefab);
				card.owner = owner;
				StoreCard(card);
			}
		}
		
		public void Tick(float deltaTime)
		{
			definition.Tick(cards, deltaTime);
		}
		
		public void LayoutTargets(Dictionary<DequeStorable, CardTarget> targets)
		{
			definition.LayoutTargets(cards, targets);
		}
		
		public bool CanFetch(DequeStorable card)
		{
			return definition.CanFetch(cards, card);
		}
		
		public void StoreCard(DequeStorable card)
		{
			int insertIndex = definition.GetIndexToInsertCardBetween(cards, card);
			cards.Insert(insertIndex, card);
		}

		public (DequeStorable, Capturellectainer) StoreItem(Capturllectable item)
		{
			int storeIndex = definition.GetIndexToStoreInto(cards);
			DequeStorable card = cards[storeIndex];
			cards.RemoveAt(storeIndex);
			
			Capturellectainer container = card.GetComponent<Capturellectainer>();
			container.Capture(item);
			StoreCard(card);

			return (card, container);
		}
		
		public Capturllectable FetchItem(DequeStorable card, Capturellectainer container)
		{
			if (!CanFetch(card))
				return null;

			cards.Remove(card);
			
			Capturllectable item = container.Fetch();
			StoreCard(card);
			
			return item;
		}
		
		
		public bool Contains(DequeStorable card) => cards.Contains(card);
		public IEnumerator<DequeStorable> GetEnumerator() => cards.GetEnumerator();
		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
	}
}

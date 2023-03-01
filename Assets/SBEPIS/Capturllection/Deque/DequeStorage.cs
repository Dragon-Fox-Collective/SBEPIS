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
		
		private void Start()
		{
			for (int _ = 0; _ < initialCardCount; _++)
			{
				DequeStorable card = Instantiate(cardPrefab);
				cards.Add(card);
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

		public void StoreItem(Capturllectable item)
		{
			int fetchIndex = definition.GetIndexToStoreInto(cards);
			DequeStorable fetchCard = cards[fetchIndex];
			cards.RemoveAt(fetchIndex);
			
			Capturellectainer container = fetchCard.GetComponent<Capturellectainer>();
			container.Fetch();
			container.Capture(item);
			StoreCard(fetchCard);
		}
		
		
		public bool Contains(DequeStorable card) => cards.Contains(card);
		public IEnumerator<DequeStorable> GetEnumerator() => cards.GetEnumerator();
		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
	}
}

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SBEPIS.Capturllection
{
	public class DequeStorage : MonoBehaviour, IEnumerable<DequeStorable>
	{
		public int initialCardCount = 5;
		
		public DequeStorable cardPrefab;
		
		private Deque deque;
		public List<Texture2D> cardTextures { get; private set; }
		
		private List<Storable> cards = new();
		
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
			deque.Tick(cards, deltaTime);
		}
		
		public void LayoutTargets(Dictionary<DequeStorable, CardTarget> targets)
		{
			deque.LayoutTargets(cards, targets);
		}
		
		public bool CanFetch(DequeStorable card)
		{
			return deque.CanFetch(cards, card);
		}
		
		public void StoreCard(DequeStorable card)
		{
			card.state.hasBeenAssembled = false;
			int insertIndex = deque.GetIndexToInsertCardBetween(cards, card);
			cards.Insert(insertIndex, card);
		}

		public (DequeStorable, Capturellectainer) StoreItem(Capturllectable item, out Capturllectable ejectedItem)
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
		
		public Capturllectable FetchItem(DequeStorable card, Capturellectainer container)
		{
			if (!CanFetch(card))
				return null;

			cards.Remove(card);
			
			Capturllectable item = container.Fetch();
			StoreCard(card);
			
			return item;
		}

		public void SyncDeque(DequeBox dequeBox)
		{
			deque = dequeBox.definition;
			cardTextures = deque.GetCardTextures().ToList();
		}
		
		
		public bool Contains(DequeStorable card) => cards.Contains(card);
		public IEnumerator<DequeStorable> GetEnumerator() => cards.GetEnumerator();
		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
	}
}

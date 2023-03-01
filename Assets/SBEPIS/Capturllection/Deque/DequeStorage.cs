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
			int index = definition.GetIndexToInsertInto(cards, card);
			cards.Insert(index, card);
		}


		public bool Contains(DequeStorable card) => cards.Contains(card);

		public IEnumerator<DequeStorable> GetEnumerator()
		{
			return cards.GetEnumerator();
		}
		
		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}

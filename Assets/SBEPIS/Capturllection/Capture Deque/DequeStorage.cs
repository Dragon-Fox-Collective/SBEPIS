using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SBEPIS.Capturllection
{
	public class DequeStorage : MonoBehaviour, IEnumerable<DequeStorable>
	{
		public DequeLayer definition { get; set; }
		
		private List<DequeStorable> cards = new();
		
		public void Tick(float deltaTime)
		{
			definition.Tick(cards, deltaTime);
		}
		
		public void LayoutTargets(Dictionary<DequeStorable, CardTarget> targets)
		{
			definition.LayoutTargets(targets);
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

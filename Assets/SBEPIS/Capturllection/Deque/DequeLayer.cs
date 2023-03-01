using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SBEPIS.Capturllection
{
	[Serializable]
	public class DequeLayer
	{
		public List<DequeType> deques;
		
		private List<Texture2D> cardTextures;
		
		public void Tick(List<DequeStorable> cards, float delta) => deques.Do(deque => deque.Tick(cards, delta));
		
		public void LayoutTargets(List<DequeStorable> cards, Dictionary<DequeStorable, CardTarget> targets) => deques[0].LayoutTargets(cards, targets);
		
		public bool CanFetch(List<DequeStorable> cards, DequeStorable card) => deques.AsEnumerable().Reverse().Any(deque => deque.CanFetch(cards, card));
		
		public int GetIndexToStoreInto(List<DequeStorable> cards) => deques[^1].GetIndexToStoreInto(cards);
		
		public int GetIndexToInsertCardBetween(List<DequeStorable> cards, DequeStorable card) => deques[^1].GetIndexToInsertCardBetween(cards, card);

		public void UpdateCardTexture(DequeStorable card)
		{
			cardTextures ??= deques.Select(deque => deque.cardTexture).ToList();
			card.split.UpdateTexture(cardTextures);
		}
	}
}

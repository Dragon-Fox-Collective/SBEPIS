using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SBEPIS.Capturllection
{
	public class CombobDeque : Deque
	{
		public List<Deque> deques;
		
		public override void Tick(List<DequeStorable> cards, float delta) => deques.Do(deque => deque.Tick(cards, delta));
		
		public override void LayoutTargets(List<DequeStorable> cards, Dictionary<DequeStorable, CardTarget> targets) => deques[0].LayoutTargets(cards, targets);
		
		public override bool CanFetch(List<DequeStorable> cards, DequeStorable card) => deques.AsEnumerable().Reverse().Any(deque => deque.CanFetch(cards, card));
		
		public override int GetIndexToStoreInto(List<DequeStorable> cards) => deques[^1].GetIndexToStoreInto(cards);
		
		public override int GetIndexToInsertCardBetween(List<DequeStorable> cards, DequeStorable card) => deques[^1].GetIndexToInsertCardBetween(cards, card);
		
		public override IEnumerable<Texture2D> GetCardTextures() => deques.Aggregate(Enumerable.Empty<Texture2D>(), (total, deque) => total.Concat(deque.GetCardTextures()));
		public override IEnumerable<Texture2D> GetBoxTextures() => deques.Aggregate(Enumerable.Empty<Texture2D>(), (total, deque) => total.Concat(deque.GetBoxTextures()));
	}
}

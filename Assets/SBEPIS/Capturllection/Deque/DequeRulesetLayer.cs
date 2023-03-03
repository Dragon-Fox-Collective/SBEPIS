using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SBEPIS.Capturllection
{
	public class DequeRulesetLayer : DequeRuleset
	{
		public List<DequeRuleset> rulesets;
		
		public override void Tick(List<Storable> inventory, float delta) => rulesets.Do(deque => deque.Tick(inventory, delta));
		
		public override void Layout(List<Storable> inventory) => rulesets[0].Layout(inventory);
		
		public override bool CanFetchFrom(List<Storable> inventory, DequeStorable card) => rulesets.AsEnumerable().Reverse().Any(deque => deque.CanFetchFrom(inventory, card));
		
		public override int GetIndexToStoreInto(List<Storable> inventory) => rulesets[^1].GetIndexToStoreInto(inventory);
		
		public override int GetIndexToInsertCardBetween(List<Storable> inventory, DequeStorable card) => rulesets[^1].GetIndexToInsertCardBetween(inventory, card);
		
		public override IEnumerable<Texture2D> GetCardTextures() => rulesets.Aggregate(Enumerable.Empty<Texture2D>(), (total, deque) => total.Concat(deque.GetCardTextures()));
		public override IEnumerable<Texture2D> GetBoxTextures() => rulesets.Aggregate(Enumerable.Empty<Texture2D>(), (total, deque) => total.Concat(deque.GetBoxTextures()));
	}
}

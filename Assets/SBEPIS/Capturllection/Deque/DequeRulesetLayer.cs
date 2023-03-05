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
		public override int GetIndexToFlushCardBetween(List<Storable> inventory, DequeStorable card) => rulesets[^1].GetIndexToFlushCardBetween(inventory, card);
		public override int GetIndexToInsertStorableBetweenAfterStore(List<Storable> inventory, Storable storable, int originalIndex) => rulesets[^1].GetIndexToInsertStorableBetweenAfterStore(inventory, storable, originalIndex);
		public override int GetIndexToInsertStorableBetweenAfterFetch(List<Storable> inventory, Storable storable, int originalIndex) => rulesets[^1].GetIndexToInsertStorableBetweenAfterFetch(inventory, storable, originalIndex);
		
		public override IEnumerable<Texture2D> GetCardTextures() => rulesets.SelectMany(ruleset => ruleset.GetCardTextures());
		public override IEnumerable<Texture2D> GetBoxTextures() => rulesets.SelectMany(ruleset => ruleset.GetBoxTextures());
	}
}

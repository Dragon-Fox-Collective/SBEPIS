using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace SBEPIS.Capturllection
{
	public class DequeRulesetLayer : DequeRuleset<DequeRulesetLayerState>
	{
		public List<DequeRuleset> rulesets;
		
		public override void Tick(List<Storable> inventory, DequeRulesetLayerState state, float deltaTime) => rulesets.Zip(state.states).Reverse().Do(zip => zip.Item1.Tick(inventory, zip.Item2, deltaTime));
		public override Vector3 GetMaxPossibleSizeOf(List<Storable> inventory, DequeRulesetLayerState state) => rulesets[0].GetMaxPossibleSizeOf(inventory, state.states[0]);
		
		public override bool CanFetchFrom(List<Storable> inventory, DequeRulesetLayerState state, Card card) => rulesets.Zip(state.states).Reverse().Any(zip => zip.Item1.CanFetchFrom(inventory, zip.Item2, card));
		
		public override UniTask<int> GetIndexToStoreInto(List<Storable> inventory, DequeRulesetLayerState state) => rulesets[^1].GetIndexToStoreInto(inventory, state.states[^1]);
		public override UniTask<int> GetIndexToFlushBetween(List<Storable> inventory, DequeRulesetLayerState state, Storable storable) => rulesets[^1].GetIndexToFlushBetween(inventory, state.states[^1], storable);
		public override UniTask<int> GetIndexToInsertBetweenAfterStore(List<Storable> inventory, DequeRulesetLayerState state, Storable storable, int originalIndex) => rulesets[^1].GetIndexToInsertBetweenAfterStore(inventory, state.states[^1], storable, originalIndex);
		public override UniTask<int> GetIndexToInsertBetweenAfterFetch(List<Storable> inventory, DequeRulesetLayerState state, Storable storable, int originalIndex) => rulesets[^1].GetIndexToInsertBetweenAfterFetch(inventory, state.states[^1], storable, originalIndex);
		
		public override DequeRulesetState GetNewState()
		{
			DequeRulesetLayerState state = new();
			state.states = rulesets.Select(ruleset => ruleset.GetNewState()).ToList();
			return state;
		}
		
		public override string GetDequeNamePart(bool isFirst, bool isLast, bool isPlural) => rulesets.Enumerate().Aggregate("", (current, zip) => current + zip.item.GetDequeNamePart(isFirst && zip.index == 0, isLast && zip.index == rulesets.Count - 1, isPlural));
		
		public override IEnumerable<DequeSettingsPageLayout> GetNewSettingsPageLayouts(bool isFirst, bool isLast) =>
			rulesets.Enumerate().SelectMany(zip => zip.item.GetNewSettingsPageLayouts(isFirst && zip.index == 0, isLast && zip.index == rulesets.Count - 1));
		
		public override IEnumerable<Texture2D> GetCardTextures() => rulesets.SelectMany(ruleset => ruleset.GetCardTextures());
		public override IEnumerable<Texture2D> GetBoxTextures() => rulesets.SelectMany(ruleset => ruleset.GetBoxTextures());
	}
}

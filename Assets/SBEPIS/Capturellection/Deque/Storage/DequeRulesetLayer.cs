using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace SBEPIS.Capturellection.Storage
{
	public class DequeRulesetLayer : DequeRuleset<DequeRulesetLayerState>
	{
		public List<DequeRuleset> rulesets;
		
		public override void Tick(List<Storable> inventory, DequeRulesetLayerState state, float deltaTime) => rulesets.Zip(state.states).Reverse().ForEach(zip => zip.Item1.Tick(inventory, zip.Item2, deltaTime));
		public override Vector3 GetMaxPossibleSizeOf(List<Storable> inventory, DequeRulesetLayerState state) => rulesets[0].GetMaxPossibleSizeOf(inventory, state.states[0]);
		
		public override bool CanFetchFrom(List<Storable> inventory, DequeRulesetLayerState state, InventoryStorable card) => rulesets.Zip(state.states).Reverse().Any(zip => zip.Item1.CanFetchFrom(inventory, zip.Item2, card));
		
		public override UniTaskVoid Store(List<Storable> inventory, DequeRulesetLayerState state) => rulesets[^1].Store(inventory, state.states[^1]);
		public override UniTask<int> Flush(List<Storable> inventory, DequeRulesetLayerState state, Storable storable) => rulesets[^1].Flush(inventory, state.states[^1], storable);
		public override UniTaskVoid RestoreAfterStore(List<Storable> inventory, DequeRulesetLayerState state, Storable storable, int originalIndex) => rulesets[^1].RestoreAfterStore(inventory, state.states[^1], storable, originalIndex);
		public override UniTask<int> RestoreAfterFetch(List<Storable> inventory, DequeRulesetLayerState state, Storable storable, int originalIndex) => rulesets[^1].RestoreAfterFetch(inventory, state.states[^1], storable, originalIndex);
		
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

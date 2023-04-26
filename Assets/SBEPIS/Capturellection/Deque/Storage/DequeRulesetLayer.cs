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
		
		public override UniTask<DequeStoreResult> StoreItem(List<Storable> inventory, DequeRulesetLayerState state, Capturellectable item) => rulesets[^1].StoreItem(inventory, state.states[^1], item);
		public override UniTask<DequeStoreResult> StoreItemHook(List<Storable> inventory, DequeRulesetLayerState state, Capturellectable item, DequeStoreResult oldResult) => rulesets.Zip(state.states).Reverse().Aggregate(oldResult, (result, zip) => zip.Item1.StoreItemHook(inventory, zip.Item2, item, result));
		public override UniTask<Capturellectable> FetchItem(List<Storable> inventory, DequeRulesetLayerState state, InventoryStorable card) => rulesets[^1].FetchItem(inventory, state.states[^1], card);
		public override UniTask<Capturellectable> FetchItemHook(List<Storable> inventory, DequeRulesetLayerState state, InventoryStorable card, Capturellectable oldItem) => rulesets.Zip(state.states).Reverse().Aggregate(oldItem, (result, zip) => zip.Item1.FetchItemHook(inventory, zip.Item2, card, result));
		public override UniTask FlushCard(List<Storable> inventory, DequeRulesetLayerState state, Storable storable) => rulesets[^1].FlushCard(inventory, state.states[^1], storable);
		public override UniTask<IEnumerable<Storable>> FlushCardPreHook(List<Storable> inventory, DequeRulesetLayerState state, Storable storable) => rulesets.Zip(state.states).Reverse().Aggregate(Enumerable.Repeat(storable, 1), (result, zip) => result.Select(res => zip.Item1.FlushCardPreHook(inventory, zip.Item2, res)).ContinueWith(res => res.Flatten()));
		public override UniTask FetchCard(List<Storable> inventory, DequeRulesetLayerState state, InventoryStorable card) => rulesets[^1].FetchCard(inventory, state.states[^1], card);
		public override UniTask FetchCardHook(List<Storable> inventory, DequeRulesetLayerState state, InventoryStorable card) => rulesets.Zip(state.states).Reverse().ForEach(zip => zip.Item1.FetchCardHook(inventory, state, card));
		
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

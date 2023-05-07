using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace SBEPIS.Capturellection.Storage
{
	public class DequeRulesetLayer : DequeRuleset<DequeRulesetLayerState>
	{
		public List<DequeRuleset> rulesets;
		
		public override void Tick(List<Storable> inventory, DequeRulesetLayerState layerState, float deltaTime) => Zip(layerState).ForEach((ruleset, state) => ruleset.Tick(inventory, state, deltaTime));
		public override Vector3 GetMaxPossibleSizeOf(List<Storable> inventory, DequeRulesetLayerState layerState) => DoOn(First, layerState, (ruleset, state) => ruleset.GetMaxPossibleSizeOf(inventory, state));
		
		public override bool CanFetchFrom(List<Storable> inventory, DequeRulesetLayerState state, InventoryStorable card) => Zip(state).Any(zip => zip.Item1.CanFetchFrom(inventory, zip.Item2, card));
		
		public override UniTask<DequeStoreResult> StoreItem(List<Storable> inventory, DequeRulesetLayerState layerState, Capturellectable item) => DoOn(Last, layerState, (ruleset, state) => ruleset.StoreItem(inventory, state, item));
		public override UniTask<DequeStoreResult> StoreItemHook(List<Storable> inventory, DequeRulesetLayerState layerState, Capturellectable item, DequeStoreResult oldResult) => Aggregate(layerState, oldResult, (result, ruleset, state) => ruleset.StoreItemHook(inventory, state, item, result));
		public override UniTask<Capturellectable> FetchItem(List<Storable> inventory, DequeRulesetLayerState layerState, InventoryStorable card) => DoOn(Last, layerState, (ruleset, state) => ruleset.FetchItem(inventory, state, card));
		public override UniTask<Capturellectable> FetchItemHook(List<Storable> inventory, DequeRulesetLayerState layerState, InventoryStorable card, Capturellectable oldItem) => Aggregate(layerState, oldItem, (result, ruleset, state) => ruleset.FetchItemHook(inventory, state, card, result));
		public override UniTask FlushCard(List<Storable> inventory, DequeRulesetLayerState layerState, Storable storable) => DoOn(Last, layerState, (ruleset, state) => ruleset.FlushCard(inventory, state, storable));
		public override UniTask<IEnumerable<Storable>> FlushCardPreHook(List<Storable> inventory, DequeRulesetLayerState layerState, Storable storable) => Aggregate(layerState, ExtensionMethods.EnumerableOf(storable), (result, ruleset, state) => result.Select(res => ruleset.FlushCardPreHook(inventory, state, res)).ContinueWith(res => res.Flatten()));
		public override UniTask<InventoryStorable> FetchCard(List<Storable> inventory, DequeRulesetLayerState layerState, InventoryStorable card) => DoOn(Last, layerState, (ruleset, state) => ruleset.FetchCard(inventory, state, card));
		public override UniTask<InventoryStorable> FetchCardHook(List<Storable> inventory, DequeRulesetLayerState layerState, InventoryStorable oldCard) => Aggregate(layerState, oldCard, (result, ruleset, state) => ruleset.FetchCardHook(inventory, state, result));
		
		private (DequeRuleset, DequeRulesetState) First(DequeRulesetLayerState state) => (rulesets[0], state.states[0]);
		private (DequeRuleset, DequeRulesetState) Last(DequeRulesetLayerState state) => (rulesets[^1], state.states[^1]);
		private static T DoOn<T>(Func<DequeRulesetLayerState, (DequeRuleset, DequeRulesetState)> getter, DequeRulesetLayerState state, Func<DequeRuleset, DequeRulesetState, T> func) => func.InvokeWith(getter(state));
		private IEnumerable<(DequeRuleset, DequeRulesetState)> Zip(DequeRulesetLayerState state) => rulesets.Zip(state.states).Reverse();
		private UniTask<T> Aggregate<T>(DequeRulesetLayerState state, T seed, Func<T, DequeRuleset, DequeRulesetState, UniTask<T>> func) => Zip(state).Aggregate(seed, (result, zip) => func(result, zip.Item1, zip.Item2));
		
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

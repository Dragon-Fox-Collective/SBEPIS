using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using KBCore.Refs;
using UnityEngine;

namespace SBEPIS.Capturellection.Storage
{
	public class DequeRulesetLayer : ValidatedMonoBehaviour, DequeRuleset<DequeRulesetLayerState>
	{
		[SerializeField, Anywhere] private InterfaceRef<DequeRuleset>[] rulesets;
		
		public void Tick(List<Storable> inventory, DequeRulesetLayerState layerState, float deltaTime) => Zip(layerState).ForEach((ruleset, state) => ruleset.Tick(inventory, state, deltaTime));
		public Vector3 GetMaxPossibleSizeOf(List<Storable> inventory, DequeRulesetLayerState layerState) => DoOn(First, layerState, (ruleset, state) => ruleset.GetMaxPossibleSizeOf(inventory, state));
		
		public bool CanFetchFrom(List<Storable> inventory, DequeRulesetLayerState state, InventoryStorable card) => Zip(state).Any(zip => zip.Item1.CanFetchFrom(inventory, zip.Item2, card));
		
		public UniTask<DequeStoreResult>		StoreItem			(List<Storable> inventory, DequeRulesetLayerState layerState, Capturellectable item)								=> DoOn(Last,	layerState,	(ruleset, state)									=> ruleset.StoreItem		(inventory, state, item));
		public UniTask<DequeStoreResult>		StoreItemHook		(List<Storable> inventory, DequeRulesetLayerState layerState, Capturellectable item, DequeStoreResult oldResult)	=> Aggregate(	layerState, oldResult, (result, ruleset, state)				=> ruleset.StoreItemHook	(inventory, state, item, result));
		public UniTask<Capturellectable>		FetchItem			(List<Storable> inventory, DequeRulesetLayerState layerState, InventoryStorable card)								=> DoOn(Last,	layerState,	(ruleset, state)									=> ruleset.FetchItem		(inventory, state, card));
		public UniTask<Capturellectable>		FetchItemHook		(List<Storable> inventory, DequeRulesetLayerState layerState, InventoryStorable card, Capturellectable oldItem)		=> Aggregate(	layerState, oldItem, (result, ruleset, state)	=> ruleset.FetchItemHook	(inventory, state, card, result));
		public UniTask							FlushCard			(List<Storable> inventory, DequeRulesetLayerState layerState, Storable storable)									=> DoOn(Last,	layerState, (ruleset, state)									=> ruleset.FlushCard		(inventory, state, storable));
		public UniTask<IEnumerable<Storable>>	FlushCardPreHook	(List<Storable> inventory, DequeRulesetLayerState layerState, Storable storable)									=> Aggregate(	layerState, ExtensionMethods.EnumerableOf(storable), (result, ruleset, state) => result.SelectMany(res => ruleset.FlushCardPreHook(inventory, state, res)).ContinueWith(res => res.Where(r => r)));
		public UniTask							FlushCardPostHook	(List<Storable> inventory, DequeRulesetLayerState layerState, Storable storable)									=> Zip(			layerState).ForEach((ruleset, state)								=> ruleset.FlushCardPostHook(inventory, state, storable));
		public UniTask<InventoryStorable>		FetchCard			(List<Storable> inventory, DequeRulesetLayerState layerState, InventoryStorable card)								=> DoOn(Last,	layerState, (ruleset, state)									=> ruleset.FetchCard		(inventory, state, card));
		public UniTask<InventoryStorable>		FetchCardHook		(List<Storable> inventory, DequeRulesetLayerState layerState, InventoryStorable oldCard)							=> Aggregate(	layerState, oldCard, (result, ruleset, state)	=> ruleset.FetchCardHook	(inventory, state, result));
		public IEnumerable<Storable>			LoadCardPreHook		(List<Storable> inventory, DequeRulesetLayerState layerState, Storable storable)									=> Aggregate(	layerState, ExtensionMethods.EnumerableOf(storable), (result, ruleset, state) => result.SelectMany(res => ruleset.LoadCardPreHook(inventory, state, res)).Where(r => r));
		public void								LoadCardPostHook	(List<Storable> inventory, DequeRulesetLayerState layerState, Storable storable)									=> Zip(			layerState).ForEach((ruleset, state)								=> ruleset.LoadCardPostHook	(inventory, state, storable));
		public InventoryStorable				SaveCardHook		(List<Storable> inventory, DequeRulesetLayerState layerState, InventoryStorable oldCard)							=> Aggregate(	layerState, oldCard, (result, ruleset, state)	=> ruleset.SaveCardHook		(inventory, state, result));
		
		private IEnumerable<DequeRuleset> Rulesets => rulesets.Select(ruleset => ruleset.Value);
		private (DequeRuleset, object) First(DequeRulesetLayerState state) => (rulesets[0].Value, state.states[0]);
		private (DequeRuleset, object) Last(DequeRulesetLayerState state) => (rulesets[^1].Value, state.states[^1]);
		private static T DoOn<T>(Func<DequeRulesetLayerState, (DequeRuleset, object)> getter, DequeRulesetLayerState state, Func<DequeRuleset, object, T> func) => func.InvokeWith(getter(state));
		private IEnumerable<(DequeRuleset, object)> Zip(DequeRulesetLayerState state) => Rulesets.Zip(state.states).Reverse();
		private T Aggregate<T>(DequeRulesetLayerState state, T seed, Func<T, DequeRuleset, object, T> func) => Zip(state).Aggregate(seed, (result, zip) => func(result, zip.Item1, zip.Item2));
		private UniTask<T> Aggregate<T>(DequeRulesetLayerState state, T seed, Func<T, DequeRuleset, object, UniTask<T>> func) => Zip(state).Aggregate(seed, (result, zip) => func(result, zip.Item1, zip.Item2));
		
		public DequeRulesetLayerState GetNewState()
		{
			DequeRulesetLayerState state = new();
			state.states = rulesets.Select(ruleset => ruleset.Value.GetNewState()).ToArray<object>();
			return state;
		}
		
		public string GetDequeNamePart(bool isFirst, bool isLast, bool isPlural) => Rulesets.Enumerate().Aggregate("", (current, zip) => current + zip.item.GetDequeNamePart(isFirst && zip.index == 0, isLast && zip.index == rulesets.Length - 1, isPlural));
		
		public IEnumerable<DequeSettingsPageLayout> GetNewSettingsPageLayouts(bool isFirst, bool isLast) =>
			rulesets.Enumerate().SelectMany(zip => zip.item.Value.GetNewSettingsPageLayouts(isFirst && zip.index == 0, isLast && zip.index == rulesets.Length - 1));
		
		public IEnumerable<Texture2D> GetCardTextures() => Rulesets.SelectMany(ruleset => ruleset.GetCardTextures());
		public IEnumerable<Texture2D> GetBoxTextures() => Rulesets.SelectMany(ruleset => ruleset.GetBoxTextures());
	}
	
	[Serializable]
	public class DequeRulesetLayerState : DirectionState
	{
		public object[] states;
		
		private Vector3 direction;
		public Vector3 Direction
		{
			get => direction;
			set
			{
				direction = value;
				foreach (object state in states)
					if (state is DirectionState directionState)
						directionState.Direction = value;
			}
		}
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using KBCore.Refs;
using UnityEngine;

namespace SBEPIS.Capturellection.Storage
{
	public class LayerDeque : ValidatedMonoBehaviour, DequeRuleset<LayerState>
	{
		[SerializeField, Anywhere] private InterfaceRef<DequeRuleset>[] rulesets;
		
		public void InitPage(LayerState layerState, DiajectorPage page) => Zip(layerState).ForEach((ruleset, state) => ruleset.InitPage(state, page));
		
		public void Tick(LayerState layerState, float deltaTime) => Zip(layerState).ForEach((ruleset, state) => ruleset.Tick(state, deltaTime));
		public void Layout(LayerState layerState) => DoOn(DequeForLayingOut, layerState, (ruleset, state) => ruleset.Layout(state));
		public Vector3 GetMaxPossibleSizeOf(LayerState layerState) => DoOn(DequeForLayingOut, layerState, (ruleset, state) => ruleset.GetMaxPossibleSizeOf(state));
		
		public bool CanFetch(LayerState state, InventoryStorable card) => Zip(state).Any(zip => zip.Item1.CanFetch(zip.Item2, card));
		
		public UniTask<StoreResult>				StoreItem			(LayerState layerState, Capturellectable item)								=> DoOn(DequeForStoring, layerState,						(ruleset, state)							=> ruleset.StoreItem		(state, item));
		public UniTask<StoreResult>				StoreItemHook		(LayerState layerState, Capturellectable item, StoreResult oldResult)		=> Aggregate(layerState, oldResult,							(result, ruleset, state)					=> ruleset.StoreItemHook	(state, item, result));
		public UniTask<FetchResult>				FetchItem			(LayerState layerState, InventoryStorable card)								=> DoOn(DequeForFetching(card), layerState,			(ruleset, state)							=> ruleset.FetchItem		(state, card));
		public UniTask<FetchResult>				FetchItemHook		(LayerState layerState, InventoryStorable card, FetchResult oldResult)		=> Aggregate(layerState, oldResult,							(result, ruleset, state)					=> ruleset.FetchItemHook	(state, card, result));
		public UniTask							Interact<TState>	(LayerState layerState, InventoryStorable card, DequeRuleset targetDeque, DequeInteraction<TState> action) => Zip(layerState).ForEach(	(ruleset, state)								=> ruleset.Interact			(state, card, targetDeque, action));
		public IEnumerable<InventoryStorable>	LoadCardHook		(LayerState layerState, InventoryStorable card)								=> AggregateExponential(layerState, card,					(result, ruleset, state)	=> ruleset.LoadCardHook		(state, result));
		public IEnumerable<InventoryStorable>	SaveCardHook		(LayerState layerState, InventoryStorable card)								=> AggregateExponential(layerState, card,					(result, ruleset, state)	=> ruleset.SaveCardHook		(state, result));
		public IEnumerable<Storable>			LoadStorableHook	(LayerState layerState, Storable storable)									=> AggregateExponential(layerState, storable,				(result, ruleset, state)			=> ruleset.LoadStorableHook	(state, result));
		public IEnumerable<Storable>			SaveStorableHook	(LayerState layerState, Storable storable)									=> AggregateExponential(layerState, storable,				(result, ruleset, state)			=> ruleset.SaveStorableHook	(state, result));
		
		private IEnumerable<DequeRuleset> Rulesets => rulesets.Select(ruleset => ruleset.Value);
		private static T DoOn<T>(Func<LayerState, (DequeRuleset, object)> getter, LayerState state, Func<DequeRuleset, object, T> func) => func.InvokeWith(getter(state));
		private static void DoOn(Func<LayerState, (DequeRuleset, object)> getter, LayerState state, Action<DequeRuleset, object> func) => func.InvokeWith(getter(state));
		private IEnumerable<(DequeRuleset, object)> Zip(LayerState state) => Rulesets.Zip(state.states).Reverse();
		private T Aggregate<T>(LayerState state, T seed, Func<T, DequeRuleset, object, T> func)						=> Zip(state).Aggregate(seed,      (result, zip) => func(result, zip.Item1, zip.Item2));
		private UniTask<T> Aggregate<T>(LayerState state, T seed, Func<T, DequeRuleset, object, UniTask<T>> func)	=> Zip(state).Aggregate(seed, (result, zip) => func(result, zip.Item1, zip.Item2));
		private IEnumerable<T> AggregateExponential<T>(LayerState layerState, T seed, Func<T, DequeRuleset, object, IEnumerable<T>> func)					=> Aggregate(layerState, ExtensionMethods.EnumerableOf(seed), (result, ruleset, state) => result.SelectMany(res => func(res, ruleset, state)).Where(res => res is not null));
		private UniTask<IEnumerable<T>> AggregateExponential<T>(LayerState layerState, T seed, Func<T, DequeRuleset, object, UniTask<IEnumerable<T>>> func)	=> Aggregate(layerState, ExtensionMethods.EnumerableOf(seed), (result, ruleset, state) => result.SelectMany(res => func(res, ruleset, state)).Where(res => res is not null));
		
		private (DequeRuleset, object) DequeForLayingOut(LayerState state) => (rulesets[0].Value, state.states[0]);
		private (DequeRuleset, object) DequeForStoring(LayerState state) => (rulesets[^1].Value, state.states[^1]);
		private Func<LayerState, (DequeRuleset, object)> DequeForFetching(InventoryStorable card) => state => Zip(state).First(zip => zip.Item1.CanFetch(zip.Item2, card));
		
		public LayerState GetNewState()
		{
			LayerState state = new();
			state.states = rulesets.Select(ruleset => ruleset.Value.GetNewState()).ToArray();
			state.Inventory = new CallbackList<Storable>();
			return state;
		}
		
		public string GetDequeNamePart(bool isFirst, bool isLast, bool isPlural) => Rulesets.Enumerate().Aggregate("", (current, zip) => current + zip.item.GetDequeNamePart(isFirst && zip.index == 0, isLast && zip.index == rulesets.Length - 1, isPlural));
		
		public IEnumerable<DequeSettingsPageLayout> GetNewSettingsPageLayouts(bool isFirst, bool isLast) =>
			rulesets.Enumerate().SelectMany(zip => zip.item.Value.GetNewSettingsPageLayouts(isFirst && zip.index == 0, isLast && zip.index == rulesets.Length - 1));
		
		public IEnumerable<Texture2D> GetCardTextures() => Rulesets.SelectMany(ruleset => ruleset.GetCardTextures());
		public IEnumerable<Texture2D> GetBoxTextures() => Rulesets.SelectMany(ruleset => ruleset.GetBoxTextures());
	}
	
	[Serializable]
	public class LayerState : InventoryState, DirectionState
	{
		public object[] states;
		
		private CallbackList<Storable> inventory;
		public CallbackList<Storable> Inventory
		{
			get => inventory;
			set
			{
				inventory = value;
				foreach (object state in states)
					if (state is InventoryState inventoryState)
						inventoryState.Inventory = inventory;
			}
		}
		
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

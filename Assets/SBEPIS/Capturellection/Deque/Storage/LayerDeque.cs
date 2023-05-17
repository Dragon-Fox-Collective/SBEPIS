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
		
		public void SetupPage(LayerState layerState, DiajectorPage page) => Zip(layerState).ForEach((ruleset, state) => ruleset.SetupPage(state, page));
		
		public void Tick(LayerState layerState, float deltaTime) => Zip(layerState).ForEach((ruleset, state) => ruleset.Tick(state, deltaTime));
		public void Layout(LayerState layerState) => DoOn(DequeForLayingOut, layerState, (ruleset, state) => ruleset.Layout(state));
		public Vector3 GetMaxPossibleSizeOf(LayerState layerState) => DoOn(DequeForLayingOut, layerState, (ruleset, state) => ruleset.GetMaxPossibleSizeOf(state));
		
		public bool CanFetch(LayerState state, InventoryStorable card) => Zip(state).Any(zip => zip.Item1.CanFetch(zip.Item2, card));
		
		public UniTask<DequeStoreResult>		StoreItem			(LayerState layerState, Capturellectable item)								=> DoOn(DequeForStoring, layerState,						(ruleset, state)							=> ruleset.StoreItem		(state, item));
		public UniTask<DequeStoreResult>		StoreItemHook		(LayerState layerState, Capturellectable item, DequeStoreResult oldResult)	=> Aggregate(layerState, oldResult,							(result, ruleset, state)					=> ruleset.StoreItemHook	(state, item, result));
		public UniTask<Capturellectable>		FetchItem			(LayerState layerState, InventoryStorable card)								=> DoOn(DequeForFetching(card), layerState,			(ruleset, state)							=> ruleset.FetchItem		(state, card));
		public UniTask<Capturellectable>		FetchItemHook		(LayerState layerState, InventoryStorable card, Capturellectable oldItem)	=> Aggregate(layerState, oldItem,							(result, ruleset, state)	=> ruleset.FetchItemHook	(state, card, result));
		public UniTask							FlushCard			(LayerState layerState, Storable storable)									=> DoOn(DequeForStoring, layerState,						(ruleset, state)							=> ruleset.FlushCard		(state, storable));
		public UniTask<IEnumerable<Storable>>	FlushCardPreHook	(LayerState layerState, Storable storable)									=> AggregateExponential(layerState, storable,				(result, ruleset, state)			=> ruleset.FlushCardPreHook	(state, result));
		public UniTask							FlushCardPostHook	(LayerState layerState, Storable storable)									=> Zip(layerState).ForEach(									(ruleset, state)								=> ruleset.FlushCardPostHook(state, storable));
		public UniTask<InventoryStorable>		FetchCard			(LayerState layerState, InventoryStorable card)								=> DoOn(DequeForFetching(card), layerState,			(ruleset, state)							=> ruleset.FetchCard		(state, card));
		public UniTask<InventoryStorable>		FetchCardHook		(LayerState layerState, InventoryStorable oldCard)							=> Aggregate(layerState, oldCard,							(result, ruleset, state)	=> ruleset.FetchCardHook	(state, result));
		public UniTask							Interact<TState>	(LayerState layerState, InventoryStorable card, DequeRuleset targetDeque, DequeInteraction<TState> action) => Zip(layerState).ForEach(	(ruleset, state)								=> ruleset.Interact			(state, card, targetDeque, action));
		public IEnumerable<Storable>			LoadCardPreHook		(LayerState layerState, Storable storable)									=> AggregateExponential(layerState, storable,				(result, ruleset, state)			=> ruleset.LoadCardPreHook	(state, result));
		public void								LoadCardPostHook	(LayerState layerState, Storable storable)									=> Zip(layerState).ForEach(									(ruleset, state)								=> ruleset.LoadCardPostHook	(state, storable));
		public InventoryStorable				SaveCardHook		(LayerState layerState, InventoryStorable oldCard)							=> Aggregate(layerState, oldCard,							(result, ruleset, state)	=> ruleset.SaveCardHook		(state, result));
		
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
			state.Inventory = new List<Storable>();
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
		
		private List<Storable> inventory;
		public List<Storable> Inventory
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

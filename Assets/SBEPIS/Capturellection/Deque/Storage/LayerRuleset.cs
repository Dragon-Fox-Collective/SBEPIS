using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace SBEPIS.Capturellection.Storage
{
	public class LayerRuleset : DequeRuleset<LayerState>
	{
		[SerializeField] private DequeRuleset[] rulesets;
		
		public override IEnumerable<DequeRuleset> Layer => rulesets.SelectMany(ruleset => ruleset.Layer);
		
		public void Init(IEnumerable<DequeRuleset> layer)
		{
			if (rulesets != null && rulesets.Any()) throw new InvalidOperationException($"LayerRuleset {this} has already been initialized");
			rulesets = layer.ToArray();
		}
		
		protected override void InitPage(LayerState layerState, DiajectorPage page) => Zip(layerState).ForEach((ruleset, state) => ruleset.InitPage(state, page));
		
		protected override void Tick(LayerState layerState, float deltaTime) => Zip(layerState).ForEach((ruleset, state) => ruleset.Tick(state, deltaTime));
		protected override void Layout(LayerState layerState) => DoOn(DequeForLayingOut, layerState, (ruleset, state) => ruleset.Layout(state));
		protected override Vector3 GetMaxPossibleSizeOf(LayerState layerState) => DoOn(DequeForLayingOut, layerState, (ruleset, state) => ruleset.GetMaxPossibleSizeOf(state));
		
		protected override bool CanFetch(LayerState state, InventoryStorable card) => Zip(state).Any(zip => zip.Item1.CanFetch(zip.Item2, card));
		
		protected override UniTask<StoreResult>				StoreItem			(LayerState layerState, Capturellectable item)								=> DoOn(DequeForStoring, layerState,						(ruleset, state)							=> ruleset.StoreItem		(state, item));
		protected override UniTask<StoreResult>				StoreItemHook		(LayerState layerState, Capturellectable item, StoreResult oldResult)		=> Aggregate(layerState, oldResult,							(result, ruleset, state)					=> ruleset.StoreItemHook	(state, item, result));
		protected override UniTask<FetchResult>				FetchItem			(LayerState layerState, InventoryStorable card)								=> DoOn(DequeForFetching(card), layerState,			(ruleset, state)							=> ruleset.FetchItem		(state, card));
		protected override UniTask<FetchResult>				FetchItemHook		(LayerState layerState, InventoryStorable card, FetchResult oldResult)		=> Aggregate(layerState, oldResult,							(result, ruleset, state)					=> ruleset.FetchItemHook	(state, card, result));
		protected override UniTask							Interact<TState>	(LayerState layerState, InventoryStorable card, DequeRuleset targetDeque, DequeInteraction<TState> action) => Zip(layerState).ForEach(	(ruleset, state)								=> ruleset.Interact			(state, card, targetDeque, action));
		protected override IEnumerable<InventoryStorable>	LoadCardHook		(LayerState layerState, InventoryStorable card)								=> AggregateExponential(layerState, card,					(result, ruleset, state)	=> ruleset.LoadCardHook		(state, result));
		protected override IEnumerable<InventoryStorable>	SaveCardHook		(LayerState layerState, InventoryStorable card)								=> AggregateExponential(layerState, card,					(result, ruleset, state)	=> ruleset.SaveCardHook		(state, result));
		protected override IEnumerable<Storable>			LoadStorableHook	(LayerState layerState, Storable storable)									=> AggregateExponential(layerState, storable,				(result, ruleset, state)			=> ruleset.LoadStorableHook	(state, result));
		protected override IEnumerable<Storable>			SaveStorableHook	(LayerState layerState, Storable storable)									=> AggregateExponential(layerState, storable,				(result, ruleset, state)			=> ruleset.SaveStorableHook	(state, result));
		
		private static T DoOn<T>(Func<LayerState, (DequeRuleset, object)> getter, LayerState state, Func<DequeRuleset, object, T> func) => func.InvokeWith(getter(state));
		private static void DoOn(Func<LayerState, (DequeRuleset, object)> getter, LayerState state, Action<DequeRuleset, object> func) => func.InvokeWith(getter(state));
		private IEnumerable<(DequeRuleset, object)> Zip(LayerState state) => rulesets.Zip(state.states).Reverse();
		private T Aggregate<T>(LayerState state, T seed, Func<T, DequeRuleset, object, T> func)						=> Zip(state).Aggregate(seed,      (result, zip) => func(result, zip.Item1, zip.Item2));
		private UniTask<T> Aggregate<T>(LayerState state, T seed, Func<T, DequeRuleset, object, UniTask<T>> func)	=> Zip(state).Aggregate(seed, (result, zip) => func(result, zip.Item1, zip.Item2));
		private IEnumerable<T> AggregateExponential<T>(LayerState layerState, T seed, Func<T, DequeRuleset, object, IEnumerable<T>> func)					=> Aggregate(layerState, ExtensionMethods.EnumerableOf(seed), (result, ruleset, state) => result.SelectMany(res => func(res, ruleset, state)).Where(res => res is not null));
		private UniTask<IEnumerable<T>> AggregateExponential<T>(LayerState layerState, T seed, Func<T, DequeRuleset, object, UniTask<IEnumerable<T>>> func)	=> Aggregate(layerState, ExtensionMethods.EnumerableOf(seed), (result, ruleset, state) => result.SelectMany(res => func(res, ruleset, state)).Where(res => res is not null));
		
		private (DequeRuleset, object) DequeForLayingOut(LayerState state) => (rulesets[0], state.states[0]);
		private (DequeRuleset, object) DequeForStoring(LayerState state) => (rulesets[^1], state.states[^1]);
		private Func<LayerState, (DequeRuleset, object)> DequeForFetching(InventoryStorable card) => state => Zip(state).First(zip => zip.Item1.CanFetch(zip.Item2, card));
		
		public override object GetNewState()
		{
			LayerState state = new();
			state.states = rulesets.Select(ruleset => ruleset.GetNewState()).ToArray();
			state.Inventory = new CallbackList<Storable>();
			return state;
		}
		
		public override string GetDequeNamePart(bool isFirst, bool isLast, bool isPlural) => rulesets.Enumerate().Aggregate("", (current, zip) => current + zip.item.GetDequeNamePart(isFirst && zip.index == 0, isLast && zip.index == rulesets.Length - 1, isPlural));
		
		public override IEnumerable<DequeSettingsPageLayout> GetNewSettingsPageLayouts(bool isFirst, bool isLast) =>
			rulesets.Enumerate().SelectMany(zip => zip.item.GetNewSettingsPageLayouts(isFirst && zip.index == 0, isLast && zip.index == rulesets.Length - 1));
		
		public override IEnumerable<Texture2D> GetCardTextures() => rulesets.SelectMany(ruleset => ruleset.GetCardTextures());
		public override IEnumerable<Texture2D> GetBoxTextures() => rulesets.SelectMany(ruleset => ruleset.GetBoxTextures());
		
		public override DequeRuleset Duplicate(Transform parent)
		{
			GameObject newGameObject = new();
			newGameObject.transform.SetParent(parent);
			LayerRuleset newRuleset = newGameObject.AddComponent<LayerRuleset>();
			newRuleset.Init(rulesets.Select(ruleset => ruleset.Duplicate(newGameObject.transform)));
			return newRuleset;
		}
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

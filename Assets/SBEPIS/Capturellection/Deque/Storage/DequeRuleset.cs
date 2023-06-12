using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace SBEPIS.Capturellection.Storage
{
	// congrats! interfaces aren't serializable. this means that you can't instantiate gameobjects that have code-set references. nerd.
	public abstract class DequeRuleset : MonoBehaviour
	{
		public abstract IEnumerable<DequeRuleset> Layer { get; }
		
		public abstract void InitPage(object state, DiajectorPage page);
		
		public abstract void Tick(object state, float deltaTime);
		public abstract void Layout(object state);
		public abstract Vector3 GetMaxPossibleSizeOf(object state);
		
		public abstract bool CanFetch(object state, InventoryStorable card);
		
		public abstract UniTask<StoreResult> StoreItem(object state, Capturellectable item);
		public abstract UniTask<StoreResult> StoreItemHook(object state, Capturellectable item, StoreResult oldResult);
		
		public abstract UniTask<FetchResult> FetchItem(object state, InventoryStorable card);
		public abstract UniTask<FetchResult> FetchItemHook(object state, InventoryStorable card, FetchResult oldResult);
		
		public abstract UniTask Interact<TState>(object state, InventoryStorable card, DequeRuleset targetDeque, DequeInteraction<TState> action);
		
		public abstract IEnumerable<InventoryStorable> LoadCardHook(object state, InventoryStorable card);
		public abstract IEnumerable<InventoryStorable> SaveCardHook(object state, InventoryStorable card);
		
		public abstract IEnumerable<Storable> LoadStorableHook(object state, Storable storable);
		public abstract IEnumerable<Storable> SaveStorableHook(object state, Storable storable);
		
		public abstract object GetNewState();
		
		public abstract string GetDequeNamePart(bool isFirst, bool isLast, bool isPlural);
		
		public abstract IEnumerable<DequeSettingsPageLayout> GetNewSettingsPageLayouts(bool isFirst, bool isLast);
		
		public abstract IEnumerable<Texture2D> GetCardTextures();
		public abstract IEnumerable<Texture2D> GetBoxTextures();
		
		public abstract DequeRuleset Duplicate(Transform parent);
	}
	
	public abstract class DequeRuleset<TState> : DequeRuleset where TState : InventoryState, DirectionState
	{
		protected abstract void InitPage(TState state, DiajectorPage page);
		
		protected abstract void Tick(TState state, float deltaTime);
		protected abstract void Layout(TState state);
		protected abstract Vector3 GetMaxPossibleSizeOf(TState state);
		
		protected abstract bool CanFetch(TState state, InventoryStorable card);
		
		protected abstract UniTask<StoreResult> StoreItem(TState state, Capturellectable item);
		protected abstract UniTask<StoreResult> StoreItemHook(TState state, Capturellectable item, StoreResult oldResult);
		
		protected abstract UniTask<FetchResult> FetchItem(TState state, InventoryStorable card);
		protected abstract UniTask<FetchResult> FetchItemHook(TState state, InventoryStorable card, FetchResult oldResult);
		
		protected abstract UniTask Interact<TIState>(TState state, InventoryStorable card, DequeRuleset targetDeque, DequeInteraction<TIState> action);
		
		protected abstract IEnumerable<InventoryStorable> LoadCardHook(TState state, InventoryStorable card);
		protected abstract IEnumerable<InventoryStorable> SaveCardHook(TState state, InventoryStorable card);
		
		protected abstract IEnumerable<Storable> LoadStorableHook(TState state, Storable storable);
		protected abstract IEnumerable<Storable> SaveStorableHook(TState state, Storable storable);
		
		// Explicit implementations
		
		public override void InitPage(object state, DiajectorPage page) => InitPage((TState)state, page);
		
		public override void Tick(object state, float deltaTime) => Tick((TState)state, deltaTime);
		public override void Layout(object state) => Layout((TState)state);
		public override Vector3 GetMaxPossibleSizeOf(object state) => GetMaxPossibleSizeOf((TState)state);
		
		public override bool CanFetch(object state, InventoryStorable card) => CanFetch((TState)state, card);
		
		public override UniTask<StoreResult> StoreItem(object state, Capturellectable item) => StoreItem((TState)state, item);
		public override UniTask<StoreResult> StoreItemHook(object state, Capturellectable item, StoreResult oldResult) => StoreItemHook((TState)state, item, oldResult);
		
		public override UniTask<FetchResult> FetchItem(object state, InventoryStorable card) => FetchItem((TState)state, card);
		public override UniTask<FetchResult> FetchItemHook(object state, InventoryStorable card, FetchResult oldResult) => FetchItemHook((TState)state, card, oldResult);
		
		public override UniTask Interact<TIState>(object state, InventoryStorable card, DequeRuleset targetDeque, DequeInteraction<TIState> action) => Interact((TState)state, card, targetDeque, action);
		
		public override IEnumerable<InventoryStorable> LoadCardHook(object state, InventoryStorable card) => LoadCardHook((TState)state, card);
		public override IEnumerable<InventoryStorable> SaveCardHook(object state, InventoryStorable card) => SaveCardHook((TState)state, card);
		
		public override IEnumerable<Storable> LoadStorableHook(object state, Storable storable) => LoadStorableHook((TState)state, storable);
		public override IEnumerable<Storable> SaveStorableHook(object state, Storable storable) => SaveStorableHook((TState)state, storable);
	}
	
	public delegate UniTask DequeInteraction<in TState>(TState state, InventoryStorable card);
}

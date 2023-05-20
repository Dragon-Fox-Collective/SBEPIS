using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace SBEPIS.Capturellection.Storage
{
	public interface DequeRuleset
	{
		public void InitPage(object state, DiajectorPage page);
		
		public void Tick(object state, float deltaTime);
		public void Layout(object state);
		public Vector3 GetMaxPossibleSizeOf(object state);
		
		public bool CanFetch(object state, InventoryStorable card);
		
		public UniTask<DequeStoreResult> StoreItem(object state, Capturellectable item);
		public UniTask<DequeStoreResult> StoreItemHook(object state, Capturellectable item, DequeStoreResult oldResult);
		
		public UniTask<Capturellectable> FetchItem(object state, InventoryStorable card);
		public UniTask<Capturellectable> FetchItemHook(object state, InventoryStorable card, Capturellectable oldItem);
		
		public UniTask Interact<TState>(object state, InventoryStorable card, DequeRuleset targetDeque, DequeInteraction<TState> action);
		
		public IEnumerable<InventoryStorable> LoadCardPreHook(object state, InventoryStorable card);
		
		public IEnumerable<InventoryStorable> SaveCardPostHook(object state, InventoryStorable card);
		
		public IEnumerable<Storable> LoadStorablePreHook(object state, Storable storable);
		
		public IEnumerable<Storable> SaveStorablePostHook(object state, Storable storable);
		
		public object GetNewState();
		
		public string GetDequeNamePart(bool isFirst, bool isLast, bool isPlural);
		
		public IEnumerable<DequeSettingsPageLayout> GetNewSettingsPageLayouts(bool isFirst, bool isLast);
		
		public IEnumerable<Texture2D> GetCardTextures();
		public IEnumerable<Texture2D> GetBoxTextures();
	}
	
	public interface DequeRuleset<TState> : DequeRuleset where TState : InventoryState, DirectionState
	{
		public void InitPage(TState state, DiajectorPage page);
		
		public void Tick(TState state, float deltaTime);
		public void Layout(TState state);
		public Vector3 GetMaxPossibleSizeOf(TState state);
		
		public bool CanFetch(TState state, InventoryStorable card);
		
		public UniTask<DequeStoreResult> StoreItem(TState state, Capturellectable item);
		public UniTask<DequeStoreResult> StoreItemHook(TState state, Capturellectable item, DequeStoreResult oldResult);
		
		public UniTask<Capturellectable> FetchItem(TState state, InventoryStorable card);
		public UniTask<Capturellectable> FetchItemHook(TState state, InventoryStorable card, Capturellectable oldItem);
		
		public UniTask Interact<TIState>(TState state, InventoryStorable card, DequeRuleset targetDeque, DequeInteraction<TIState> action);
		
		public InventoryStorable LoadCardPostHook(TState state, InventoryStorable card);
		
		public InventoryStorable SaveCardPostHook(TState state, InventoryStorable card);
		
		public IEnumerable<Storable> LoadStorablePreHook(TState state, Storable storable);
		public Storable LoadStorablePostHook(TState state, Storable storable);
		
		public IEnumerable<Storable> SaveStorablePreHook(TState state, Storable storable);
		public Storable SaveStorablePostHook(TState state, Storable storable);
		
		public new TState GetNewState();
		
		// Explicit implementations
		
		void DequeRuleset.InitPage(object state, DiajectorPage page) => InitPage((TState)state, page);
		
		void DequeRuleset.Tick(object state, float deltaTime) => Tick((TState)state, deltaTime);
		void DequeRuleset.Layout(object state) => Layout((TState)state);
		Vector3 DequeRuleset.GetMaxPossibleSizeOf(object state) => GetMaxPossibleSizeOf((TState)state);
		
		bool DequeRuleset.CanFetch(object state, InventoryStorable card) => CanFetch((TState)state, card);
		
		UniTask<DequeStoreResult> DequeRuleset.StoreItem(object state, Capturellectable item) => StoreItem((TState)state, item);
		UniTask<DequeStoreResult> DequeRuleset.StoreItemHook(object state, Capturellectable item, DequeStoreResult oldResult) => StoreItemHook((TState)state, item, oldResult);
		
		UniTask<Capturellectable> DequeRuleset.FetchItem(object state, InventoryStorable card) => FetchItem((TState)state, card);
		UniTask<Capturellectable> DequeRuleset.FetchItemHook(object state, InventoryStorable card, Capturellectable oldItem) => FetchItemHook((TState)state, card, oldItem);
		
		UniTask DequeRuleset.Interact<TIState>(object state, InventoryStorable card, DequeRuleset targetDeque, DequeInteraction<TIState> action) => Interact((TState)state, card, targetDeque, action);
		
		InventoryStorable DequeRuleset.LoadCardPostHook(object state, InventoryStorable card) => LoadCardPostHook((TState)state, card);
		
		InventoryStorable DequeRuleset.SaveCardPostHook(object state, InventoryStorable card) => SaveCardPostHook((TState)state, card);
		
		IEnumerable<Storable> DequeRuleset.LoadStorablePreHook(object state, Storable storable) => LoadStorablePreHook((TState)state, storable);
		Storable DequeRuleset.LoadStorablePostHook(object state, Storable storable) => SaveStorablePostHook((TState)state, storable);
		
		IEnumerable<Storable> DequeRuleset.SaveStorablePreHook(object state, Storable storable) => SaveStorablePreHook((TState)state, storable);
		Storable DequeRuleset.SaveStorablePostHook(object state, Storable storable) => SaveStorablePostHook((TState)state, storable);
		
		object DequeRuleset.GetNewState() => GetNewState();
	}
	
	public delegate UniTask DequeInteraction<in TState>(TState state, InventoryStorable card);
	
	public struct DequeStoreResult
	{
		public InventoryStorable card;
		public CaptureContainer container;
		public Capturellectable ejectedItem;
		public int flushIndex;
		public Storable storable;
		
		public DequeStoreResult(InventoryStorable card, CaptureContainer container, Capturellectable ejectedItem, int flushIndex, Storable storable)
		{
			this.card = card;
			this.container = container;
			this.ejectedItem = ejectedItem;
			this.flushIndex = flushIndex;
			this.storable = storable;
		}
		
		public StorableStoreResult ToStorableResult() => new(card, container, ejectedItem);
	}
}

using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace SBEPIS.Capturellection.Storage
{
	public interface DequeRuleset
	{
		public void SetupPage(object state, DiajectorPage page);
		
		public void Tick(object state, float deltaTime);
		public void Layout(object state);
		public Vector3 GetMaxPossibleSizeOf(object state);
		
		public bool CanFetch(object state, InventoryStorable card);
		
		public UniTask<DequeStoreResult> StoreItem(object state, Capturellectable item);
		public UniTask<DequeStoreResult> StoreItemHook(object state, Capturellectable item, DequeStoreResult oldResult);
		
		public UniTask<Capturellectable> FetchItem(object state, InventoryStorable card);
		public UniTask<Capturellectable> FetchItemHook(object state, InventoryStorable card, Capturellectable oldItem);
		
		public UniTask FlushCard(object state, Storable storable);
		public UniTask<IEnumerable<Storable>> FlushCardPreHook(object state, Storable storable);
		public UniTask FlushCardPostHook(object state, Storable storable);
		
		public UniTask<InventoryStorable> FetchCard(object state, InventoryStorable card);
		public UniTask<InventoryStorable> FetchCardHook(object state, InventoryStorable card);
		
		public UniTask Interact<TState>(object state, InventoryStorable card, DequeRuleset targetDeque, DequeInteraction<TState> action);
		
		public IEnumerable<Storable> LoadCardPreHook(object state, Storable storable);
		public void LoadCardPostHook(object state, Storable storable);
		
		public InventoryStorable SaveCardHook(object state, InventoryStorable card);
		
		public object GetNewState();
		
		public string GetDequeNamePart(bool isFirst, bool isLast, bool isPlural);
		
		public IEnumerable<DequeSettingsPageLayout> GetNewSettingsPageLayouts(bool isFirst, bool isLast);
		
		public IEnumerable<Texture2D> GetCardTextures();
		public IEnumerable<Texture2D> GetBoxTextures();
	}
	
	public interface DequeRuleset<TState> : DequeRuleset where TState : InventoryState, DirectionState
	{
		public void SetupPage(TState state, DiajectorPage page);
		
		public void Tick(TState state, float deltaTime);
		public void Layout(TState state);
		public Vector3 GetMaxPossibleSizeOf(TState state);
		
		public bool CanFetch(TState state, InventoryStorable card);
		
		public UniTask<DequeStoreResult> StoreItem(TState state, Capturellectable item);
		public UniTask<DequeStoreResult> StoreItemHook(TState state, Capturellectable item, DequeStoreResult oldResult);
		
		public UniTask<Capturellectable> FetchItem(TState state, InventoryStorable card);
		public UniTask<Capturellectable> FetchItemHook(TState state, InventoryStorable card, Capturellectable oldItem);
		
		public UniTask FlushCard(TState state, Storable storable);
		public UniTask<IEnumerable<Storable>> FlushCardPreHook(TState state, Storable storable);
		public UniTask FlushCardPostHook(TState state, Storable storable);
		
		public UniTask<InventoryStorable> FetchCard(TState state, InventoryStorable card);
		public UniTask<InventoryStorable> FetchCardHook(TState state, InventoryStorable card);
		
		public UniTask Interact<TIState>(TState state, InventoryStorable card, DequeRuleset targetDeque, DequeInteraction<TIState> action);
		
		public IEnumerable<Storable> LoadCardPreHook(TState state, Storable storable);
		public void LoadCardPostHook(TState state, Storable storable);
		
		public InventoryStorable SaveCardHook(TState state, InventoryStorable card);
		
		public new TState GetNewState();
		
		// Explicit implementations
		
		void DequeRuleset.SetupPage(object state, DiajectorPage page) => SetupPage((TState)state, page);
		
		void DequeRuleset.Tick(object state, float deltaTime) => Tick((TState)state, deltaTime);
		void DequeRuleset.Layout(object state) => Layout((TState)state);
		Vector3 DequeRuleset.GetMaxPossibleSizeOf(object state) => GetMaxPossibleSizeOf((TState)state);
		
		bool DequeRuleset.CanFetch(object state, InventoryStorable card) => CanFetch((TState)state, card);
		
		UniTask<DequeStoreResult> DequeRuleset.StoreItem(object state, Capturellectable item) => StoreItem((TState)state, item);
		UniTask<DequeStoreResult> DequeRuleset.StoreItemHook(object state, Capturellectable item, DequeStoreResult oldResult) => StoreItemHook((TState)state, item, oldResult);
		
		UniTask<Capturellectable> DequeRuleset.FetchItem(object state, InventoryStorable card) => FetchItem((TState)state, card);
		UniTask<Capturellectable> DequeRuleset.FetchItemHook(object state, InventoryStorable card, Capturellectable oldItem) => FetchItemHook((TState)state, card, oldItem);
		
		UniTask DequeRuleset.FlushCard(object state, Storable storable) => FlushCard((TState)state, storable);
		UniTask<IEnumerable<Storable>> DequeRuleset.FlushCardPreHook(object state, Storable storable) => FlushCardPreHook((TState)state, storable);
		UniTask DequeRuleset.FlushCardPostHook(object state, Storable storable) => FlushCardPostHook((TState)state, storable);
		
		UniTask<InventoryStorable> DequeRuleset.FetchCard(object state, InventoryStorable card) => FetchCard((TState)state, card);
		UniTask<InventoryStorable> DequeRuleset.FetchCardHook(object state, InventoryStorable card) => FetchCardHook((TState)state, card);
		
		UniTask DequeRuleset.Interact<TIState>(object state, InventoryStorable card, DequeRuleset targetDeque, DequeInteraction<TIState> action) => Interact((TState)state, card, targetDeque, action);
		
		IEnumerable<Storable> DequeRuleset.LoadCardPreHook(object state, Storable storable) => LoadCardPreHook((TState)state, storable);
		void DequeRuleset.LoadCardPostHook(object state, Storable storable) => LoadCardPostHook((TState)state, storable);
		
		InventoryStorable DequeRuleset.SaveCardHook(object state, InventoryStorable card) => SaveCardHook((TState)state, card);
		
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

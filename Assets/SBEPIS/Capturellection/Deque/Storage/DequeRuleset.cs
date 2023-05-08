using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace SBEPIS.Capturellection.Storage
{
	public interface DequeRuleset
	{
		public void Tick(List<Storable> inventory, DequeRulesetState state, float deltaTime);
		public Vector3 GetMaxPossibleSizeOf(List<Storable> inventory, DequeRulesetState state);
		
		public bool CanFetchFrom(List<Storable> inventory, DequeRulesetState state, InventoryStorable card);
		
		public UniTask<DequeStoreResult> StoreItem(List<Storable> inventory, DequeRulesetState state, Capturellectable item);
		public UniTask<DequeStoreResult> StoreItemHook(List<Storable> inventory, DequeRulesetState state, Capturellectable item, DequeStoreResult oldResult);
		
		public UniTask<Capturellectable> FetchItem(List<Storable> inventory, DequeRulesetState state, InventoryStorable card);
		public UniTask<Capturellectable> FetchItemHook(List<Storable> inventory, DequeRulesetState state, InventoryStorable card, Capturellectable oldItem);
		
		public UniTask FlushCard(List<Storable> inventory, DequeRulesetState state, Storable storable);
		public UniTask<IEnumerable<Storable>> FlushCardPreHook(List<Storable> inventory, DequeRulesetState state, Storable storable);
		public UniTask FlushCardPostHook(List<Storable> inventory, DequeRulesetState state, Storable storable);
		
		public UniTask<InventoryStorable> FetchCard(List<Storable> inventory, DequeRulesetState state, InventoryStorable card);
		public UniTask<InventoryStorable> FetchCardHook(List<Storable> inventory, DequeRulesetState state, InventoryStorable card);
		
		public IEnumerable<Storable> LoadCardPreHook(List<Storable> inventory, DequeRulesetState state, Storable storable);
		public void LoadCardPostHook(List<Storable> inventory, DequeRulesetState state, Storable storable);
		
		public InventoryStorable SaveCardHook(List<Storable> inventory, DequeRulesetState state, InventoryStorable card);
		
		public DequeRulesetState GetNewState();
		
		public string GetDequeNamePart(bool isFirst, bool isLast, bool isPlural);
		
		public IEnumerable<DequeSettingsPageLayout> GetNewSettingsPageLayouts(bool isFirst, bool isLast);
		
		public IEnumerable<Texture2D> GetCardTextures();
		public IEnumerable<Texture2D> GetBoxTextures();
	}
	
	public interface DequeRuleset<TState> : DequeRuleset where TState : DequeRulesetState
	{
		public void Tick(List<Storable> inventory, TState state, float deltaTime);
		public Vector3 GetMaxPossibleSizeOf(List<Storable> inventory, TState state);
		
		public bool CanFetchFrom(List<Storable> inventory, TState state, InventoryStorable card);
		
		public UniTask<DequeStoreResult> StoreItem(List<Storable> inventory, TState state, Capturellectable item);
		public UniTask<DequeStoreResult> StoreItemHook(List<Storable> inventory, TState state, Capturellectable item, DequeStoreResult oldResult);
		
		public UniTask<Capturellectable> FetchItem(List<Storable> inventory, TState state, InventoryStorable card);
		public UniTask<Capturellectable> FetchItemHook(List<Storable> inventory, TState state, InventoryStorable card, Capturellectable oldItem);
		
		public UniTask FlushCard(List<Storable> inventory, TState state, Storable storable);
		public UniTask<IEnumerable<Storable>> FlushCardPreHook(List<Storable> inventory, TState state, Storable storable);
		public UniTask FlushCardPostHook(List<Storable> inventory, TState state, Storable storable);
		
		public UniTask<InventoryStorable> FetchCard(List<Storable> inventory, TState state, InventoryStorable card);
		public UniTask<InventoryStorable> FetchCardHook(List<Storable> inventory, TState state, InventoryStorable card);
		
		public IEnumerable<Storable> LoadCardPreHook(List<Storable> inventory, TState state, Storable storable);
		public void LoadCardPostHook(List<Storable> inventory, TState state, Storable storable);
		
		public InventoryStorable SaveCardHook(List<Storable> inventory, TState state, InventoryStorable card);
		
		public new TState GetNewState();
		
		// Explicit implementations
		
		void DequeRuleset.Tick(List<Storable> inventory, DequeRulesetState state, float deltaTime) => Tick(inventory, (TState)state, deltaTime);
		Vector3 DequeRuleset.GetMaxPossibleSizeOf(List<Storable> inventory, DequeRulesetState state) => GetMaxPossibleSizeOf(inventory, (TState)state);
		
		bool DequeRuleset.CanFetchFrom(List<Storable> inventory, DequeRulesetState state, InventoryStorable card) => CanFetchFrom(inventory, (TState)state, card);
		
		UniTask<DequeStoreResult> DequeRuleset.StoreItem(List<Storable> inventory, DequeRulesetState state, Capturellectable item) => StoreItem(inventory, (TState)state, item);
		UniTask<DequeStoreResult> DequeRuleset.StoreItemHook(List<Storable> inventory, DequeRulesetState state, Capturellectable item, DequeStoreResult oldResult) => StoreItemHook(inventory, (TState)state, item, oldResult);
		
		UniTask<Capturellectable> DequeRuleset.FetchItem(List<Storable> inventory, DequeRulesetState state, InventoryStorable card) => FetchItem(inventory, (TState)state, card);
		UniTask<Capturellectable> DequeRuleset.FetchItemHook(List<Storable> inventory, DequeRulesetState state, InventoryStorable card, Capturellectable oldItem) => FetchItemHook(inventory, (TState)state, card, oldItem);
		
		UniTask DequeRuleset.FlushCard(List<Storable> inventory, DequeRulesetState state, Storable storable) => FlushCard(inventory, (TState)state, storable);
		UniTask<IEnumerable<Storable>> DequeRuleset.FlushCardPreHook(List<Storable> inventory, DequeRulesetState state, Storable storable) => FlushCardPreHook(inventory, (TState)state, storable);
		UniTask DequeRuleset.FlushCardPostHook(List<Storable> inventory, DequeRulesetState state, Storable storable) => FlushCardPostHook(inventory, (TState)state, storable);
		
		UniTask<InventoryStorable> DequeRuleset.FetchCard(List<Storable> inventory, DequeRulesetState state, InventoryStorable card) => FetchCard(inventory, (TState)state, card);
		UniTask<InventoryStorable> DequeRuleset.FetchCardHook(List<Storable> inventory, DequeRulesetState state, InventoryStorable card) => FetchCardHook(inventory, (TState)state, card);
		
		IEnumerable<Storable> DequeRuleset.LoadCardPreHook(List<Storable> inventory, DequeRulesetState state, Storable storable) => LoadCardPreHook(inventory, (TState)state, storable);
		void DequeRuleset.LoadCardPostHook(List<Storable> inventory, DequeRulesetState state, Storable storable) => LoadCardPostHook(inventory, (TState)state, storable);
		
		InventoryStorable DequeRuleset.SaveCardHook(List<Storable> inventory, DequeRulesetState state, InventoryStorable card) => SaveCardHook(inventory, (TState)state, card);
		
		DequeRulesetState DequeRuleset.GetNewState() => GetNewState();
	}
	
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

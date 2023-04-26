using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace SBEPIS.Capturellection.Storage
{
	public abstract class DequeRuleset : MonoBehaviour
	{
		public abstract void Tick(List<Storable> inventory, DequeRulesetState state, float deltaTime);
		public abstract Vector3 GetMaxPossibleSizeOf(List<Storable> inventory, DequeRulesetState state);
		
		public abstract bool CanFetchFrom(List<Storable> inventory, DequeRulesetState state, InventoryStorable card);
		
		public abstract UniTask<DequeStoreResult> StoreItem(List<Storable> inventory, DequeRulesetState state, Capturellectable item);
		public virtual UniTask<DequeStoreResult> StoreItemHook(List<Storable> inventory, DequeRulesetState state, Capturellectable item, DequeStoreResult oldResult) => UniTask.FromResult(oldResult);
		
		public abstract UniTask<Capturellectable> FetchItem(List<Storable> inventory, DequeRulesetState state, InventoryStorable card);
		public virtual UniTask<Capturellectable> FetchItemHook(List<Storable> inventory, DequeRulesetState state, InventoryStorable card, Capturellectable oldItem) => UniTask.FromResult(oldItem);
		
		public abstract UniTask FlushCard(List<Storable> inventory, DequeRulesetState state, Storable storable);
		public virtual UniTask<IEnumerable<Storable>> FlushCardPreHook(List<Storable> inventory, DequeRulesetState state, Storable storable) => UniTask.FromResult(ExtensionMethods.EnumerableOf(storable));
		public virtual UniTask FlushCardPostHook(List<Storable> inventory, DequeRulesetState state, Storable storable) => UniTask.CompletedTask;
		
		public abstract UniTask FetchCard(List<Storable> inventory, DequeRulesetState state, InventoryStorable card);
		public virtual UniTask FetchCardHook(List<Storable> inventory, DequeRulesetState state, InventoryStorable card) => UniTask.CompletedTask;
		
		public abstract DequeRulesetState GetNewState();
		
		public abstract string GetDequeNamePart(bool isFirst, bool isLast, bool isPlural);
		
		public abstract IEnumerable<DequeSettingsPageLayout> GetNewSettingsPageLayouts(bool isFirst, bool isLast);
		
		public abstract IEnumerable<Texture2D> GetCardTextures();
		public abstract IEnumerable<Texture2D> GetBoxTextures();
	}
	
	public abstract class DequeRuleset<T> : DequeRuleset where T : DequeRulesetState, new()
	{
		public override void Tick(List<Storable> inventory, DequeRulesetState state, float deltaTime) => Tick(inventory, (T)state, deltaTime);
		public abstract void Tick(List<Storable> inventory, T state, float deltaTime);
		public override Vector3 GetMaxPossibleSizeOf(List<Storable> inventory, DequeRulesetState state) => GetMaxPossibleSizeOf(inventory, (T)state);
		public abstract Vector3 GetMaxPossibleSizeOf(List<Storable> inventory, T state);
		
		public override bool CanFetchFrom(List<Storable> inventory, DequeRulesetState state, InventoryStorable card) => CanFetchFrom(inventory, (T)state, card);
		public abstract bool CanFetchFrom(List<Storable> inventory, T state, InventoryStorable card);
		
		public override UniTask<DequeStoreResult> StoreItem(List<Storable> inventory, DequeRulesetState state, Capturellectable item) => StoreItem(inventory, (T)state, item);
		public abstract UniTask<DequeStoreResult> StoreItem(List<Storable> inventory, T state, Capturellectable item);
		public override UniTask<DequeStoreResult> StoreItemHook(List<Storable> inventory, DequeRulesetState state, Capturellectable item, DequeStoreResult oldResult) => StoreItemHook(inventory, (T)state, item, oldResult);
		public virtual UniTask<DequeStoreResult> StoreItemHook(List<Storable> inventory, T state, Capturellectable item, DequeStoreResult oldResult) => UniTask.FromResult(oldResult);
		
		public override UniTask<Capturellectable> FetchItem(List<Storable> inventory, DequeRulesetState state, InventoryStorable card) => FetchItem(inventory, (T)state, card);
		public abstract UniTask<Capturellectable> FetchItem(List<Storable> inventory, T state, InventoryStorable card);
		public override UniTask<Capturellectable> FetchItemHook(List<Storable> inventory, DequeRulesetState state, InventoryStorable card, Capturellectable oldItem) => FetchItemHook(inventory, (T)state, card, oldItem);
		public virtual UniTask<Capturellectable> FetchItemHook(List<Storable> inventory, T state, InventoryStorable card, Capturellectable oldItem) => UniTask.FromResult(oldItem);
		
		public override UniTask FlushCard(List<Storable> inventory, DequeRulesetState state, Storable storable) => FlushCard(inventory, (T)state, storable);
		public abstract UniTask FlushCard(List<Storable> inventory, T state, Storable storable);
		public override UniTask<IEnumerable<Storable>> FlushCardPreHook(List<Storable> inventory, DequeRulesetState state, Storable storable) => FlushCardPreHook(inventory, (T)state, storable);
		public virtual UniTask<IEnumerable<Storable>> FlushCardPreHook(List<Storable> inventory, T state, Storable storable) => UniTask.FromResult(ExtensionMethods.EnumerableOf(storable));
		public override UniTask FlushCardPostHook(List<Storable> inventory, DequeRulesetState state, Storable storable) => FlushCardPostHook(inventory, (T)state, storable);
		public virtual UniTask FlushCardPostHook(List<Storable> inventory, T state, Storable storable) => UniTask.CompletedTask;
		
		public override UniTask FetchCard(List<Storable> inventory, DequeRulesetState state, InventoryStorable card) => FetchCard(inventory, (T)state, card);
		public abstract UniTask FetchCard(List<Storable> inventory, T state, InventoryStorable card);
		public override UniTask FetchCardHook(List<Storable> inventory, DequeRulesetState state, InventoryStorable card) => FetchCardHook(inventory, (T)state, card);
		public virtual UniTask FetchCardHook(List<Storable> inventory, T state, InventoryStorable card) => UniTask.CompletedTask;
		
		public override DequeRulesetState GetNewState() => new T();
	}
	
	public struct DequeStoreResult
	{
		public InventoryStorable card;
		public CaptureContainer container;
		public Capturellectable ejectedItem;
		public int flushIndex;
		
		public DequeStoreResult(InventoryStorable card, CaptureContainer container, Capturellectable ejectedItem, int flushIndex)
		{
			this.card = card;
			this.container = container;
			this.ejectedItem = ejectedItem;
			this.flushIndex = flushIndex;
		}
		
		public StorableStoreResult ToStorableResult() => new(card, container, ejectedItem);
	}
}

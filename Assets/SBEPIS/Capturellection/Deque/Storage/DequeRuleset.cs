using System;
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
		
		public abstract UniTaskVoid Store(List<Storable> inventory, DequeRulesetState state);
		public abstract UniTaskVoid Flush(List<Storable> inventory, DequeRulesetState state, Storable storable);
		public abstract UniTaskVoid RestoreAfterStore(List<Storable> inventory, DequeRulesetState state, Storable storable, int originalIndex);
		public abstract UniTaskVoid RestoreAfterFetch(List<Storable> inventory, DequeRulesetState state, Storable storable, int originalIndex);
		
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
		
		public override UniTaskVoid Store(List<Storable> inventory, DequeRulesetState state) => Store(inventory, (T)state);
		public virtual UniTaskVoid Store(List<Storable> inventory, T state) => DoVoid(() => StoreSync(inventory, state));
		public virtual void StoreSync(List<Storable> inventory, T state) => throw new InvalidOperationException("Method not overridden");
		public override UniTaskVoid Flush(List<Storable> inventory, DequeRulesetState state, Storable storable) => Flush(inventory, (T)state, storable);
		public virtual UniTaskVoid Flush(List<Storable> inventory, T state, Storable storable) => DoVoid(() => FlushSync(inventory, state, storable));
		public virtual void FlushSync(List<Storable> inventory, T state, Storable storable) => throw new InvalidOperationException("Method not overridden");
		public override UniTaskVoid RestoreAfterStore(List<Storable> inventory, DequeRulesetState state, Storable storable, int originalIndex) => RestoreAfterStore(inventory, (T)state, storable, originalIndex);
		public virtual UniTaskVoid RestoreAfterStore(List<Storable> inventory, T state, Storable storable, int originalIndex) => DoVoid(() => RestoreAfterStoreSync(inventory, state, storable, originalIndex));
		public virtual void RestoreAfterStoreSync(List<Storable> inventory, T state, Storable storable, int originalIndex) => throw new InvalidOperationException("Method not overridden");
		public override UniTaskVoid RestoreAfterFetch(List<Storable> inventory, DequeRulesetState state, Storable storable, int originalIndex) => RestoreAfterFetch(inventory, (T)state, storable, originalIndex);
		public virtual UniTaskVoid RestoreAfterFetch(List<Storable> inventory, T state, Storable storable, int originalIndex) => DoVoid(() => RestoreAfterFetchSync(inventory, state, storable, originalIndex));
		public virtual void RestoreAfterFetchSync(List<Storable> inventory, T state, Storable storable, int originalIndex) => throw new InvalidOperationException("Method not overridden");
		
		private static UniTaskVoid DoVoid(Action action)
		{
			action();
			return new UniTaskVoid();
		}
		
		public override DequeRulesetState GetNewState() => new T();
	}
}

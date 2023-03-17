using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace SBEPIS.Capturllection
{
	public abstract class DequeRuleset : MonoBehaviour
	{
		public abstract void Tick(List<Storable> inventory, DequeRulesetState state, float deltaTime);
		public abstract Vector3 GetMaxPossibleSizeOf(List<Storable> inventory, DequeRulesetState state);
		
		public abstract bool CanFetchFrom(List<Storable> inventory, DequeRulesetState state, DequeStorable card);
		
		public abstract Task<int> GetIndexToStoreInto(List<Storable> inventory, DequeRulesetState state);
		public abstract int GetIndexToFlushBetween(List<Storable> inventory, DequeRulesetState state, Storable storable);
		public abstract int GetIndexToInsertBetweenAfterStore(List<Storable> inventory, DequeRulesetState state, Storable storable, int originalIndex);
		public abstract int GetIndexToInsertBetweenAfterFetch(List<Storable> inventory, DequeRulesetState state, Storable storable, int originalIndex);
		
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
		
		public override bool CanFetchFrom(List<Storable> inventory, DequeRulesetState state, DequeStorable card) => CanFetchFrom(inventory, (T)state, card);
		public abstract bool CanFetchFrom(List<Storable> inventory, T state, DequeStorable card);
		
		public override Task<int> GetIndexToStoreInto(List<Storable> inventory, DequeRulesetState state) => GetIndexToStoreInto(inventory, (T)state);
		public abstract Task<int> GetIndexToStoreInto(List<Storable> inventory, T state);
		public override int GetIndexToFlushBetween(List<Storable> inventory, DequeRulesetState state, Storable storable) => GetIndexToFlushBetween(inventory, (T)state, storable);
		public abstract int GetIndexToFlushBetween(List<Storable> inventory, T state, Storable storable);
		public override int GetIndexToInsertBetweenAfterStore(List<Storable> inventory, DequeRulesetState state, Storable storable, int originalIndex) => GetIndexToInsertBetweenAfterStore(inventory, (T)state, storable, originalIndex);
		public abstract int GetIndexToInsertBetweenAfterStore(List<Storable> inventory, T state, Storable storable, int originalIndex);
		public override int GetIndexToInsertBetweenAfterFetch(List<Storable> inventory, DequeRulesetState state, Storable storable, int originalIndex) => GetIndexToInsertBetweenAfterFetch(inventory, (T)state, storable, originalIndex);
		public abstract int GetIndexToInsertBetweenAfterFetch(List<Storable> inventory, T state, Storable storable, int originalIndex);
		
		public override DequeRulesetState GetNewState() => new T();
	}
}

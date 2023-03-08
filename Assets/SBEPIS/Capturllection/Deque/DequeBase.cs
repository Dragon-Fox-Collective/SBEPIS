using System.Collections.Generic;
using UnityEngine;

namespace SBEPIS.Capturllection
{
	public abstract class DequeBase<T> : DequeRuleset where T : DequeRulesetState
	{
		public Dequeration dequeration;
		
		public override string dequeName => GetType().Name;
		
		public override void Tick(List<Storable> inventory, DequeRulesetState state, float deltaTime, Vector3 direction) => Tick(inventory, (T)state, deltaTime, direction);
		public abstract void Tick(List<Storable> inventory, T state, float deltaTime, Vector3 direction);
		
		public override bool CanFetchFrom(List<Storable> inventory, DequeRulesetState state, DequeStorable card) => CanFetchFrom(inventory, (T)state, card);
		public abstract bool CanFetchFrom(List<Storable> inventory, T state, DequeStorable card);
		
		public override int GetIndexToStoreInto(List<Storable> inventory, DequeRulesetState state) => GetIndexToStoreInto(inventory, (T)state);
		public abstract int GetIndexToStoreInto(List<Storable> inventory, T state);
		public override int GetIndexToFlushBetween(List<Storable> inventory, DequeRulesetState state, Storable storable) => GetIndexToFlushBetween(inventory, (T)state, storable);
		public abstract int GetIndexToFlushBetween(List<Storable> inventory, T state, Storable storable);
		public override int GetIndexToInsertBetweenAfterStore(List<Storable> inventory, DequeRulesetState state, Storable storable, int originalIndex) => GetIndexToInsertBetweenAfterStore(inventory, (T)state, storable, originalIndex);
		public abstract int GetIndexToInsertBetweenAfterStore(List<Storable> inventory, T state, Storable storable, int originalIndex);
		public override int GetIndexToInsertBetweenAfterFetch(List<Storable> inventory, DequeRulesetState state, Storable storable, int originalIndex) => GetIndexToInsertBetweenAfterFetch(inventory, (T)state, storable, originalIndex);
		public abstract int GetIndexToInsertBetweenAfterFetch(List<Storable> inventory, T state, Storable storable, int originalIndex);
		
		public override IEnumerable<Texture2D> GetCardTextures()
		{
			yield return dequeration.cardTexture;
		}
		
		public override IEnumerable<Texture2D> GetBoxTextures()
		{
			yield return dequeration.boxTexture;
		}
	}
}

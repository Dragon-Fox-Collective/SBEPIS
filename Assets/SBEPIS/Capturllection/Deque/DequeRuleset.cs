using System.Collections.Generic;
using UnityEngine;

namespace SBEPIS.Capturllection
{
	public abstract class DequeRuleset : MonoBehaviour
	{
		public abstract string dequeName { get; }
		
		public abstract void Tick(List<Storable> inventory, DequeRulesetState state, float deltaTime, Vector3 direction);
		public abstract Vector3 GetMaxPossibleSizeOf(List<Storable> inventory);
		
		public abstract bool CanFetchFrom(List<Storable> inventory, DequeRulesetState state, DequeStorable card);
		
		public abstract int GetIndexToStoreInto(List<Storable> inventory, DequeRulesetState state);
		public abstract int GetIndexToFlushBetween(List<Storable> inventory, DequeRulesetState state, Storable storable);
		public abstract int GetIndexToInsertBetweenAfterStore(List<Storable> inventory, DequeRulesetState state, Storable storable, int originalIndex);
		public abstract int GetIndexToInsertBetweenAfterFetch(List<Storable> inventory, DequeRulesetState state, Storable storable, int originalIndex);
		
		public abstract IEnumerable<Texture2D> GetCardTextures();
		public abstract IEnumerable<Texture2D> GetBoxTextures();
	}
}

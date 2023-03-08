using System.Collections.Generic;
using UnityEngine;

namespace SBEPIS.Capturllection
{
	public abstract class DequeRuleset : MonoBehaviour
	{
		public abstract string dequeName { get; }
		
		public abstract Vector3 TickAndGetMaxSize(List<Storable> inventory, float deltaTime, Vector3 direction);
		
		public abstract bool CanFetchFrom(List<Storable> inventory, DequeStorable card);
		
		public abstract int GetIndexToStoreInto(List<Storable> inventory);
		public abstract int GetIndexToFlushBetween(List<Storable> inventory, Storable storable);
		public abstract int GetIndexToInsertBetweenAfterStore(List<Storable> inventory, Storable storable, int originalIndex);
		public abstract int GetIndexToInsertBetweenAfterFetch(List<Storable> inventory, Storable storable, int originalIndex);
		
		public abstract IEnumerable<Texture2D> GetCardTextures();
		public abstract IEnumerable<Texture2D> GetBoxTextures();
	}
}

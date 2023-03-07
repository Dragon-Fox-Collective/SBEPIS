using System.Collections.Generic;
using UnityEngine;

namespace SBEPIS.Capturllection
{
	public abstract class DequeRuleset : MonoBehaviour
	{
		public abstract string dequeName { get; }
		
		public abstract void Tick(List<Storable> inventory, float delta);
		public abstract void Layout(List<Storable> inventory);
		
		public abstract bool CanFetchFrom(List<Storable> inventory, DequeStorable card);
		
		public abstract int GetIndexToStoreInto(List<Storable> inventory);
		public abstract int GetIndexToFlushBetween(List<Storable> inventory, Storable storable);
		public abstract int GetIndexToInsertBetweenAfterStore(List<Storable> inventory, Storable storable, int originalIndex);
		public abstract int GetIndexToInsertBetweenAfterFetch(List<Storable> inventory, Storable storable, int originalIndex);
		
		public abstract IEnumerable<Texture2D> GetCardTextures();
		public abstract IEnumerable<Texture2D> GetBoxTextures();
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SBEPIS.Capturllection
{
	public abstract class DequeRuleset : MonoBehaviour
	{
		public abstract void Tick(List<Storable> inventory, float delta);
		public abstract void Layout(List<Storable> inventory);
		
		public abstract bool CanFetchFrom(List<Storable> inventory, DequeStorable card);
		
		public abstract int GetIndexToStoreInto(List<Storable> inventory);
		public abstract int GetIndexToFlushCardBetween(List<Storable> inventory, DequeStorable card);
		public abstract int GetIndexToInsertStorableBetweenAfterStore(List<Storable> inventory, Storable storable, int originalIndex);
		public abstract int GetIndexToInsertStorableBetweenAfterFetch(List<Storable> inventory, Storable storable, int originalIndex);
		
		public abstract IEnumerable<Texture2D> GetCardTextures();
		public abstract IEnumerable<Texture2D> GetBoxTextures();
	}
}

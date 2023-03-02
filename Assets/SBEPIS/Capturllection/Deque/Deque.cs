using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SBEPIS.Capturllection
{
	public abstract class Deque : MonoBehaviour
	{
		public abstract void Tick(List<Storable> cards, float delta);
		public abstract void LayoutTargets(List<Storable> cards, Dictionary<DequeStorable, CardTarget> targets);
		public abstract bool CanFetch(List<Storable> cards, DequeStorable card);
		public abstract int GetIndexToStoreInto(List<Storable> cards);
		public abstract int GetIndexToInsertCardBetween(List<Storable> cards, DequeStorable card);
		public abstract IEnumerable<Texture2D> GetCardTextures();
		public abstract IEnumerable<Texture2D> GetBoxTextures();
	}
}

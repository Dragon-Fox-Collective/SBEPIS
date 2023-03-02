using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SBEPIS.Capturllection
{
	public abstract class Deque : MonoBehaviour
	{
		public abstract void Tick(List<DequeStorable> cards, float delta);
		public abstract void LayoutTargets(List<DequeStorable> cards, Dictionary<DequeStorable, CardTarget> targets);
		public abstract bool CanFetch(List<DequeStorable> cards, DequeStorable card);
		public abstract int GetIndexToStoreInto(List<DequeStorable> cards);
		public abstract int GetIndexToInsertCardBetween(List<DequeStorable> cards, DequeStorable card);
		public abstract IEnumerable<Texture2D> GetCardTextures();
		public abstract IEnumerable<Texture2D> GetBoxTextures();
	}
}

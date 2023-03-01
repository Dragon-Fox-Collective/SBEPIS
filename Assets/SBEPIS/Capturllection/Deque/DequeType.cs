using System.Collections.Generic;
using UnityEngine;

namespace SBEPIS.Capturllection
{
	public abstract class DequeType : ScriptableObject
	{
		public Texture2D cardTexture;
		public Texture2D dequeTexture;
		
		public abstract void Tick(List<DequeStorable> cards, float delta);
		public abstract void LayoutTargets(List<DequeStorable> cards, Dictionary<DequeStorable, CardTarget> targets);
		
		public abstract bool CanFetch(List<DequeStorable> cards, DequeStorable card);
		public abstract int GetIndexToInsertInto(List<DequeStorable> cards, DequeStorable card);
	}
}

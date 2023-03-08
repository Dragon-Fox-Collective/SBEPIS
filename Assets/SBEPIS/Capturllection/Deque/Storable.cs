using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SBEPIS.Capturllection
{
	public abstract class Storable : MonoBehaviour, IEnumerable<DequeStorable>
	{
		public Vector3 position
		{
			get => transform.localPosition;
			set => transform.localPosition = value;
		}
		public Quaternion rotation
		{
			get => transform.localRotation;
			set => transform.localRotation = value;
		}
		
		public Vector3 maxPossibleSize { get; protected set; }
		
		public abstract bool hasNoCards { get; }
		public abstract bool hasAllCards { get; }
		
		public abstract bool hasAllCardsEmpty { get; }
		public abstract bool hasAllCardsFull { get; }
		
		public abstract void Tick(float deltaTime, Vector3 direction);
		public abstract void LayoutTarget(DequeStorable card, CardTarget target);
		
		public abstract bool CanFetch(DequeStorable card);
		public abstract bool Contains(DequeStorable card);
		
		public abstract (DequeStorable, Capturellectainer) Store(Capturllectable item, out Capturllectable ejectedItem);
		public abstract Capturllectable Fetch(DequeStorable card);
		public abstract void Flush(List<DequeStorable> cards);
		
		public IEnumerable<Texture2D> GetCardTextures(DequeStorable card) => GetCardTextures(card, Enumerable.Empty<IEnumerable<Texture2D>>(), 0);
		public abstract IEnumerable<Texture2D> GetCardTextures(DequeStorable card, IEnumerable<IEnumerable<Texture2D>> textures, int indexOfThisInParent);
		
		public abstract IEnumerator<DequeStorable> GetEnumerator();
		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
	}
}

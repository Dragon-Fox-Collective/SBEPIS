using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SBEPIS.Capturllection
{
	public abstract class Storable : IEnumerable<DequeStorable>
	{
		protected readonly Transform transform;
		
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
		
		public abstract bool hasNoCards { get; }
		public abstract bool hasAllCards { get; }
		
		public abstract bool hasAllCardsEmpty { get; }
		public abstract bool hasAllCardsFull { get; }
		
		public Storable(Transform transform)
		{
			this.transform = transform;
		}
		
		public abstract void Tick(float deltaTime);
		public abstract void Layout();
		public abstract void LayoutTarget(DequeStorable card, CardTarget target);
		
		public abstract bool CanFetch(DequeStorable card);
		public abstract bool Contains(DequeStorable card);
		
		public abstract (DequeStorable, Capturellectainer) Store(Capturllectable item, out Capturllectable ejectedItem);
		public abstract Capturllectable Fetch(DequeStorable card);
		public abstract DequeStorable Flush(DequeStorable card);
		
		public abstract IEnumerable<DequeStorable> Save();
		public abstract IEnumerable<DequeStorable> Load(IEnumerable<DequeStorable> newInventory);
		public abstract void Clear();

		public abstract IEnumerator<DequeStorable> GetEnumerator();
		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
	}
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace SBEPIS.Capturllection
{
	public abstract class Storable : MonoBehaviour, IEnumerable<DequeStorable>
	{
		public DequeRulesetState state;
		
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

		public abstract Vector3 maxPossibleSize { get; }
		
		public abstract int inventoryCount { get; }
		
		public abstract bool hasNoCards { get; }
		public abstract bool hasAllCards { get; }
		
		public abstract bool hasAllCardsEmpty { get; }
		public abstract bool hasAllCardsFull { get; }
		
		public abstract void Tick(float deltaTime);
		public abstract void LayoutTarget(DequeStorable card, CardTarget target);
		
		public abstract bool CanFetch(DequeStorable card);
		public abstract bool Contains(DequeStorable card);
		
		public abstract void Store(Capturllectable item, UnityAction<DequeStorable, Capturellectainer, Capturllectable> callback);
		public abstract void Fetch(DequeStorable card, UnityAction<Capturllectable> callback);
		public abstract void Flush(List<DequeStorable> cards);
		
		public IEnumerable<Texture2D> GetCardTextures(DequeStorable card) => GetCardTextures(card, Enumerable.Empty<IEnumerable<Texture2D>>(), 0);
		public abstract IEnumerable<Texture2D> GetCardTextures(DequeStorable card, IEnumerable<IEnumerable<Texture2D>> textures, int indexOfThisInParent);
		
		private void OnDrawGizmosSelected()
		{
			DrawSize(maxPossibleSize, transform, Color.magenta);
		}
		
		private static void DrawSize(Vector3 size, Transform parent, Color color, float axisLength = 0.1f)
		{
			Vector3 extents = size / 2;
			
			Matrix4x4 m = new();
			m.SetTRS(parent.position, parent.rotation, Vector3.one);
			
			Vector3 point1 = m.MultiplyPoint(new Vector3(-extents.x, -extents.y, +extents.z));
			Vector3 point2 = m.MultiplyPoint(new Vector3(+extents.x, -extents.y, +extents.z));
			Vector3 point3 = m.MultiplyPoint(new Vector3(+extents.x, -extents.y, -extents.z));
			Vector3 point4 = m.MultiplyPoint(new Vector3(-extents.x, -extents.y, -extents.z));
			
			Vector3 point5 = m.MultiplyPoint(new Vector3(-extents.x, +extents.y, +extents.z));
			Vector3 point6 = m.MultiplyPoint(new Vector3(+extents.x, +extents.y, +extents.z));
			Vector3 point7 = m.MultiplyPoint(new Vector3(+extents.x, +extents.y, -extents.z));
			Vector3 point8 = m.MultiplyPoint(new Vector3(-extents.x, +extents.y, -extents.z));
			
			Debug.DrawLine(point1, point2, color);
			Debug.DrawLine(point2, point3, color);
			Debug.DrawLine(point3, point4, color);
			Debug.DrawLine(point4, point1, color);
			
			Debug.DrawLine(point5, point6, color);
			Debug.DrawLine(point6, point7, color);
			Debug.DrawLine(point7, point8, color);
			Debug.DrawLine(point8, point5, color);
			
			Debug.DrawLine(point1, point5, color);
			Debug.DrawLine(point2, point6, color);
			Debug.DrawLine(point3, point7, color);
			Debug.DrawLine(point4, point8, color);

			if (axisLength > 0)
			{
				Debug.DrawRay(m.GetPosition(), m.MultiplyVector(Vector3.right * axisLength), Color.red);
				Debug.DrawRay(m.GetPosition(), m.MultiplyVector(Vector3.up * axisLength), Color.green);
				Debug.DrawRay(m.GetPosition(), m.MultiplyVector(Vector3.forward * axisLength), Color.blue);
			}
		}
		
		public abstract IEnumerator<DequeStorable> GetEnumerator();
		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
	}
}

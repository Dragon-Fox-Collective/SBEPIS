using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace SBEPIS.Capturellection.Storage
{
	public abstract class Storable : MonoBehaviour, IEnumerable<InventoryStorable>
	{
		public DequeRulesetState state;
		
		public Vector3 Position
		{
			get => transform.localPosition;
			set => transform.localPosition = value;
		}
		public Quaternion Rotation
		{
			get => transform.localRotation;
			set => transform.localRotation = value;
		}
		
		public abstract Vector3 MaxPossibleSize { get; }
		
		public abstract int InventoryCount { get; }
		
		public abstract bool HasNoCards { get; }
		public abstract bool HasAllCards { get; }
		
		public abstract bool HasAllCardsEmpty { get; }
		public abstract bool HasAllCardsFull { get; }
		
		public abstract void Tick(float deltaTime);
		public abstract void LayoutTarget(InventoryStorable card, CardTarget target);
		
		public abstract bool CanFetch(InventoryStorable card);
		public abstract bool Contains(InventoryStorable card);
		
		public abstract UniTask<StorableStoreResult> StoreItem(Capturellectable item);
		public abstract UniTask<Capturellectable> FetchItem(InventoryStorable card);
		public abstract UniTask FlushCards(List<InventoryStorable> cards);
		public abstract UniTask<InventoryStorable> FetchCard(InventoryStorable card);
		
		public abstract void Load(List<InventoryStorable> cards);
		public abstract void Save(List<InventoryStorable> cards);
		
		public IEnumerable<Texture2D> GetCardTextures(InventoryStorable card) => GetCardTextures(card, Enumerable.Empty<IEnumerable<Texture2D>>(), 0);
		public abstract IEnumerable<Texture2D> GetCardTextures(InventoryStorable card, IEnumerable<IEnumerable<Texture2D>> textures, int indexOfThisInParent);
		
		private void OnDrawGizmosSelected()
		{
			DrawSize(MaxPossibleSize, transform, Color.magenta);
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
		
		public abstract IEnumerator<InventoryStorable> GetEnumerator();
		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
	}

	public struct StorableStoreResult
	{
		public InventoryStorable card;
		public CaptureContainer container;
		public Capturellectable ejectedItem;

		public StorableStoreResult(InventoryStorable card, CaptureContainer container, Capturellectable ejectedItem)
		{
			this.card = card;
			this.container = container;
			this.ejectedItem = ejectedItem;
		}

		public DequeStoreResult ToDequeResult(int flushIndex, Storable storable) => new(card, container, ejectedItem, flushIndex, storable);
	}
}

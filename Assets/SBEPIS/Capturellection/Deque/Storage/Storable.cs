using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace SBEPIS.Capturellection.Storage
{
	public interface Storable : IEnumerable<InventoryStorable>
	{
		public Vector3 Position { get; set; }
		public Quaternion Rotation { get; set; }
		public Transform Parent { get; set; }
		
		public Vector3 MaxPossibleSize { get; }
		
		public int InventoryCount { get; }
		
		public int NumEmptySlots { get; }
		
		public bool HasNoCards { get; }
		public bool HasAllCards { get; }
		
		public bool HasAllCardsEmpty { get; }
		public bool HasAllCardsFull { get; }
		
		public void InitPage(DiajectorPage page);
		
		public void Tick(float deltaTime);
		public void Layout(Vector3 direction);
		public void LayoutTarget(InventoryStorable card, CardTarget target);
		
		public bool CanFetch(InventoryStorable card);
		public bool Contains(InventoryStorable card);
		
		public UniTask<StoreResult> StoreItem(Capturellectable item);
		public UniTask<FetchResult> FetchItem(InventoryStorable card);
		public UniTask Interact<TState>(InventoryStorable card, DequeRuleset targetDeque, DequeInteraction<TState> action);
		
		public void Load(List<InventoryStorable> cards);
		public IEnumerable<InventoryStorable> Save();
		
		public Storable GetNewStorableLikeThis();
		
		public IEnumerable<Texture2D> GetCardTextures(InventoryStorable card) => GetCardTextures(card, Enumerable.Empty<IEnumerable<Texture2D>>(), 0);
		public IEnumerable<Texture2D> GetCardTextures(InventoryStorable card, IEnumerable<IEnumerable<Texture2D>> textures, int indexOfThisInParent);
		
		protected static void DrawSize(Vector3 size, Transform parent, Color color, float axisLength = 0.1f)
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
		
		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
	}

	public struct StoreResult
	{
		public InventoryStorable card;
		public CaptureContainer container;
		public Capturellectable ejectedItem;
		
		public StoreResult(InventoryStorable card, CaptureContainer container, Capturellectable ejectedItem)
		{
			this.card = card;
			this.container = container;
			this.ejectedItem = ejectedItem;
		}
	}
	
	public struct FetchResult
	{
		public Capturellectable fetchedItem;
		
		public FetchResult(Capturellectable fetchedItem)
		{
			this.fetchedItem = fetchedItem;
		}
	}
}

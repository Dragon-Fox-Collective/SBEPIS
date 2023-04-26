using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using SBEPIS.Capturellection.Storage;
using UnityEngine;

namespace SBEPIS.Capturellection.Deques
{
	public class CycloneDeque : DequeBase<CycloneState>
	{
		public float timePerCard = 1;
		
		public override void Tick(List<Storable> inventory, CycloneState state, float deltaTime)
		{
			state.time += deltaTime;
			
			foreach (Storable storable in inventory)
			{
				storable.state.Direction = Quaternion.Euler(0, 0, -90) * state.Direction;
				storable.Tick(deltaTime * storable.InventoryCount);
			}
			
			List<Vector3> sizes = inventory.Select(storable => storable.MaxPossibleSize).ToList();
			float longestEdge = sizes.Select(size => size.x).Aggregate(Mathf.Max);
			float innerRadius = DistanceToRegularPolygonEdge(inventory.Count, longestEdge);
			
			float anglePerCard = 360f / inventory.Count;
			float cardAngle = state.time / timePerCard * anglePerCard;
			foreach ((Storable storable, Vector3 size) in inventory.Zip(sizes))
			{
				float modAngle = cardAngle.ModAround(360);
				if (Mathf.Abs(modAngle) < anglePerCard / 2)
					state.topStorable = storable;
				
				storable.Position = Quaternion.Euler(0, 0, cardAngle) * Vector3.up * (innerRadius + size.y / 2);
				storable.Rotation = Quaternion.Euler(0, 0, 180f + cardAngle) * ArrayDeque.GetOffsetRotation(state.Direction);
				
				cardAngle += anglePerCard;
			}
		}
		public override Vector3 GetMaxPossibleSizeOf(List<Storable> inventory, CycloneState state)
		{
			List<Vector3> sizes = inventory.Select(storable => storable.MaxPossibleSize).ToList();
			float longestEdge = sizes.Select(size => size.x).Aggregate(Mathf.Max);
			float innerRadius = DistanceToRegularPolygonEdge(inventory.Count, longestEdge);
			Vector3 maxSize = sizes.Aggregate(ExtensionMethods.Max);
			Vector3 sumSize = new(
				(innerRadius + maxSize.y) * 2,
				(innerRadius + maxSize.y) * 2,
				maxSize.z);
			return sumSize;
		}
		
		public static float DistanceToRegularPolygonEdge(int edgeCount, float edgeLength) => edgeCount < 3 ? 0 : 0.5f * edgeLength / Mathf.Tan(Mathf.PI / edgeCount);
		
		public override bool CanFetchFrom(List<Storable> inventory, CycloneState state, InventoryStorable card) => state.topStorable.CanFetch(card);
		
		public override async UniTask<DequeStoreResult> StoreItem(List<Storable> inventory, CycloneState state, Capturellectable item)
		{
			int index = inventory.IndexOf(state.topStorable);
			if (index < 0) index = Mathf.Max(inventory.FindIndex(storable => !storable.HasAllCardsFull), 0);
			Storable storable = inventory[index];
			StorableStoreResult res = await storable.StoreItem(item);
			return res.ToDequeResult(index);
		}
		
		public override UniTask FlushCard(List<Storable> inventory, CycloneState state, Storable storable)
		{
			int index = inventory.IndexOf(state.topStorable);
			if (index < 0) index = inventory.Count;
			inventory.Insert(index, storable);
			return UniTask.CompletedTask;
		}
	}
}

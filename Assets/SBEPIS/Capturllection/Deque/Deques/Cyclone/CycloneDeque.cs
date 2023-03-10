using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SBEPIS.Capturllection.Deques
{
	public class CycloneDeque : DequeBase<CycloneState>
	{
		public float timePerCard = 1;
		
		public override void Tick(List<Storable> inventory, CycloneState state, float deltaTime)
		{
			state.time += deltaTime;
			
			foreach (Storable storable in inventory)
			{
				storable.state.direction = Quaternion.Euler(0, 0, -90) * state.direction;
				storable.Tick(deltaTime * storable.inventoryCount);
			}
			
			List<Vector3> sizes = inventory.Select(storable => storable.maxPossibleSize).ToList();
			float longestEdge = sizes.Select(size => size.x).Aggregate(Mathf.Max);
			float innerRadius = DistanceToRegularPolygonEdge(inventory.Count, longestEdge);
			
			float anglePerCard = 360f / inventory.Count;
			float cardAngle = state.time / timePerCard * anglePerCard;
			foreach ((Storable storable, Vector3 size) in inventory.Zip(sizes))
			{
				float modAngle = cardAngle.ModAround(360);
				if (Mathf.Abs(modAngle) < anglePerCard / 2)
					state.topStorable = storable;
				
				storable.position = Quaternion.Euler(0, 0, cardAngle) * Vector3.up * (innerRadius + size.y / 2);
				storable.rotation = Quaternion.Euler(0, 0, 180f + cardAngle) * ArrayDeque.GetOffsetRotation(state.direction);
				
				cardAngle += anglePerCard;
			}
		}
		public override Vector3 GetMaxPossibleSizeOf(List<Storable> inventory, CycloneState state)
		{
			List<Vector3> sizes = inventory.Select(storable => storable.maxPossibleSize).ToList();
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
		
		public override bool CanFetchFrom(List<Storable> inventory, CycloneState state, DequeStorable card) => state.topStorable.CanFetch(card);

		public override int GetIndexToStoreInto(List<Storable> inventory, CycloneState state)
		{
			if (inventory.Contains(state.topStorable))
				return inventory.IndexOf(state.topStorable);
			
			int index = inventory.FindIndex(storable => !storable.hasAllCardsFull);
			return index is -1 ? 0 : index;
		}
		public override int GetIndexToFlushBetween(List<Storable> inventory, CycloneState state, Storable storable) => inventory.Contains(state.topStorable) ? inventory.IndexOf(state.topStorable) : inventory.Count;
		public override int GetIndexToInsertBetweenAfterStore(List<Storable> inventory, CycloneState state, Storable storable, int originalIndex) => originalIndex;
		public override int GetIndexToInsertBetweenAfterFetch(List<Storable> inventory, CycloneState state, Storable storable, int originalIndex) => originalIndex;
	}
}

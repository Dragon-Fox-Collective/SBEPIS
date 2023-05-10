using System;
using System.Collections.Generic;
using System.Linq;
using SBEPIS.Capturellection.Storage;
using UnityEngine;

namespace SBEPIS.Capturellection.Deques
{
	[Serializable]
	public class CycloneLayout : DequeLayoutBase
	{
		public float timePerCard = 1;
		
		public void Tick<TState>(List<Storable> inventory, TState state, float deltaTime) where TState : DirectionState, TimeState, TopState
		{
			state.Time += deltaTime;
			
			foreach (Storable storable in inventory)
			{
				storable.Direction = Quaternion.Euler(0, 0, -90) * state.Direction;
				storable.Tick(deltaTime * storable.InventoryCount);
			}
			
			List<Vector3> sizes = inventory.Select(storable => storable.MaxPossibleSize).ToList();
			float longestEdge = sizes.Select(size => size.x).Aggregate(Mathf.Max);
			float innerRadius = DistanceToRegularPolygonEdge(inventory.Count, longestEdge);
			
			float anglePerCard = 360f / inventory.Count;
			float cardAngle = state.Time / timePerCard * anglePerCard;
			foreach ((Storable storable, Vector3 size) in inventory.Zip(sizes))
			{
				float modAngle = cardAngle.ModAround(360);
				if (Mathf.Abs(modAngle) < anglePerCard / 2)
					state.TopStorable = storable;
				
				storable.Position = Quaternion.Euler(0, 0, cardAngle) * Vector3.up * (innerRadius + size.y / 2);
				storable.Rotation = Quaternion.Euler(0, 0, 180f + cardAngle) * DequeLayout.GetOffsetRotation(state.Direction);
				
				cardAngle += anglePerCard;
			}
		}
		
		public Vector3 GetMaxPossibleSizeOf<TState>(List<Storable> inventory, TState state)
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
		
		private static float DistanceToRegularPolygonEdge(int edgeCount, float edgeLength) => edgeCount < 3 ? 0 : 0.5f * edgeLength / Mathf.Tan(Mathf.PI / edgeCount);
	}
	
	[Serializable]
	public class CycloneState : InventoryState, DirectionState, TimeState, TopState
	{
		public List<Storable> Inventory { get; set; } = new();
		public Vector3 Direction { get; set; }
		public float Time { get; set; }
		public Storable TopStorable { get; set; }
	}
}

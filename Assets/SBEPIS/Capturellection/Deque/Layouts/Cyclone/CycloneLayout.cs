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
		public float timePerCard = 3;
		
		public void Tick<TState>(IList<Storable> inventory, TState state, float deltaTime) where TState : DirectionState, TimeState, TopState
		{
			state.Time += deltaTime;
			
			if (inventory.Count > 0)
				state.TopStorable = inventory[^(Mathf.FloorToInt(state.Time / timePerCard - 0.5f).Mod(inventory.Count) + 1)];
			
			foreach (Storable storable in inventory)
				storable.Tick(deltaTime * storable.InventoryCount);
		}
		
		public void Layout<TState>(IList<Storable> inventory, TState state) where TState : DirectionState, TimeState, TopState
		{
			foreach (Storable storable in inventory)
				storable.Layout(Quaternion.Euler(0, 0, -90) * state.Direction);
			
			List<Vector3> sizes = inventory.Select(storable => storable.MaxPossibleSize).ToList();
			float longestEdge = sizes.Select(size => size.x).Aggregate(Mathf.Max);
			float innerRadius = DistanceToRegularPolygonEdge(inventory.Count, longestEdge);
			
			float anglePerCard = 360f / inventory.Count;
			float cardAngle = state.Time / timePerCard * anglePerCard;
			foreach ((Storable storable, Vector3 size) in inventory.Zip(sizes))
			{
				storable.Position = Quaternion.Euler(0, 0, cardAngle) * Vector3.up * (innerRadius + size.y / 2);
				storable.Rotation = Quaternion.Euler(0, 0, 180f + cardAngle) * DequeLayout.GetOffsetRotation(state.Direction);
				
				cardAngle += anglePerCard;
			}
		}
		
		public Vector3 GetMaxPossibleSizeOf<TState>(IList<Storable> inventory, TState state)
		{
			List<Vector3> sizes = inventory.Select(storable => storable.MaxPossibleSize).ToList();
			float longestEdge = sizes.Select(size => size.x).Aggregate(Mathf.Max);
			float innerRadius = DistanceToRegularPolygonEdge(inventory.Count, longestEdge);
			Vector3 maxSize = sizes.Aggregate(VectorExtensions.Max);
			Vector3 sumSize = new(
				(innerRadius + maxSize.y) * 2,
				(innerRadius + maxSize.y) * 2,
				maxSize.z);
			return sumSize;
		}
		
		private static float DistanceToRegularPolygonEdge(int edgeCount, float edgeLength) => edgeCount < 3 ? 0 : 0.5f * edgeLength / Mathf.Tan(Mathf.PI / edgeCount);
	}
	
	public class CycloneState : InventoryState, DirectionState, TimeState, TopState
	{
		public CallbackList<Storable> Inventory { get; set; } = new();
		public Vector3 Direction { get; set; }
		public float Time { get; set; }
		public Storable TopStorable { get; set; }
	}
	
	[Serializable]
	public class CycloneSettings : LayoutSettings<CycloneLayout>
	{
		[SerializeField] private CycloneLayout layout;
		public CycloneLayout Layout => layout;
	}
}

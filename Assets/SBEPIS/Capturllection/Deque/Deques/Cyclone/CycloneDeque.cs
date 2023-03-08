using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SBEPIS.Capturllection.Deques
{
	public class CycloneDeque : DequeBase<CycloneState>
	{
		public float innerRadius = 0.1f;
		public float speed = 20;
		
		public override void Tick(List<Storable> inventory, CycloneState state, float deltaTime, Vector3 direction)
		{
			state.time += deltaTime;
			
			if (inventory.Count == 0)
				state.topStorable = null;
			
			float cardAngle = state.time * speed;
			float deltaAngle = 360f / inventory.Count;
			foreach (Storable storable in inventory)
			{
				storable.Tick(deltaTime / inventory.Count, Vector3.down);
				Vector3 size = storable.maxPossibleSize;
				
				float modAngle = cardAngle.ModAround(360);
				if (Mathf.Abs(modAngle) < deltaAngle / 2)
					state.topStorable = storable;

				storable.position = Quaternion.Euler(0, 0, cardAngle) * Vector3.up * (innerRadius + size.y / 2);
				storable.rotation = Quaternion.Euler(0, 0, 180f + cardAngle) * Quaternion.identity;
				
				cardAngle += deltaAngle;
			}
		}
		public override Vector3 GetMaxPossibleSizeOf(List<Storable> inventory)
		{
			List<Vector3> sizes = inventory.Select(storable => storable.maxPossibleSize).ToList();
			Vector3 maxSize = sizes.Aggregate(ExtensionMethods.Max);
			Vector3 sumSize = new(
				(innerRadius + maxSize.y) * 2,
				(innerRadius + maxSize.y) * 2,
				0);
			return ExtensionMethods.Max(maxSize, sumSize);
		}

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

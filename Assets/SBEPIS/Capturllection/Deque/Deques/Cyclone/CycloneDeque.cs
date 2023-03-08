using System.Collections.Generic;
using UnityEngine;

namespace SBEPIS.Capturllection.Deques
{
	public class CycloneDeque : DequeBase
	{
		public float innerRadius = 0.1f;
		public float speed = 20;
		
		private float time;
		private Storable topStorable;
		
		public override Vector3 TickAndGetMaxSize(List<Storable> inventory, float deltaTime, Vector3 direction)
		{
			time += deltaTime;
			
			if (inventory.Count == 0)
			{
				topStorable = null;
				return Vector3.zero;
			}

			float maxYSize = 0;
			float maxZSize = 0;
			
			float cardAngle = time * speed;
			float deltaAngle = 360f / inventory.Count;
			foreach (Storable storable in inventory)
			{
				float modAngle = cardAngle.ModAround(360);
				if (Mathf.Abs(modAngle) < deltaAngle / 2)
					topStorable = storable;
				
				Vector3 size = storable.TickAndGetMaxSize(deltaTime * inventory.Count, Vector3.down);
				maxYSize = Mathf.Max(maxYSize, size.y);
				maxZSize = Mathf.Max(maxZSize, size.z);
				
				storable.position = Quaternion.Euler(0, 0, cardAngle) * Vector3.up * (innerRadius + size.y / 2);
				storable.rotation = Quaternion.Euler(0, 0, cardAngle) * Quaternion.identity;
				
				cardAngle += deltaAngle;
			}
			
			return new Vector3(
				(innerRadius + maxYSize) * 2,
				(innerRadius + maxYSize) * 2,
				maxZSize * 2);
		}
		
		public override bool CanFetchFrom(List<Storable> inventory, DequeStorable card) => topStorable.CanFetch(card);
		
		public override int GetIndexToStoreInto(List<Storable> inventory)
		{
			if (inventory.Contains(topStorable))
				return inventory.IndexOf(topStorable);
			
			int index = inventory.FindIndex(storable => !storable.hasAllCardsFull);
			return index is -1 ? 0 : index;
		}
		public override int GetIndexToFlushBetween(List<Storable> inventory, Storable storable) => inventory.Contains(topStorable) ? inventory.IndexOf(topStorable) : inventory.Count;
		public override int GetIndexToInsertBetweenAfterStore(List<Storable> inventory, Storable storable, int originalIndex) => originalIndex;
		public override int GetIndexToInsertBetweenAfterFetch(List<Storable> inventory, Storable storable, int originalIndex) => originalIndex;
	}
}

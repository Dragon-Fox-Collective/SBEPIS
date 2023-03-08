using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

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

			float maxYExtent = 0;
			float maxZExtent = 0;
			
			float cardAngle = time * speed;
			float deltaAngle = 360f / inventory.Count;
			foreach (Storable storable in inventory)
			{
				float modAngle = cardAngle.ModAround(360);
				if (Mathf.Abs(modAngle) < deltaAngle / 2)
					topStorable = storable;
				
				Vector3 size = storable.TickAndGetMaxSize(deltaTime * inventory.Count, Vector3.down);
				maxYExtent = Mathf.Max(maxYExtent, size.y);
				
				storable.position = Quaternion.Euler(0, 0, cardAngle) * Vector3.up * innerRadius;
				storable.rotation = Quaternion.Euler(0, 0, cardAngle) * Quaternion.identity;

				maxZExtent = Mathf.Max(maxZExtent, Mathf.Max(Mathf.Abs(storable..z + size.z), Mathf.Abs(storable.position.z - size.z)));
				
				cardAngle += deltaAngle;
			}
			
			return new Vector3(
				(innerRadius + maxYExtent) * 2,
				(innerRadius + maxYExtent) * 2,
				maxZExtent * 2);
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

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SBEPIS.Capturllection.Deques
{
	public class StackDeque : DequeBase
	{
		public float overlap = 0.05f;
		
		public override Vector3 Tick(List<Storable> inventory, float deltaTime, Vector3 direction)
		{
			List<Vector3> sizes = inventory.Select(storable => storable.Tick(deltaTime, Quaternion.Euler(0, 0, -60) * direction)).ToList();
			
			Vector3 absDirection = direction.Select(Mathf.Abs);
			float xSum = -overlap * (inventory.Count - 1) + sizes.Select(size => Vector3.Project(size, absDirection)).Aggregate(ExtensionMethods.Add).magnitude;

			Vector3 right = -xSum / 2 * direction;
			foreach ((Storable storable, Vector3 size) in inventory.Zip(sizes))
			{
				right += direction * size.x / 2;
				
				storable.position = right;
				storable.rotation = Quaternion.identity;
				
				right += direction * (size.x / 2 - overlap);
			}
			
			Vector3 maxSizes = sizes.Aggregate(ExtensionMethods.Max);
			Vector3 maxSize = maxSizes * inventory.Count - overlap * (inventory.Count - 1) * Vector3.one;
			
			return maxSize;
		}
		public override Vector3 GetMaxPossibleSizeOf(List<Storable> inventory)
		{
			
		}
		
		public override bool CanFetchFrom(List<Storable> inventory, DequeStorable card) => inventory[0].CanFetch(card);
		
		public override int GetIndexToStoreInto(List<Storable> inventory) => inventory.Count - 1;
		public override int GetIndexToFlushBetween(List<Storable> inventory, Storable storable) => storable.hasAllCardsEmpty ? inventory.Count : 0;
		public override int GetIndexToInsertBetweenAfterStore(List<Storable> inventory, Storable storable, int originalIndex) => 0;
		public override int GetIndexToInsertBetweenAfterFetch(List<Storable> inventory, Storable storable, int originalIndex) => inventory.Count;
	}
}

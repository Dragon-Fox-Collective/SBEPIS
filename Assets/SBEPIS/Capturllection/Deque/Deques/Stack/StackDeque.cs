using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SBEPIS.Capturllection.Deques
{
	public class StackDeque : DequeBase
	{
		public float overlap = 0.05f;
		
		public override Vector3 TickAndGetMaxSize(List<Storable> inventory, float deltaTime, Vector3 direction)
		{
			List<Vector3> sizes = inventory.Select(storable => storable.TickAndGetMaxSize(deltaTime, Quaternion.Euler(0, 0, -60) * direction)).ToList();
			float xSum = -overlap * (inventory.Count - 1) + sizes.Select(size => size.x).Aggregate(ExtensionMethods.Add);
			float maxYSize = sizes.Select(size => size.y).Aggregate(Mathf.Max);
			float maxZSize = sizes.Select(size => size.z).Aggregate(Mathf.Max);
			
			Vector3 right = -xSum / 2 * direction;
			foreach ((Storable storable, Vector3 size) in inventory.Zip(sizes))
			{
				right += direction * size.x / 2;
				
				storable.position = right;
				storable.rotation = Quaternion.identity;
				
				right += direction * (size.x / 2 - overlap);
			}
			
			return new Vector3(xSum, maxYSize, maxZSize);
		}
		
		public override bool CanFetchFrom(List<Storable> inventory, DequeStorable card) => inventory[0].CanFetch(card);
		
		public override int GetIndexToStoreInto(List<Storable> inventory) => inventory.Count - 1;
		public override int GetIndexToFlushBetween(List<Storable> inventory, Storable storable) => storable.hasAllCardsEmpty ? inventory.Count : 0;
		public override int GetIndexToInsertBetweenAfterStore(List<Storable> inventory, Storable storable, int originalIndex) => 0;
		public override int GetIndexToInsertBetweenAfterFetch(List<Storable> inventory, Storable storable, int originalIndex) => inventory.Count;
	}
}

using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SBEPIS.Capturllection.Deques
{
	public class ArrayDeque : DequeBase
	{
		public float overlap = 0.05f;
		public float wobbleAmplitude = 0.1f;
		public float wobbleTimeFactor = 1;
		public float wobbleSpaceFactor = 1;
		
		private float time;
		
		public override Vector3 TickAndGetMaxSize(List<Storable> inventory, float deltaTime, Vector3 direction)
		{
			time += deltaTime;
			
			List<Vector3> sizes = inventory.Select(storable => storable.TickAndGetMaxSize(deltaTime, Quaternion.Euler(0, 0, -60) * direction)).ToList();
			float xSum = -overlap * (inventory.Count - 1) + sizes.Select(size => size.x).Aggregate(ExtensionMethods.Add);
			float maxYSize = sizes.Select(size => size.y).Aggregate(Mathf.Max);
			float maxZSize = sizes.Select(size => size.z).Aggregate(Mathf.Max);
			
			Vector3 right = -xSum / 2 * direction;
			foreach ((Storable storable, Vector3 size) in inventory.Zip(sizes))
			{
				right += direction * size.x / 2;
				Vector3 up = Mathf.Sin(time * wobbleTimeFactor + right.x * wobbleSpaceFactor) * wobbleAmplitude * Vector3.up;
				
				storable.position = right + up;
				storable.rotation = Quaternion.identity;
				
				right += direction * (size.x / 2 - overlap);
			}
			
			return new Vector3(
				xSum,
				(wobbleAmplitude + maxYSize / 2) * 2,
				maxZSize);
		}
		
		public override bool CanFetchFrom(List<Storable> inventory, DequeStorable card) => inventory.Any(storable => storable.CanFetch(card));
		
		public override int GetIndexToStoreInto(List<Storable> inventory)
		{
			int index = inventory.FindIndex(storable => !storable.hasAllCardsFull);
			return index is -1 ? 0 : index;
		}
		public override int GetIndexToFlushBetween(List<Storable> inventory, Storable storable) => inventory.Count;
		public override int GetIndexToInsertBetweenAfterStore(List<Storable> inventory, Storable storable, int originalIndex) => originalIndex;
		public override int GetIndexToInsertBetweenAfterFetch(List<Storable> inventory, Storable storable, int originalIndex) => originalIndex;
	}
}

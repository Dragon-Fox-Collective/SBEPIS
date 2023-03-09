using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SBEPIS.Capturllection.Deques
{
	public class StackDeque : DequeBase<NoState>
	{
		public float overlap = 0.05f;
		
		public override void Tick(List<Storable> inventory, NoState state, float deltaTime, Vector3 direction)
		{
			List<Vector3> sizes = inventory.Select(storable => storable.maxPossibleSize).ToList();
			Vector3 absDirection = direction.Select(Mathf.Abs);
			print($"{transform.GetSiblingIndex()} {absDirection} {sizes[0]} {Vector3.Project(sizes[0], absDirection)}");
			float lengthSum = -overlap * (inventory.Count - 1) + sizes.Select(size => Vector3.Project(size, absDirection)).Aggregate(ExtensionMethods.Add).magnitude;
			
			Vector3 right = -lengthSum / 2 * direction;
			foreach ((Storable storable, Vector3 size) in inventory.Zip(sizes))
			{
				storable.Tick(deltaTime, Quaternion.Euler(0, 0, -60) * direction);
				
				float length = Vector3.Project(size, absDirection).magnitude;
				right += direction * length / 2;
				
				storable.position = right;
				storable.rotation = Quaternion.identity;
				
				right += direction * (length / 2 - overlap);
			}
		}
		public override Vector3 GetMaxPossibleSizeOf(List<Storable> inventory)
		{
			List<Vector3> sizes = inventory.Select(storable => storable.maxPossibleSize).ToList();
			Vector3 maxSize = sizes.Aggregate(ExtensionMethods.Max);
			Vector3 sumSize = sizes.Aggregate(ExtensionMethods.Add) - overlap * (inventory.Count - 1) * Vector3.one;
			return ExtensionMethods.Max(maxSize, sumSize);
		}
		
		public override bool CanFetchFrom(List<Storable> inventory, NoState state, DequeStorable card) => inventory[0].CanFetch(card);
		
		public override int GetIndexToStoreInto(List<Storable> inventory, NoState state) => inventory.Count - 1;
		public override int GetIndexToFlushBetween(List<Storable> inventory, NoState state, Storable storable) => storable.hasAllCardsEmpty ? inventory.Count : 0;
		public override int GetIndexToInsertBetweenAfterStore(List<Storable> inventory, NoState state, Storable storable, int originalIndex) => 0;
		public override int GetIndexToInsertBetweenAfterFetch(List<Storable> inventory, NoState state, Storable storable, int originalIndex) => inventory.Count;
	}
}

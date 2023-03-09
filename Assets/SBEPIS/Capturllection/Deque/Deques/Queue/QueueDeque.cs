using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

namespace SBEPIS.Capturllection.Deques
{
	public class QueueDeque : DequeBase<NoState>
	{
		public bool offsetFromEnd = false;
		[FormerlySerializedAs("overlap")]
		public float offset = 0.05f;
		
		public override void Tick(List<Storable> inventory, NoState state, float deltaTime, Vector3 direction)
		{
			List<Vector3> sizes = inventory.Select(storable => storable.maxPossibleSize).ToList();
			Vector3 absDirection = direction.Select(Mathf.Abs);
			float lengthSum = (offsetFromEnd ? -1 : 1) * offset * (inventory.Count - 1) + sizes.Select(size => Vector3.Project(size, absDirection)).Aggregate(ExtensionMethods.Add).magnitude;
			
			Vector3 right = -lengthSum / 2 * direction;
			foreach ((Storable storable, Vector3 size) in inventory.Zip(sizes))
			{
				storable.Tick(deltaTime, Quaternion.Euler(0, 0, -60) * direction);
				
				float length = Vector3.Project(size, absDirection).magnitude;
				right += direction * length / 2;
				
				storable.position = right;
				storable.rotation = Quaternion.identity;
				
				right += direction * (offsetFromEnd ? length / 2 - offset : offset);
			}
		}
		public override Vector3 GetMaxPossibleSizeOf(List<Storable> inventory)
		{
			List<Vector3> sizes = inventory.Select(storable => storable.maxPossibleSize).ToList();
			Vector3 maxSize = sizes.Aggregate(ExtensionMethods.Max);
			Vector3 sumSize = sizes.Aggregate(ExtensionMethods.Add) + (offsetFromEnd ? -1 : 1) * offset * (inventory.Count - 1) * Vector3.one;
			return ExtensionMethods.Max(maxSize, sumSize);
		}
		
		public override bool CanFetchFrom(List<Storable> inventory, NoState state, DequeStorable card) => inventory[^1].CanFetch(card);
		
		public override int GetIndexToStoreInto(List<Storable> inventory, NoState state)
		{
			int index = inventory.FindIndex(storable => !storable.hasAllCardsEmpty);
			return index is -1 or 0 ? inventory.Count - 1 : index - 1;
		}
		public override int GetIndexToFlushBetween(List<Storable> inventory, NoState state, Storable storable)
		{
			int index = inventory.FindIndex(storable => !storable.hasAllCardsEmpty);
			return index is -1 ? inventory.Count : index;
		}
		public override int GetIndexToInsertBetweenAfterStore(List<Storable> inventory, NoState state, Storable storable, int originalIndex) => GetIndexToFlushBetween(inventory, state, storable);
		public override int GetIndexToInsertBetweenAfterFetch(List<Storable> inventory, NoState state, Storable storable, int originalIndex) => GetIndexToFlushBetween(inventory, state, storable);
	}
}

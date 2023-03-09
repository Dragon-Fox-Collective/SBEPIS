using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

namespace SBEPIS.Capturllection.Deques
{
	public class ArrayDeque : DequeBase<ArrayState>
	{
		public bool offsetFromEnd = false;
		[FormerlySerializedAs("overlap")]
		public float offset = 0.05f;
		public float wobbleAmplitude = 0.1f;
		public float wobbleTimeFactor = 1;
		public float wobbleSpaceFactor = 1;
		
		public override void Tick(List<Storable> inventory, ArrayState state, float deltaTime, Vector3 direction)
		{
			state.time += deltaTime;
			
			List<Vector3> sizes = inventory.Select(storable => storable.maxPossibleSize).ToList();
			Vector3 absDirection = direction.Select(Mathf.Abs);
			float lengthSum = offsetFromEnd ?
				-offset * (inventory.Count - 1) + sizes.Select(size => Vector3.Project(size, absDirection)).Aggregate(ExtensionMethods.Add).magnitude :
				offset * (inventory.Count - 1);
			
			Vector3 startRight = -lengthSum / 2 * direction;
			Vector3 right = startRight;
			foreach ((Storable storable, Vector3 size) in inventory.Zip(sizes))
			{
				storable.Tick(deltaTime, Quaternion.Euler(0, 0, -60) * direction);
				
				float length = Vector3.Project(size, absDirection).magnitude;
				right += direction * (offsetFromEnd ? length / 2 : 0);
				
				Vector3 up = Mathf.Sin(state.time * wobbleTimeFactor + (right - startRight).magnitude * wobbleSpaceFactor) * wobbleAmplitude * Vector3.up;
				
				storable.position = right + up;
				storable.rotation = Quaternion.identity;
				
				right += direction * (offset + (offsetFromEnd ? length / 2 : 0));
			}
		}
		public override Vector3 GetMaxPossibleSizeOf(List<Storable> inventory)
		{
			List<Vector3> sizes = inventory.Select(storable => storable.maxPossibleSize).ToList();
			Vector3 maxSize = sizes.Aggregate(ExtensionMethods.Max);
			Vector3 sumSize = offsetFromEnd ?
				-offset * (inventory.Count - 1) * Vector3.one + sizes.Aggregate(ExtensionMethods.Add) :
				offset * (inventory.Count - 1) * Vector3.one;
			return ExtensionMethods.Max(maxSize, sumSize);
		}
		
		public override bool CanFetchFrom(List<Storable> inventory, ArrayState state, DequeStorable card) => inventory.Any(storable => storable.CanFetch(card));
		
		public override int GetIndexToStoreInto(List<Storable> inventory, ArrayState state)
		{
			int index = inventory.FindIndex(storable => !storable.hasAllCardsFull);
			return index is -1 ? 0 : index;
		}
		public override int GetIndexToFlushBetween(List<Storable> inventory, ArrayState state, Storable storable) => inventory.Count;
		public override int GetIndexToInsertBetweenAfterStore(List<Storable> inventory, ArrayState state, Storable storable, int originalIndex) => originalIndex;
		public override int GetIndexToInsertBetweenAfterFetch(List<Storable> inventory, ArrayState state, Storable storable, int originalIndex) => originalIndex;
	}
}

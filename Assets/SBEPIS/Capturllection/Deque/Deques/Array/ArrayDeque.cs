using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

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

			Vector3 maxExtents = Vector3.zero;
			
			Vector3 right = -cardDistance * (inventory.Count - 1) / 2 * direction;
			foreach (Storable storable in inventory)
			{
				Vector3 up = Mathf.Sin(time * wobbleTimeFactor + right.magnitude * wobbleSpaceFactor) * wobbleAmplitude * Vector3.up;
				storable.position = right + up;
				storable.rotation = Quaternion.identity;
				right += direction * cardDistance;
			}
			
			return new Vector3(
				inventory.Select(storable => storable.maxPossibleSize.x).Aggregate(ExtensionMethods.Add),
				inventory.Select(storable => storable.maxPossibleSize.y).Aggregate(Mathf.Max),
				inventory.Select(storable => storable.maxPossibleSize.z).Aggregate(Mathf.Max));
		}
		
		public static Vector3 MaxExtends(Vector3 prevMaxExtents, Vector3 position, Vector3 extent) =>
			new(MaxExtent(prevMaxExtents.x, position.x, extent.x),
				MaxExtent(prevMaxExtents.y, position.y, extent.y),
				MaxExtent(prevMaxExtents.z, position.z, extent.z));
		public static float MaxExtent(float prevMaxExtent, float position, float extent) =>
			Mathf.Max(prevMaxExtent, Mathf.Max(Mathf.Abs(position + extent), Mathf.Max(position - extent)));
		
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

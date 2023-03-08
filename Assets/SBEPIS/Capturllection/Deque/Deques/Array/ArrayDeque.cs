using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SBEPIS.Capturllection.Deques
{
	public class ArrayDeque : DequeBase
	{
		public float cardDistance = 0.1f;
		public Quaternion cardRotation = Quaternion.identity;
		public float wobbleHeight = 0.1f;
		public float wobbleTimeOffset = 1;
		
		private float time;
		
		public override void Tick(List<Storable> inventory, float delta)
		{
			time += delta;
		}
		
		public override void Layout(List<Storable> inventory, Vector3 direction)
		{
			int i = 0;
			Vector3 right = -cardDistance * (inventory.Count - 1) / 2 * direction;
			foreach (Storable storable in inventory)
			{
				Vector3 up = Mathf.Sin(time + i * wobbleTimeOffset) * wobbleHeight * Vector3.up;
				storable.position = right + up;
				storable.rotation = cardRotation;
				right += direction * cardDistance;
				i++;
			}
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

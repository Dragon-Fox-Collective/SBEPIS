using System.Collections.Generic;
using UnityEngine;

namespace SBEPIS.Capturllection.Deques
{
	public class QueueDeque : DequeBase
	{
		public float separatingDistance = 0.1f;
		public Quaternion cardRotation = Quaternion.identity;
		
		public override void Tick(List<Storable> inventory, float delta) { }
		
		public override void Layout(List<Storable> inventory)
		{
			Vector3 right = separatingDistance * (inventory.Count - 1) / 2 * Vector3.left;
			foreach (Storable storable in inventory)
			{
				storable.position = right;
				storable.rotation = cardRotation;
				right += Vector3.right * separatingDistance;
			}
		}
		
		public override bool CanFetchFrom(List<Storable> inventory, DequeStorable card) => inventory[^1].CanFetch(card);
		
		public override int GetIndexToStoreInto(List<Storable> inventory)
		{
			int index = inventory.FindIndex(storable => !storable.hasAllCardsEmpty);
			return index is -1 or 0 ? inventory.Count - 1 : index - 1;
		}
		public override int GetIndexToFlushBetween(List<Storable> inventory, Storable storable)
		{
			int index = inventory.FindIndex(storable => !storable.hasAllCardsEmpty);
			return index is -1 ? inventory.Count : index;
		}
		public override int GetIndexToInsertBetweenAfterStore(List<Storable> inventory, Storable storable, int originalIndex) => GetIndexToFlushBetween(inventory, storable);
		public override int GetIndexToInsertBetweenAfterFetch(List<Storable> inventory, Storable storable, int originalIndex) => GetIndexToFlushBetween(inventory, storable);
	}
}

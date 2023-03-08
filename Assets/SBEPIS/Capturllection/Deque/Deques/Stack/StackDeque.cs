using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SBEPIS.Capturllection.Deques
{
	public class StackDeque : DequeBase
	{
		public float separatingDistance = 0.1f;
		public Quaternion cardRotation = Quaternion.identity;
		
		public override void Tick(List<Storable> inventory, float delta) { }
		
		public override void Layout(List<Storable> inventory, Vector3 direction)
		{
			Vector3 right = separatingDistance * (inventory.Count - 1) / 2 * direction;
			foreach (Storable storable in inventory)
			{
				storable.position = right;
				storable.rotation = cardRotation;
				right += direction * separatingDistance;
			}
		}
		
		public override bool CanFetchFrom(List<Storable> inventory, DequeStorable card) => inventory[0].CanFetch(card);
		
		public override int GetIndexToStoreInto(List<Storable> inventory) => inventory.Count - 1;
		public override int GetIndexToFlushBetween(List<Storable> inventory, Storable storable) => storable.hasAllCardsEmpty ? inventory.Count : 0;
		public override int GetIndexToInsertBetweenAfterStore(List<Storable> inventory, Storable storable, int originalIndex) => 0;
		public override int GetIndexToInsertBetweenAfterFetch(List<Storable> inventory, Storable storable, int originalIndex) => inventory.Count;
	}
}

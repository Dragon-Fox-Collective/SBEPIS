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
		
		public override bool CanFetchFrom(List<Storable> inventory, DequeStorable card) => inventory[0].CanFetch(card);
		
		public override int GetIndexToStoreInto(List<Storable> inventory) => inventory.Count - 1;
		
		public override int GetIndexToInsertCardBetween(List<Storable> inventory, DequeStorable card) => 0;
	}
}

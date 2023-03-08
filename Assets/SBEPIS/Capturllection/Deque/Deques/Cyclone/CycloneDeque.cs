using System.Collections.Generic;
using UnityEngine;

namespace SBEPIS.Capturllection.Deques
{
	public class CycloneDeque : DequeBase
	{
		public float radius = 0.1f;
		public Quaternion cardRotation = Quaternion.identity;
		public float speed = 20;

		private float time;
		private Storable topStorable;

		public override void Tick(List<Storable> inventory, float delta)
		{
			time += delta;
			UpdateTopStorable(inventory);
		}

		private void UpdateTopStorable(List<Storable> inventory)
		{
			if (inventory.Count == 0)
			{
				topStorable = null;
				return;
			}
			
			float cardAngle = time * speed;
			float deltaAngle = 360f / inventory.Count;
			foreach (Storable storable in inventory)
			{
				float modAngle = cardAngle.ModAround(360);
				if (Mathf.Abs(modAngle) < deltaAngle / 2)
					topStorable = storable;
				cardAngle += deltaAngle;
			}
		}
		
		public override void Layout(List<Storable> inventory, Vector3 direction)
		{
			float cardAngle = time * speed;
			float deltaAngle = 360f / inventory.Count;
			foreach (Storable storable in inventory)
			{
				storable.position = Quaternion.Euler(0, 0, cardAngle) * Vector3.up * radius;
				storable.rotation = Quaternion.Euler(0, 0, cardAngle) * cardRotation;
				cardAngle += deltaAngle;
			}
		}
		
		public override bool CanFetchFrom(List<Storable> inventory, DequeStorable card) => topStorable.CanFetch(card);
		
		public override int GetIndexToStoreInto(List<Storable> inventory)
		{
			if (inventory.Contains(topStorable))
				return inventory.IndexOf(topStorable);
			
			int index = inventory.FindIndex(storable => !storable.hasAllCardsFull);
			return index is -1 ? 0 : index;
		}
		public override int GetIndexToFlushBetween(List<Storable> inventory, Storable storable) => inventory.Contains(topStorable) ? inventory.IndexOf(topStorable) : inventory.Count;
		public override int GetIndexToInsertBetweenAfterStore(List<Storable> inventory, Storable storable, int originalIndex) => originalIndex;
		public override int GetIndexToInsertBetweenAfterFetch(List<Storable> inventory, Storable storable, int originalIndex) => originalIndex;
	}
}

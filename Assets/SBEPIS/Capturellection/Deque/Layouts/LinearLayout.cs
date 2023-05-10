using System;
using System.Collections.Generic;
using System.Linq;
using SBEPIS.Capturellection.Storage;
using UnityEngine;

namespace SBEPIS.Capturellection.Deques
{
	[Serializable]
	public class LinearLayout : DequeLayoutBase
	{
		public bool offsetFromEnd = false;
		public float offset = 0.1f;
		
		public void Tick<TState>(List<Storable> inventory, TState state, float deltaTime) where TState : DirectionState
		{
			foreach (Storable storable in inventory)
			{
				storable.Direction = Quaternion.Euler(0, 0, -60) * state.Direction;
				storable.Tick(deltaTime);
			}
			
			List<Vector3> sizes = inventory.Select(storable => storable.MaxPossibleSize).ToList();
			Vector3 absDirection = state.Direction.Select(Mathf.Abs);
			float lengthSum = offsetFromEnd ?
				-offset * (inventory.Count - 1) + sizes.Select(size => Vector3.Project(size, absDirection)).Aggregate(ExtensionMethods.Add).magnitude :
				offset * (inventory.Count - 1);
			
			Vector3 right = -lengthSum / 2 * state.Direction;
			foreach ((Storable storable, Vector3 size) in inventory.Zip(sizes))
			{
				float length = Vector3.Project(size, absDirection).magnitude;
				right += state.Direction * (offsetFromEnd ? length / 2 : 0);
				
				storable.Position = right;
				storable.Rotation = DequeLayout.GetOffsetRotation(state.Direction);
				
				right += state.Direction * (offset + (offsetFromEnd ? length / 2 : 0));
			}
		}
	}
	
	[Serializable]
	public class LinearState : InventoryState, DirectionState
	{
		public List<Storable> Inventory { get; set; }
		public Vector3 Direction { get; set; }
	}
}

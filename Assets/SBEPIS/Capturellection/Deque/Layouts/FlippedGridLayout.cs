using System;
using System.Collections.Generic;
using System.Linq;
using SBEPIS.Capturellection.Storage;
using UnityEngine;

namespace SBEPIS.Capturellection.Deques
{
	[Serializable]
	public class FlippedGridLayout : DequeLayoutBase
	{
		public bool offsetXFromEnd = false;
		public float offsetX = 0.1f;
		public bool offsetYFromEnd = false;
		public float offsetY = 0.1f;
		
		public void Tick<TState>(List<Storable> inventory, TState state, float deltaTime) where TState : DirectionState, FlippedState
		{
			foreach (Storable storable in inventory)
			{
				storable.Direction = Quaternion.Euler(0, 0, -60) * state.Direction;
				storable.Tick(deltaTime);
			}
			
			List<Vector3> sizes = inventory.Select(storable => storable.MaxPossibleSize).ToList();
			Vector3 absDirection = state.Direction.Select(Mathf.Abs);
			float lengthSum = offsetXFromEnd
				? -offsetX * (inventory.Count - 1) + sizes.Select(size => Vector3.Project(size, absDirection)).Aggregate(ExtensionMethods.Add).magnitude
				: offsetX * (inventory.Count - 1);
			
			Vector3 right = -lengthSum / 2 * state.Direction;
			foreach ((Storable storable, Vector3 size) in inventory.Zip(sizes))
			{
				float length = Vector3.Project(size, absDirection).magnitude;
				right += state.Direction * (offsetXFromEnd ? length / 2 : 0);
				
				storable.Position = right;
				storable.Rotation = (!state.FlippedStorables.Contains(storable) && !storable.HasAllCardsEmpty ? Quaternion.Euler(0, 180, 0) : Quaternion.identity) * DequeLayout.GetOffsetRotation(state.Direction);
				
				right += state.Direction * (offsetX + (offsetXFromEnd ? length / 2 : 0));
			}
		}
	}
	
	[Serializable]
	public class FlippedGridState : InventoryState, DirectionState, FlippedState
	{
		public List<Storable> Inventory { get; set; }
		public Vector3 Direction { get; set; }
		public List<Storable> FlippedStorables { get; } = new();
	}
}

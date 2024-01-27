using System;
using System.Collections.Generic;
using System.Linq;
using SBEPIS.Capturellection.Storage;
using UnityEngine;

namespace SBEPIS.Capturellection.Deques
{
	[Serializable]
	public class WobblyLayout : DequeLayoutBase
	{
		public bool offsetFromEnd = false;
		public float offset = 0.1f;
		public float wobbleAmplitude = 0.1f;
		public float wobbleTimeFactor = 1;
		public float wobbleSpaceFactor = 1;
		
		public void Tick<TState>(IList<Storable> inventory, TState state, float deltaTime) where TState : DirectionState, TimeState
		{
			state.Time += deltaTime;
			
			foreach (Storable storable in inventory)
				storable.Tick(deltaTime);
		}
		
		public void Layout<TState>(IList<Storable> inventory, TState state) where TState : DirectionState, TimeState
		{
			foreach (Storable storable in inventory)
				storable.Layout(Quaternion.Euler(0, 0, -60) * state.Direction);
			
			List<Vector3> sizes = inventory.Select(storable => storable.MaxPossibleSize).ToList();
			Vector3 absDirection = state.Direction.Select(Mathf.Abs);
			float lengthSum = offsetFromEnd
				? offset * (inventory.Count - 1) + sizes.Select(size => Vector3.Project(size, absDirection)).Aggregate(VectorExtensions.Add).magnitude
				: offset * (inventory.Count - 1);
			
			Vector3 startRight = -lengthSum / 2 * state.Direction;
			Vector3 right = startRight;
			foreach ((Storable storable, Vector3 size) in inventory.Zip(sizes))
			{
				float length = Vector3.Project(size, absDirection).magnitude;
				right += state.Direction * (offsetFromEnd ? length / 2 : 0);
				
				Vector3 up = Mathf.Sin(state.Time * wobbleTimeFactor + (right - startRight).magnitude * wobbleSpaceFactor) * wobbleAmplitude * Vector3.up;
				
				storable.Position = right + up;
				storable.Rotation = DequeLayout.GetOffsetRotation(state.Direction);
				
				right += state.Direction * (offset + (offsetFromEnd ? length / 2 : 0));
			}
		}
	}
	
	public class WobblyState : InventoryState, DirectionState, TimeState
	{
		public CallbackList<Storable> Inventory { get; set; } = new();
		public Vector3 Direction { get; set; }
		public float Time { get; set; }
	}
	
	[Serializable]
	public class WobblySettings : LayoutSettings<WobblyLayout>
	{
		[SerializeField] private WobblyLayout layout;
		public WobblyLayout Layout => layout;
	}
}

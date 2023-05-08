using System;
using System.Collections.Generic;
using System.Linq;
using SBEPIS.Capturellection.Storage;
using UnityEngine;

namespace SBEPIS.Capturellection.Deques
{
	[Serializable]
	public class WobblyLayout
	{
		public bool offsetFromEnd = false;
		public float offset = 0.05f;
		public float wobbleAmplitude = 0.1f;
		public float wobbleTimeFactor = 1;
		public float wobbleSpaceFactor = 1;
		
		public void Tick<T>(List<Storable> inventory, T state, float deltaTime) where T : DirectionState, TimeState
		{
			state.Time += deltaTime;
			
			foreach (Storable storable in inventory)
			{
				storable.state.Direction = Quaternion.Euler(0, 0, -60) * state.Direction;
				storable.Tick(deltaTime);
			}
			
			List<Vector3> sizes = inventory.Select(storable => storable.MaxPossibleSize).ToList();
			Vector3 absDirection = state.Direction.Select(Mathf.Abs);
			float lengthSum = offsetFromEnd ?
				-offset * (inventory.Count - 1) + sizes.Select(size => Vector3.Project(size, absDirection)).Aggregate(ExtensionMethods.Add).magnitude :
				offset * (inventory.Count - 1);
			
			Vector3 startRight = -lengthSum / 2 * state.Direction;
			Vector3 right = startRight;
			foreach ((Storable storable, Vector3 size) in inventory.Zip(sizes))
			{
				float length = Vector3.Project(size, absDirection).magnitude;
				right += state.Direction * (offsetFromEnd ? length / 2 : 0);
				
				Vector3 up = Mathf.Sin(state.Time * wobbleTimeFactor + (right - startRight).magnitude * wobbleSpaceFactor) * wobbleAmplitude * Vector3.up;
				
				storable.Position = right + up;
				storable.Rotation = LinearLayout.GetOffsetRotation(state.Direction);
				
				right += state.Direction * (offset + (offsetFromEnd ? length / 2 : 0));
			}
		}
	}
	
	[Serializable]
	public class WobblyState : DirectionState, TimeState
	{
		public Vector3 Direction { get; set; }
		public float Time { get; set; }
	}
}

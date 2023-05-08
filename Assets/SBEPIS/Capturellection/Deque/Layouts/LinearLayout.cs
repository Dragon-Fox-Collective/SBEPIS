using System;
using System.Collections.Generic;
using System.Linq;
using SBEPIS.Capturellection.Storage;
using UnityEngine;

namespace SBEPIS.Capturellection.Deques
{
	[Serializable]
	public class LinearLayout
	{
		public bool offsetFromEnd = false;
		public float offset = 0.05f;
		
		public void Tick(List<Storable> inventory, DirectionState state, float deltaTime)
		{
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
			
			Vector3 right = -lengthSum / 2 * state.Direction;
			foreach ((Storable storable, Vector3 size) in inventory.Zip(sizes))
			{
				float length = Vector3.Project(size, absDirection).magnitude;
				right += state.Direction * (offsetFromEnd ? length / 2 : 0);
				
				storable.Position = right;
				storable.Rotation = GetOffsetRotation(state.Direction);
				
				right += state.Direction * (offset + (offsetFromEnd ? length / 2 : 0));
			}
		}
		
		public static Quaternion GetOffsetRotation(Vector3 direction) => Quaternion.AngleAxis(-5f, Vector3.Cross(direction, Vector3.forward));
	}
	
	[Serializable]
	public class LinearState : DirectionState
	{
		public Vector3 Direction { get; set; }
	}
}

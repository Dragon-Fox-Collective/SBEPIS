using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using SBEPIS.Capturellection.Storage;
using UnityEngine;
using UnityEngine.Serialization;

namespace SBEPIS.Capturellection.Deques
{
	public class ArrayDeque : DequeBase<ArrayState>
	{
		public bool offsetFromEnd = false;
		[FormerlySerializedAs("overlap")]
		public float offset = 0.05f;
		public float wobbleAmplitude = 0.1f;
		public float wobbleTimeFactor = 1;
		public float wobbleSpaceFactor = 1;
		
		public override void Tick(List<Storable> inventory, ArrayState state, float deltaTime)
		{
			state.time += deltaTime;
			
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
				
				Vector3 up = Mathf.Sin(state.time * wobbleTimeFactor + (right - startRight).magnitude * wobbleSpaceFactor) * wobbleAmplitude * Vector3.up;
				
				storable.Position = right + up;
				storable.Rotation = GetOffsetRotation(state.Direction);
				
				right += state.Direction * (offset + (offsetFromEnd ? length / 2 : 0));
			}
		}
		
		public static Quaternion GetOffsetRotation(Vector3 direction) => Quaternion.AngleAxis(-5f, Vector3.Cross(direction, Vector3.forward));

		public static void TickLinearLayout(List<Storable> inventory, DequeRulesetState state, float deltaTime, bool offsetFromEnd, float offset)
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
		
		public override bool CanFetchFrom(List<Storable> inventory, ArrayState state, InventoryStorable card) => inventory.Any(storable => storable.CanFetch(card));
	}
}

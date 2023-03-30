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
				storable.state.direction = Quaternion.Euler(0, 0, -60) * state.direction;
				storable.Tick(deltaTime);
			}
			
			List<Vector3> sizes = inventory.Select(storable => storable.MaxPossibleSize).ToList();
			Vector3 absDirection = state.direction.Select(Mathf.Abs);
			float lengthSum = offsetFromEnd ?
				-offset * (inventory.Count - 1) + sizes.Select(size => Vector3.Project(size, absDirection)).Aggregate(ExtensionMethods.Add).magnitude :
				offset * (inventory.Count - 1);
			
			Vector3 startRight = -lengthSum / 2 * state.direction;
			Vector3 right = startRight;
			foreach ((Storable storable, Vector3 size) in inventory.Zip(sizes))
			{
				float length = Vector3.Project(size, absDirection).magnitude;
				right += state.direction * (offsetFromEnd ? length / 2 : 0);
				
				Vector3 up = Mathf.Sin(state.time * wobbleTimeFactor + (right - startRight).magnitude * wobbleSpaceFactor) * wobbleAmplitude * Vector3.up;
				
				storable.Position = right + up;
				storable.Rotation = GetOffsetRotation(state.direction);
				
				right += state.direction * (offset + (offsetFromEnd ? length / 2 : 0));
			}
		}
		public override Vector3 GetMaxPossibleSizeOf(List<Storable> inventory, ArrayState state) => GetSizeFromExistingLayout(inventory);
		public static Vector3 GetSizeFromExistingLayout(IEnumerable<Storable> inventory)
		{
			return inventory.Select(storable => new Bounds(storable.Position, storable.MaxPossibleSize)).Aggregate(new Bounds(), (current, bounds) => current.Containing(bounds)).size;
		}
		public static Quaternion GetOffsetRotation(Vector3 direction) => Quaternion.AngleAxis(-5f, Vector3.Cross(direction, Vector3.forward));
		
		public override bool CanFetchFrom(List<Storable> inventory, ArrayState state, DequeStorable card) => inventory.Any(storable => storable.CanFetch(card));
		
		public override async UniTask<int> GetIndexToStoreInto(List<Storable> inventory, ArrayState state)
		{
			await UniTask.Delay(TimeSpan.FromSeconds(1), DelayType.Realtime);
			int index = inventory.FindIndex(storable => !storable.HasAllCardsFull);
			return index is -1 ? 0 : index;
		}
		public override UniTask<int> GetIndexToFlushBetween(List<Storable> inventory, ArrayState state, Storable storable) => UniTask.FromResult(inventory.Count);
		public override UniTask<int> GetIndexToInsertBetweenAfterStore(List<Storable> inventory, ArrayState state, Storable storable, int originalIndex) => UniTask.FromResult(originalIndex);
		public override UniTask<int> GetIndexToInsertBetweenAfterFetch(List<Storable> inventory, ArrayState state, Storable storable, int originalIndex) => UniTask.FromResult(originalIndex);
	}
}

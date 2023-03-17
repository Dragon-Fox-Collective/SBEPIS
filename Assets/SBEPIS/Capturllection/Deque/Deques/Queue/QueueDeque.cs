using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Serialization;

namespace SBEPIS.Capturllection.Deques
{
	public class QueueDeque : DequeBase<BaseState>
	{
		public bool offsetFromEnd = false;
		[FormerlySerializedAs("overlap")]
		public float offset = 0.05f;
		
		public override void Tick(List<Storable> inventory, BaseState state, float deltaTime)
		{
			foreach (Storable storable in inventory)
			{
				storable.state.direction = Quaternion.Euler(0, 0, -60) * state.direction;
				storable.Tick(deltaTime);
			}
			
			List<Vector3> sizes = inventory.Select(storable => storable.maxPossibleSize).ToList();
			Vector3 absDirection = state.direction.Select(Mathf.Abs);
			float lengthSum = offsetFromEnd ?
				-offset * (inventory.Count - 1) + sizes.Select(size => Vector3.Project(size, absDirection)).Aggregate(ExtensionMethods.Add).magnitude :
				offset * (inventory.Count - 1);
			
			Vector3 right = -lengthSum / 2 * state.direction;
			foreach ((Storable storable, Vector3 size) in inventory.Zip(sizes))
			{
				float length = Vector3.Project(size, absDirection).magnitude;
				right += state.direction * (offsetFromEnd ? length / 2 : 0);
				
				storable.position = right;
				storable.rotation = ArrayDeque.GetOffsetRotation(state.direction);
				
				right += state.direction * (offset + (offsetFromEnd ? length / 2 : 0));
			}
		}
		public override Vector3 GetMaxPossibleSizeOf(List<Storable> inventory, BaseState state) => ArrayDeque.GetSizeFromExistingLayout(inventory);
		
		public override bool CanFetchFrom(List<Storable> inventory, BaseState state, DequeStorable card) => inventory[^1].CanFetch(card);
		
		public override UniTask<int> GetIndexToStoreInto(List<Storable> inventory, BaseState state)
		{
			int index = inventory.FindIndex(storable => !storable.hasAllCardsEmpty);
			return UniTask.FromResult(index is -1 or 0 ? inventory.Count - 1 : index - 1);
		}
		public override int GetIndexToFlushBetween(List<Storable> inventory, BaseState state, Storable storable)
		{
			int index = inventory.FindIndex(storable => !storable.hasAllCardsEmpty);
			return index is -1 ? inventory.Count : index;
		}
		public override int GetIndexToInsertBetweenAfterStore(List<Storable> inventory, BaseState state, Storable storable, int originalIndex) => GetIndexToFlushBetween(inventory, state, storable);
		public override int GetIndexToInsertBetweenAfterFetch(List<Storable> inventory, BaseState state, Storable storable, int originalIndex) => GetIndexToFlushBetween(inventory, state, storable);
	}
}

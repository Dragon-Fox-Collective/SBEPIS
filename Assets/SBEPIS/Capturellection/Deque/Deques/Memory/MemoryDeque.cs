using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using SBEPIS.Capturellection.Storage;
using UnityEngine;

namespace SBEPIS.Capturellection.Deques
{
	public class MemoryDeque : DequeBase<MemoryState>
	{
		public bool offsetXFromEnd = false;
		public float offsetX = 0.05f;
		public bool offsetYFromEnd = false;
		public float offsetY = 0.05f;
		public Storable memoryCardPrefab;
		
		public override void Tick(List<Storable> inventory, MemoryState state, float deltaTime)
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
		public override Vector3 GetMaxPossibleSizeOf(List<Storable> inventory, MemoryState state) => ArrayDeque.GetSizeFromExistingLayout(inventory);
		
		public override bool CanFetchFrom(List<Storable> inventory, MemoryState state, InventoryStorable card) => inventory.Any(storable => storable.CanFetch(card));
		
		public override void StoreSync(List<Storable> inventory, MemoryState state) => UniTask.FromResult(Mathf.Max(inventory.FindIndex(storable => !storable.HasAllCardsFull), 0));
		public override void FlushSync(List<Storable> inventory, MemoryState state, Storable storable)
		{
			(Storable, Storable) cards = (InstantiateCard(storable), InstantiateCard(storable));
			state.backingInventory[storable] = cards;
			inventory.Add(cards.Item1);
			inventory.Add(cards.Item2);
		}
		private Storable InstantiateCard(Storable storable)
		{
			Storable card = Instantiate(memoryCardPrefab, storable.transform.parent);
			return card;
		}
		public override void RestoreAfterStoreSync(List<Storable> inventory, MemoryState state, Storable storable, int originalIndex) => UniTask.FromResult(originalIndex);
		public override void RestoreAfterFetchSync(List<Storable> inventory, MemoryState state, Storable storable, int originalIndex) => UniTask.FromResult(originalIndex);
	}
}

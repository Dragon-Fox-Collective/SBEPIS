using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using SBEPIS.Capturellection.Storage;
using UnityEngine;

namespace SBEPIS.Capturellection.Deques
{
	public class StateDeque : DequeBase<BaseState>
	{
		public bool offsetFromEnd = false;
		public float offset = 0.05f;
		
		public bool State { get; set; }
		
		public override void Tick(List<Storable> inventory, BaseState state, float deltaTime) => ArrayDeque.TickLinearLayout(inventory, state, deltaTime, offsetFromEnd, offset);
		public override Vector3 GetMaxPossibleSizeOf(List<Storable> inventory, BaseState state) => ArrayDeque.GetSizeFromExistingLayout(inventory);
		
		public override bool CanFetchFrom(List<Storable> inventory, BaseState state, InventoryStorable card) => State && inventory.Any(storable => storable.CanFetch(card));
		
		public override async UniTask<int> GetIndexToStoreInto(List<Storable> inventory, BaseState state)
		{
			await UniTask.WaitUntil(() => State);
			int index = inventory.FindIndex(storable => !storable.HasAllCardsFull);
			return index is -1 ? 0 : index;
		}
		public override UniTask<int> GetIndexToFlushBetween(List<Storable> inventory, BaseState state, Storable storable) => UniTask.FromResult(inventory.Count);
		public override UniTask<int> GetIndexToInsertBetweenAfterStore(List<Storable> inventory, BaseState state, Storable storable, int originalIndex) => UniTask.FromResult(originalIndex);
		public override UniTask<int> GetIndexToInsertBetweenAfterFetch(List<Storable> inventory, BaseState state, Storable storable, int originalIndex) => UniTask.FromResult(originalIndex);
	}
}

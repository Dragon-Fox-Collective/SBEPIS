using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using SBEPIS.Capturellection.Storage;
using UnityEngine;
using UnityEngine.Serialization;

namespace SBEPIS.Capturellection.Deques
{
	public class QueueDeque : DequeBase<BaseState>
	{
		public bool offsetFromEnd = false;
		[FormerlySerializedAs("overlap")]
		public float offset = 0.05f;
		
		public override void Tick(List<Storable> inventory, BaseState state, float deltaTime) => ArrayDeque.TickLinearLayout(inventory, state, deltaTime, offsetFromEnd, offset);
		public override Vector3 GetMaxPossibleSizeOf(List<Storable> inventory, BaseState state) => ArrayDeque.GetSizeFromExistingLayout(inventory);
		
		public override bool CanFetchFrom(List<Storable> inventory, BaseState state, InventoryStorable card) => inventory[^1].CanFetch(card);
		
		public override UniTaskVoid Store(List<Storable> inventory, BaseState state)
		{
			int index = inventory.FindIndex(storable => !storable.HasAllCardsEmpty);
			return UniTask.FromResult(index is -1 or 0 ? inventory.Count - 1 : index - 1);
		}
		public override UniTask<int> Flush(List<Storable> inventory, BaseState state, Storable storable)
		{
			int index = inventory.FindIndex(storable => !storable.HasAllCardsEmpty);
			return UniTask.FromResult(index is -1 ? inventory.Count : index);
		}
		public override UniTaskVoid RestoreAfterStore(List<Storable> inventory, BaseState state, Storable storable, int originalIndex) => Flush(inventory, state, storable);
		public override UniTask<int> RestoreAfterFetch(List<Storable> inventory, BaseState state, Storable storable, int originalIndex) => Flush(inventory, state, storable);
	}
}

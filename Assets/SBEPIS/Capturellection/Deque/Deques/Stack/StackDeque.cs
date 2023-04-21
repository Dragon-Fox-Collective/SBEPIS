using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using SBEPIS.Capturellection.Storage;
using UnityEngine;
using UnityEngine.Serialization;

namespace SBEPIS.Capturellection.Deques
{
	public class StackDeque : DequeBase<BaseState>
	{
		public bool offsetFromEnd = false;
		public float offset = 0.05f;
		
		public override void Tick(List<Storable> inventory, BaseState state, float deltaTime) => ArrayDeque.TickLinearLayout(inventory, state, deltaTime, offsetFromEnd, offset);
		public override Vector3 GetMaxPossibleSizeOf(List<Storable> inventory, BaseState state) => ArrayDeque.GetSizeFromExistingLayout(inventory);
		
		public override bool CanFetchFrom(List<Storable> inventory, BaseState state, InventoryStorable card) => inventory[0].CanFetch(card);
		
		public override UniTaskVoid Store(List<Storable> inventory, BaseState state) => UniTask.FromResult(inventory.Count - 1);
		public override UniTask<int> Flush(List<Storable> inventory, BaseState state, Storable storable) => UniTask.FromResult(storable.HasAllCardsEmpty ? inventory.Count : 0);
		public override UniTaskVoid RestoreAfterStore(List<Storable> inventory, BaseState state, Storable storable, int originalIndex) => UniTask.FromResult(0);
		public override UniTask<int> RestoreAfterFetch(List<Storable> inventory, BaseState state, Storable storable, int originalIndex) => UniTask.FromResult(inventory.Count);
	}
}

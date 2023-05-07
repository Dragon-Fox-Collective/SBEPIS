using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using SBEPIS.Capturellection.Storage;

namespace SBEPIS.Capturellection.Deques
{
	public class StackDeque : DequeBase<BaseState>
	{
		public bool offsetFromEnd = false;
		public float offset = 0.05f;
		
		public override void Tick(List<Storable> inventory, BaseState state, float deltaTime) => ArrayDeque.TickLinearLayout(inventory, state, deltaTime, offsetFromEnd, offset);
		
		public override bool CanFetchFrom(List<Storable> inventory, BaseState state, InventoryStorable card) => inventory[0].CanFetch(card);
		
		public override async UniTask<DequeStoreResult> StoreItem(List<Storable> inventory, BaseState state, Capturellectable item)
		{
			Storable storable = inventory[^1];
			inventory.Remove(storable);
			StorableStoreResult res = await storable.StoreItem(item);
			inventory.Insert(0, storable);
			return res.ToDequeResult(inventory.Count - 1, storable);
		}
		
		public override async UniTask<Capturellectable> FetchItem(List<Storable> inventory, BaseState state, InventoryStorable card)
		{
			Storable storable = inventory[0];
			inventory.Remove(storable);
			Capturellectable item = await storable.FetchItem(card);
			inventory.Add(storable);
			return item;
		}
	}
}

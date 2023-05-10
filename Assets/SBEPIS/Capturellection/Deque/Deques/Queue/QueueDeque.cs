using Cysharp.Threading.Tasks;
using SBEPIS.Capturellection.Storage;

namespace SBEPIS.Capturellection.Deques
{
	public class QueueDeque : LaidOutDeque<LinearLayout, LinearState>
	{
		public override bool CanFetchFrom(LinearState state, InventoryStorable card) => state.Inventory[^1].CanFetch(card);
		
		public override async UniTask<DequeStoreResult> StoreItem(LinearState state, Capturellectable item)
		{
			int index = state.Inventory.FindIndex(invStorable => !invStorable.HasAllCardsEmpty) - 1;
			if (index < 0) index = state.Inventory.Count - 1;
			Storable storable = state.Inventory[index];
			state.Inventory.Remove(storable);
			StorableStoreResult res = await storable.StoreItem(item);
			state.Inventory.Insert(0, storable);
			return res.ToDequeResult(index, storable);
		}
		
		public override UniTask<Capturellectable> FetchItem(LinearState state, InventoryStorable card)
		{
			Storable storable = state.Inventory[^1];
			state.Inventory.Remove(storable);
			state.Inventory.Insert(0, storable);
			return storable.FetchItem(card);
		}
		
		public override UniTask FlushCard(LinearState state, Storable storable)
		{
			int index = state.Inventory.FindIndex(invStorable => !invStorable.HasAllCardsEmpty) - 1;
			if (index < 0) index = state.Inventory.Count - 1;
			state.Inventory.Insert(index, storable);
			return UniTask.CompletedTask;
		}
	}
}

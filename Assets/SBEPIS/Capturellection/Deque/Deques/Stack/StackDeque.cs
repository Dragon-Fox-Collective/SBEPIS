using Cysharp.Threading.Tasks;
using SBEPIS.Capturellection.Storage;

namespace SBEPIS.Capturellection.Deques
{
	public class StackDeque : LaidOutDeque<LinearSettings, LinearLayout, LinearState>
	{
		public override bool CanFetch(LinearState state, InventoryStorable card) => state.Inventory[0].CanFetch(card);
		
		public override async UniTask<DequeStoreResult> StoreItem(LinearState state, Capturellectable item)
		{
			Storable storable = state.Inventory[^1];
			state.Inventory.Remove(storable);
			StorableStoreResult res = await storable.StoreItem(item);
			state.Inventory.Insert(0, storable);
			return res.ToDequeResult(state.Inventory.Count - 1, storable);
		}
		
		public override UniTask<Capturellectable> FetchItem(LinearState state, InventoryStorable card)
		{
			Storable storable = state.Inventory[0];
			state.Inventory.Remove(storable);
			state.Inventory.Add(storable);
			return storable.FetchItem(card);
		}
	}
}

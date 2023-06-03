using Cysharp.Threading.Tasks;
using SBEPIS.Capturellection.Storage;

namespace SBEPIS.Capturellection.Deques
{
	public class QueueDeque : LaidOutDeque<LinearSettings, LinearLayout, LinearState>
	{
		public override bool CanFetch(LinearState state, InventoryStorable card) => state.Inventory[^1].CanFetch(card);
		
		public override async UniTask<StoreResult> StoreItem(LinearState state, Capturellectable item)
		{
			int index = state.Inventory.FindIndex(invStorable => !invStorable.HasAllCardsEmpty) - 1;
			if (index < 0) index = state.Inventory.Count - 1;
			Storable storable = state.Inventory[index];
			state.Inventory.Remove(storable);
			StoreResult res = await storable.StoreItem(item);
			state.Inventory.Insert(0, storable);
			return res;
		}
		
		public override UniTask<FetchResult> FetchItem(LinearState state, InventoryStorable card)
		{
			Storable storable = state.Inventory[^1];
			state.Inventory.Remove(storable);
			state.Inventory.Insert(0, storable);
			return storable.FetchItem(card);
		}
	}
}

using Cysharp.Threading.Tasks;
using SBEPIS.Capturellection.Storage;

namespace SBEPIS.Capturellection.Deques
{
	public class StackDeque : LaidOutRuleset<LinearSettings, LinearLayout, LinearState>
	{
		protected override bool CanFetch(LinearState state, InventoryStorable card) => state.Inventory[0].CanFetch(card);
		
		protected override async UniTask<StoreResult> StoreItem(LinearState state, Capturellectable item)
		{
			Storable storable = state.Inventory[^1];
			state.Inventory.Remove(storable);
			StoreResult res = await storable.StoreItem(item);
			state.Inventory.Insert(0, storable);
			return res;
		}
		
		protected override UniTask<FetchResult> FetchItem(LinearState state, InventoryStorable card)
		{
			Storable storable = state.Inventory[0];
			state.Inventory.Remove(storable);
			state.Inventory.Add(storable);
			return storable.FetchItem(card);
		}
	}
}

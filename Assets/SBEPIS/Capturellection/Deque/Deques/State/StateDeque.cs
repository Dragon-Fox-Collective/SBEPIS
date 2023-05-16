using System.Linq;
using Cysharp.Threading.Tasks;
using SBEPIS.Capturellection.Storage;

namespace SBEPIS.Capturellection.Deques
{
	public class StateDeque : LaidOutDeque<LinearSettings, LinearLayout, LinearState>
	{
		public bool State { get; set; }
		
		public override bool CanFetchFrom(LinearState state, InventoryStorable card) => State && state.Inventory.Any(storable => storable.CanFetch(card));
		
		public override async UniTask<DequeStoreResult> StoreItem(LinearState state, Capturellectable item)
		{
			await UniTask.WaitUntil(() => State);
			return await base.StoreItem(state, item);
		}
	}
}

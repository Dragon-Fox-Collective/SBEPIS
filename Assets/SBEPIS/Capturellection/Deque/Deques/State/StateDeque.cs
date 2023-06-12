using System.Linq;
using Cysharp.Threading.Tasks;
using SBEPIS.Capturellection.Storage;

namespace SBEPIS.Capturellection.Deques
{
	public class StateDeque : LaidOutRuleset<LinearSettings, LinearLayout, LinearState>
	{
		public bool State { get; set; }
		
		protected override bool CanFetch(LinearState state, InventoryStorable card) => State && state.Inventory.Any(storable => storable.CanFetch(card));
		
		protected override async UniTask<StoreResult> StoreItem(LinearState state, Capturellectable item)
		{
			await UniTask.WaitUntil(() => State);
			return await base.StoreItem(state, item);
		}
	}
}

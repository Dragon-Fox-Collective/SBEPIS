using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using SBEPIS.Capturellection.Storage;

namespace SBEPIS.Capturellection.Deques
{
	public class StateDeque : DequeBase<BaseState>
	{
		public bool offsetFromEnd = false;
		public float offset = 0.05f;
		
		public bool State { get; set; }
		
		public override void Tick(List<Storable> inventory, BaseState state, float deltaTime) => ArrayDeque.TickLinearLayout(inventory, state, deltaTime, offsetFromEnd, offset);
		
		public override bool CanFetchFrom(List<Storable> inventory, BaseState state, InventoryStorable card) => State && inventory.Any(storable => storable.CanFetch(card));
		
		public override async UniTask<DequeStoreResult> StoreItem(List<Storable> inventory, BaseState state, Capturellectable item)
		{
			await UniTask.WaitUntil(() => State);
			return await base.StoreItem(inventory, state, item);
		}
	}
}

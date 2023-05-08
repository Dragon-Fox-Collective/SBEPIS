using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using SBEPIS.Capturellection.Storage;
using UnityEngine;

namespace SBEPIS.Capturellection.Deques
{
	public class StateDeque : SingleDeque<LinearLayout, LinearState>
	{
		[SerializeField] private LinearLayout layout;
		
		public bool State { get; set; }
		
		public override void Tick(List<Storable> inventory, LinearState state, float deltaTime) => layout.Tick(inventory, state, deltaTime);
		
		public override bool CanFetchFrom(List<Storable> inventory, LinearState state, InventoryStorable card) => State && inventory.Any(storable => storable.CanFetch(card));
		
		public override async UniTask<DequeStoreResult> StoreItem(List<Storable> inventory, LinearState state, Capturellectable item)
		{
			await UniTask.WaitUntil(() => State);
			return await base.StoreItem(inventory, state, item);
		}
		
		protected override LinearLayout SettingsPageLayoutData => layout;
	}
}

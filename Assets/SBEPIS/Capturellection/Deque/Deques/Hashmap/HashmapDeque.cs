using Cysharp.Threading.Tasks;
using SBEPIS.Capturellection.Storage;
using UnityEngine;

namespace SBEPIS.Capturellection.Deques
{
	public class HashmapDeque : LaidOutDeque<LinearLayout, LinearState>
	{
		[SerializeField] private GameObject keyboardPrefab;
		[SerializeField] private HashFunction hashFunction;
		
		private Keyboard keyboard;
		
		public override void SetupPage(LinearState state, DiajectorPage page)
		{
			keyboard = Instantiate(keyboardPrefab, page.transform).GetComponentInChildren<Keyboard>();
		}
		
		public override bool CanFetchFrom(LinearState state, InventoryStorable card)
		{
			if (keyboard.Text.Length == 0)
				return false;
			
			int index = hashFunction.Hash(keyboard.Text, state.Inventory.Count);
			return state.Inventory[index].CanFetch(card);
		}
		
		public override async UniTask<DequeStoreResult> StoreItem(LinearState state, Capturellectable item)
		{
			int index = hashFunction.Hash(keyboard.Text, state.Inventory.Count);
			Storable storable = state.Inventory[index];
			StorableStoreResult res = await storable.StoreItem(item);
			
			keyboard.Text = "";
			
			return res.ToDequeResult(index, storable);
		}
		
		public override UniTask<Capturellectable> FetchItem(LinearState state, InventoryStorable card)
		{
			keyboard.Text = "";
			return base.FetchItem(state, card);
		}
	}
}

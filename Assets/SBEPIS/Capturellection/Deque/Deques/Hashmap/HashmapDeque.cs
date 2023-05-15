using Cysharp.Threading.Tasks;
using SBEPIS.Bits;
using SBEPIS.Capturellection.Storage;
using SBEPIS.Items;
using UnityEngine;

namespace SBEPIS.Capturellection.Deques
{
	public class HashmapDeque : LaidOutDeque<LinearLayout, LinearState>
	{
		[SerializeField] private HashFunction hashFunction;
		
		private string currentKey;
		
		public override bool CanFetchFrom(LinearState state, InventoryStorable card)
		{
			int index = hashFunction.Hash(currentKey, state.Inventory.Count);
			return state.Inventory[index].CanFetch(card);
		}
		
		public override async UniTask<DequeStoreResult> StoreItem(LinearState state, Capturellectable item)
		{
			int index = hashFunction.Hash(BitManager.instance.Bits.BitSetToCode(item.GetComponentInParent<Item>().Module.Bits.Bits), state.Inventory.Count);
			Storable storable = state.Inventory[index];
			StorableStoreResult res = await storable.StoreItem(item);
			return res.ToDequeResult(index, storable);
		}
	}
}

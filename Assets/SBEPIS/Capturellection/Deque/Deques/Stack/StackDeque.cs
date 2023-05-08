using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using SBEPIS.Capturellection.Storage;
using UnityEngine;

namespace SBEPIS.Capturellection.Deques
{
	public class StackDeque : LaidOutDeque<LinearLayout, LinearState>
	{
		public override bool CanFetchFrom(List<Storable> inventory, LinearState state, InventoryStorable card) => inventory[0].CanFetch(card);
		
		public override async UniTask<DequeStoreResult> StoreItem(List<Storable> inventory, LinearState state, Capturellectable item)
		{
			Storable storable = inventory[^1];
			inventory.Remove(storable);
			StorableStoreResult res = await storable.StoreItem(item);
			inventory.Insert(0, storable);
			return res.ToDequeResult(inventory.Count - 1, storable);
		}
		
		public override async UniTask<Capturellectable> FetchItem(List<Storable> inventory, LinearState state, InventoryStorable card)
		{
			Storable storable = inventory[0];
			inventory.Remove(storable);
			Capturellectable item = await storable.FetchItem(card);
			inventory.Add(storable);
			return item;
		}
	}
}

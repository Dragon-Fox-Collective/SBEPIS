using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using SBEPIS.Capturellection.Storage;
using UnityEngine;

namespace SBEPIS.Capturellection.Deques
{
	public class QueueDeque : LaidOutDeque<LinearLayout, LinearState>
	{
		public override bool CanFetchFrom(List<Storable> inventory, LinearState state, InventoryStorable card) => inventory[^1].CanFetch(card);
		
		public override async UniTask<DequeStoreResult> StoreItem(List<Storable> inventory, LinearState state, Capturellectable item)
		{
			int index = inventory.FindIndex(invStorable => !invStorable.HasAllCardsEmpty) - 1;
			if (index < 0) index = inventory.Count - 1;
			Storable storable = inventory[index];
			inventory.Remove(storable);
			StorableStoreResult res = await storable.StoreItem(item);
			inventory.Insert(0, storable);
			return res.ToDequeResult(index, storable);
		}
		
		public override async UniTask<Capturellectable> FetchItem(List<Storable> inventory, LinearState state, InventoryStorable card)
		{
			Storable storable = inventory[^1];
			inventory.Remove(storable);
			Capturellectable item = await storable.FetchItem(card);
			inventory.Insert(0, storable);
			return item;
		}
		
		public override UniTask FlushCard(List<Storable> inventory, LinearState state, Storable storable)
		{
			int index = inventory.FindIndex(invStorable => !invStorable.HasAllCardsEmpty) - 1;
			if (index < 0) index = inventory.Count - 1;
			inventory.Insert(index, storable);
			return UniTask.CompletedTask;
		}
	}
}

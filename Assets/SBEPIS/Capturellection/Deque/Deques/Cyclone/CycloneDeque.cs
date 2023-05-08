using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using SBEPIS.Capturellection.Storage;
using UnityEngine;

namespace SBEPIS.Capturellection.Deques
{
	public class CycloneDeque : LaidOutDeque<CycloneLayout, CycloneState>
	{
		public override bool CanFetchFrom(List<Storable> inventory, CycloneState state, InventoryStorable card) => state.TopStorable.CanFetch(card);
		
		public override async UniTask<DequeStoreResult> StoreItem(List<Storable> inventory, CycloneState state, Capturellectable item)
		{
			int index = inventory.IndexOf(state.TopStorable);
			if (index < 0) index = Mathf.Max(inventory.FindIndex(storable => !storable.HasAllCardsFull), 0);
			Storable storable = inventory[index];
			StorableStoreResult res = await storable.StoreItem(item);
			return res.ToDequeResult(index, storable);
		}

		public override UniTask FlushCard(List<Storable> inventory, CycloneState state, Storable storable)
		{
			int index = inventory.IndexOf(state.TopStorable);
			if (index < 0) index = inventory.Count;
			inventory.Insert(index, storable);
			return UniTask.CompletedTask;
		}
	}
}

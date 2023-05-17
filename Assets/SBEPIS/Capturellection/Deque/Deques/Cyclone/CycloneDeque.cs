using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using SBEPIS.Capturellection.Storage;
using UnityEngine;

namespace SBEPIS.Capturellection.Deques
{
	public class CycloneDeque : LaidOutDeque<CycloneSettings, CycloneLayout, CycloneState>
	{
		public override bool CanFetch(CycloneState state, InventoryStorable card) => state.TopStorable.CanFetch(card);
		
		public override async UniTask<DequeStoreResult> StoreItem(CycloneState state, Capturellectable item)
		{
			int index = state.Inventory.IndexOf(state.TopStorable);
			if (index < 0) index = Mathf.Max(state.Inventory.FindIndex(storable => !storable.HasAllCardsFull), 0);
			Storable storable = state.Inventory[index];
			StorableStoreResult res = await storable.StoreItem(item);
			return res.ToDequeResult(index, storable);
		}
		
		public override UniTask FlushCard(CycloneState state, Storable storable)
		{
			int index = state.Inventory.IndexOf(state.TopStorable);
			if (index < 0) index = state.Inventory.Count;
			state.Inventory.Insert(index, storable);
			return UniTask.CompletedTask;
		}
	}
}

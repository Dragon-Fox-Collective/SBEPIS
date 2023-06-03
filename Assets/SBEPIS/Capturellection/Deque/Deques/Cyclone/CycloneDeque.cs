using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using SBEPIS.Capturellection.Storage;
using UnityEngine;

namespace SBEPIS.Capturellection.Deques
{
	public class CycloneDeque : LaidOutDeque<CycloneSettings, CycloneLayout, CycloneState>
	{
		public override bool CanFetch(CycloneState state, InventoryStorable card) => state.TopStorable.CanFetch(card);
		
		public override UniTask<StoreResult> StoreItem(CycloneState state, Capturellectable item)
		{
			int index = state.Inventory.IndexOf(state.TopStorable);
			if (index < 0) index = Mathf.Max(state.Inventory.FindIndex(storable => !storable.HasAllCardsFull), 0);
			Storable storable = state.Inventory[index];
			return storable.StoreItem(item);
		}
	}
}

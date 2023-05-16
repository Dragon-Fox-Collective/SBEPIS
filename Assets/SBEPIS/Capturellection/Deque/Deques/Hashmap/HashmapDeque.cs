using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using SBEPIS.Bits;
using SBEPIS.Capturellection.Storage;
using SBEPIS.Items;
using UnityEngine;

namespace SBEPIS.Capturellection.Deques
{
	public class HashmapDeque : LaidOutDeque<LinearLayout, TextState>
	{
		[SerializeField] private GameObject keyboardPrefab;
		[SerializeField] private HashFunction hashFunction;
		
		public override void SetupPage(TextState state, DiajectorPage page)
		{
			Keyboard keyboard = Instantiate(keyboardPrefab, page.transform).GetComponentInChildren<Keyboard>();
			state.Text = keyboard.Text;
			keyboard.onType.AddListener(text => state.Text = text);
		}
		
		public override bool CanFetchFrom(TextState state, InventoryStorable card)
		{
			int index = hashFunction.Hash(state.Text, state.Inventory.Count);
			return state.Inventory[index].CanFetch(card);
		}
		
		public override async UniTask<DequeStoreResult> StoreItem(TextState state, Capturellectable item)
		{
			int index = hashFunction.Hash(state.Text, state.Inventory.Count);
			Storable storable = state.Inventory[index];
			StorableStoreResult res = await storable.StoreItem(item);
			return res.ToDequeResult(index, storable);
		}
	}
	
	[Serializable]
	public class TextState : InventoryState, DirectionState
	{
		public List<Storable> Inventory { get; set; } = new();
		public Vector3 Direction { get; set; }
		public string Text { get; set; }
	}
}

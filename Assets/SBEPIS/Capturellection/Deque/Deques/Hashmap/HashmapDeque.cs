using System;
using Cysharp.Threading.Tasks;
using SBEPIS.Capturellection.Storage;
using UnityEngine;

namespace SBEPIS.Capturellection.Deques
{
	public class HashmapDeque : LaidOutRuleset<HashmapSettings, LinearLayout, LinearState>
	{
		[SerializeField] private GameObject keyboardPrefab;
		
		private Keyboard keyboard;
		
		protected override void InitPageOnce(LinearState state, DiajectorPage page)
		{
			keyboard = Instantiate(keyboardPrefab, page.transform).GetComponentInChildren<Keyboard>();
		}
		
		protected override bool CanFetch(LinearState state, InventoryStorable card)
		{
			if (keyboard.Text.Length == 0)
				return false;
			
			int index = Settings.HashFunction.Hash(keyboard.Text, state.Inventory.Count);
			return state.Inventory[index].CanFetch(card);
		}
		
		protected override async UniTask<StoreResult> StoreItem(LinearState state, Capturellectable item)
		{
			int index = Settings.HashFunction.Hash(keyboard.Text, state.Inventory.Count);
			Storable storable = state.Inventory[index];
			StoreResult res = await storable.StoreItem(item);
			
			keyboard.Text = "";
			
			return res;
		}
		
		protected override UniTask<FetchResult> FetchItem(LinearState state, InventoryStorable card)
		{
			keyboard.Text = "";
			return base.FetchItem(state, card);
		}
	}
	
	public interface HashFunctionSettings
	{
		public HashFunction HashFunction { get; set; }
	}
	
	[Serializable]
	public class HashmapSettings : LayoutSettings<LinearLayout>, HashFunctionSettings
	{
		[SerializeField] private LinearLayout layout;
		public LinearLayout Layout => layout;
		[SerializeField] private HashFunction hashFunction;
		public HashFunction HashFunction
		{
			get => hashFunction;
			set => hashFunction = value;
		}
	}
}

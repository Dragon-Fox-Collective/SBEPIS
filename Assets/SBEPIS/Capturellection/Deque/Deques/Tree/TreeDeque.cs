using System;
using System.Linq;
using Cysharp.Threading.Tasks;
using SBEPIS.Capturellection.Storage;
using SBEPIS.Utils;
using UnityEngine;

namespace SBEPIS.Capturellection.Deques
{
	public class TreeDeque : LaidOutDeque<TreeSettings, TreeLayout, TreeState>
	{
		[SerializeField] private GameObject keyboardPrefab;
		
		private Keyboard keyboard;
		
		protected override void InitPageOnce(TreeState state, DiajectorPage page)
		{
			keyboard = Instantiate(keyboardPrefab, page.transform).GetComponentInChildren<Keyboard>();
		}
		
		public override bool CanFetch(TreeState state, InventoryStorable card) => state.Tree.ContainsValue(StorableWithCard(state, card));
		
		public override UniTask<StoreResult> StoreItem(TreeState state, Capturellectable item)
		{
			if (TryStoreInExistingTreeSlotIfKeyExists(out UniTask<StoreResult> res)) return res;
			else if (TryStoreInEmptyCard(out res)) return res;
			else return EjectWholeTreeAndStoreInEmptyCard();
			
			bool TryStoreInExistingTreeSlotIfKeyExists(out UniTask<StoreResult> result)
			{
				if (state.Tree.TryGetValue(keyboard.Text, out Storable storable))
				{
					result = storable.StoreItem(item);
					return true;
				}
				else
				{
					result = default;
					return false;
				}
			}
			
			bool TryStoreInEmptyCard(out UniTask<StoreResult> result)
			{
				Storable storable = state.Inventory.Except(state.Tree).FirstOrDefault(storable => !storable.HasAllCardsFull);
				if (storable != null)
				{
					result = storable.StoreItem(item);
					return true;
				}
				else
				{
					result = default;
					return false;
				}
			}
			
			UniTask<StoreResult> EjectWholeTreeAndStoreInEmptyCard()
			{
				foreach (Storable droppedStorable in state.Tree.DropRoot(Settings.Balance))
					droppedStorable.Eject();
				
				return state.Inventory.First(storable => !storable.HasAllCardsFull).StoreItem(item);
			}
		}
		
		public override async UniTask<FetchResult> FetchItem(TreeState state, InventoryStorable card)
		{
			Storable storable = StorableWithCard(state, card);
			FetchResult result = await storable.FetchItem(card);
			if (storable.HasAllCardsEmpty)
				foreach (Storable droppedStorable in state.Tree.Drop(storable, Settings.Balance))
				{
					Debug.Log($"Ejecting {droppedStorable.First().name}");
					droppedStorable.Eject();
				}
			
			return result;
		}
		
		public override UniTask<StoreResult> StoreItemHook(TreeState state, Capturellectable item, StoreResult oldResult)
		{
			Storable storable = StorableWithCard(state, oldResult.card);
			if (!state.Tree.ContainsValue(storable))
			{
				state.Tree.Add(keyboard.Text, storable, Settings.Balance);
				keyboard.Text = "";
			}
			return UniTask.FromResult(oldResult);
		}
	}
	
	public class TreeState : InventoryState, DirectionState, TreeDictionaryState
	{
		public CallbackList<Storable> Inventory { get; set; } = new();
		public Vector3 Direction { get; set; }
		public TreeDictionary<string, Storable> Tree { get; } = new();
	}
	
	public interface TreeBalanceSettings
	{
		public bool Balance { get; set; }
	}
	
	[Serializable]
	public class TreeSettings : LayoutSettings<TreeLayout>, TreeBalanceSettings
	{
		[SerializeField] private TreeLayout layout;
		public TreeLayout Layout => layout;
		[SerializeField] private bool balance = true;
		public bool Balance
		{
			get => balance;
			set => balance = value;
		}
	}
}
